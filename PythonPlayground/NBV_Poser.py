import open3d as o3d
from sklearn.cluster import MeanShift
from common import *
from skimage.exposure import adjust_log
from skimage.filters import sobel
from skimage.feature import blob_dog
from NBV_3d_structs import *
from scipy.spatial.transform import Rotation as R



 
import v3r_pb2
import logging

class NBVPoser():
    def __init__(self, img_col, ref_pose:v3r_pb2.CamPose, zoe) -> None:
        self.img_col = img_col
        self.ref_pos = ref_pose
        self.cam_pos = np.array([
            ref_pose.location.x,
            ref_pose.location.y,
            ref_pose.location.z
        ])
        if zoe == None:
            self.img_dep = compute_depth_midas(img_col, self.cam_pos)
        else:
            self.img_dep = compute_depth(img_col, zoe)


        self.mask = create_mask(img_col)
        self.img_col_m = mask_out(self.mask, img_col)
        self.img_dep_m = mask_out(self.mask, self.img_dep)

        self.Initialise_Params()
        self.intrinsics = self.CalcIntrinsics()
        self.camera_offset = 0.1

    def Initialise_Params(self):
        # Calculate Occupancy
        area = self.img_dep_m[self.img_dep_m > 0].size
        occupancy = area/self.img_dep_m.size
        self.occ_R = int(np.sqrt(area / np.pi))
        
        # Initialise Sigma Values
        self.min_sigma = int(self.occ_R*.05)
        self.max_sigma = int(self.occ_R*.25)


    def CalcIntrinsics(self, focal_l=6, sensor_width=6.287):
        img_width = self.img_col.shape[1]
        img_height = self.img_col.shape[0]
        fx, fy = (focal_l *  img_width/sensor_width, focal_l *  img_height/sensor_width)
        cx, cy = ( img_width/2, img_height/2 )
        return o3d.camera.PinholeCameraIntrinsic(img_width, img_height, fx, fy, cx, cy)
    
    def RegionsofInterest(self):
        edge_map = adjust_log(sobel(self.img_dep, mask=None,  axis=[0,1]))

        # Compute blobs from the Depth Map
        depth_map_blobs = blob_dog(self.img_dep, min_sigma=self.min_sigma, max_sigma=self.max_sigma, overlap=0.1, threshold=.001)

        # Compute blobs from the Edge Map
        edge_map_blobs = blob_dog(edge_map, min_sigma=self.min_sigma, max_sigma=self.max_sigma, overlap=0.1, threshold=.001)
        
        all_blobs = np.concatenate( (depth_map_blobs, edge_map_blobs) )

        selected_bandwidth = (self.max_sigma+self.min_sigma) * 1

        filtered_blobs = np.asarray(filter_blobs(all_blobs, self.mask))

        # Applying Mean Shift Clustering
        meanshift = MeanShift(bandwidth=selected_bandwidth)
        meanshift.fit(filtered_blobs[:,:2])
        labels = meanshift.labels_
        cluster_centers = meanshift.cluster_centers_
        n_clusters = len(cluster_centers)

        blobs_dict = dict()

        for label, blob in zip(labels, filtered_blobs):
            if label in blobs_dict:
                blobs_dict[label].append(blob)
            else:
                blobs_dict[label] = [blob]

        blobs_radii = {}
        # Loop over Clusters
        for key, blobs in blobs_dict.items():
            _blobs = np.asarray(blobs)    
            if( len(blobs) == 1 ):
                radius = blob[2]
            else:
                x_len = max(_blobs[:,0]) - min(_blobs[:,0])
                y_len = max(_blobs[:,1]) - min(_blobs[:,1])
                radius =  np.sqrt( y_len**2 + x_len**2)/2 + np.mean(_blobs[:,2:])
            blobs_radii[key] = radius 
            
        # Form a Blob Structure (x,y radius)
        self.blobs = np.column_stack([cluster_centers, np.array( list(blobs_radii.values()), dtype=float )])

    def Structuring(self):
        
        self.viewpoint_pcd, self.viewpoint_points, self.viewpoint_normals = create_point_cloud( [ self.img_col_m, self.img_dep_m ], self.intrinsics )
        self.viewpoint_centroid = np.mean(self.viewpoint_points, axis=0)
        
        # Get Baselines for Blob Radius and Primar Pose Distance
        eds = np.sqrt(np.sum((self.viewpoint_points - self.viewpoint_centroid)**2, axis=1))
        ed_mean = np.mean(eds) 
        ed_stdv = np.std(eds)
        min_wd = ed_mean + (2 * ed_stdv)

        WD = np.min(self.img_dep_m[self.img_dep_m > 0])
        Rad = int(np.max(self.blobs[:,2]))
    
        y_min = np.ceil( np.min(self.viewpoint_points[:,1]) * 100)/100 + self.camera_offset

        self.local_poses = []



        for blob in self.blobs:
            # Get Blob Centroid and Normal
            roi_col = isolate_image( self.img_col_m, blob )
            roi_dep = isolate_image( self.img_dep_m, blob )
            pcd, points, normals = create_point_cloud( [ roi_col, roi_dep ], self.intrinsics )
            centroid, normal = retrieve_normal(points, normals)
            

            # Calculate Primary Pose Position and Orientation
            blob_size_s = int(blob[2])/Rad
            wd = np.clip( blob_size_s * WD, ed_mean + (ed_stdv*2), WD )
            priPose_loc = (normal * wd) + centroid
            
            # Avoid Floor Collision
            if priPose_loc[1] < y_min:
                logging.info(f"Point: {priPose_loc} is below the floor at {y_min}" )
                priPose_loc[1] = y_min
                logging.info(f"New Position: {priPose_loc}")
            priPose_rot = normalise_vector(centroid - priPose_loc)
            priPose_eul = angle_difference_on_planes(priPose_rot)
            self.local_poses.append({ "pos" : priPose_loc, "rot" : priPose_rot,"eul": priPose_eul })

            # Get Secondary Pose
            v1 = priPose_loc - centroid
            v2 = self.viewpoint_centroid - centroid
            _, internal_angle = angle_difference(v1, v2)
            delta_angle = np.pi - internal_angle
            
            # Calculate Secondary Pose
            normal = np.cross(v1, v2)
            rot_axis = normal / np.linalg.norm(normal)
            theta = -np.pi/12
            
            quat = R.from_rotvec(theta * rot_axis) # Create a quaternion for the rotation
            secPose_loc = quat.apply(v1) + centroid
            
            if secPose_loc[1] < y_min:
                logging.info(f"Point: {secPose_loc} is below the floor at {y_min}" )
                secPose_loc[1] = y_min
                
                v3 = secPose_loc - centroid
                v4 = priPose_loc - centroid
                _deg, delta_angle2 = angle_difference(v3, v4)
                
                logging.info(f"New Angle Delta: {_deg}")
                
                rot_axis2 = np.cross(v3, v4)
                rot_axis2 = rot_axis2/ np.linalg.norm(rot_axis2)
                quat2 = R.from_rotvec( (theta - delta_angle2)/2  * rot_axis2) # Create a quaternion for the rotation
                secPose_loc = quat2.apply(v3) + centroid
                secPose_loc[1] = y_min
                
            secPose_rot = normalise_vector( centroid - secPose_loc )
            secPose_eul = angle_difference_on_planes(secPose_rot)
            self.local_poses.append({ "pos" : secPose_loc, "rot" : secPose_rot, "eul": secPose_eul })
    
    def vector_to_quaternion(self,dir_vec):           
        forward_vec = np.array([0, 0, 1])  # Assuming Z-axis is forward
        cross_prod = np.cross(forward_vec, dir_vec)
        dot_prod = np.dot(forward_vec, dir_vec)
        x, y, z = cross_prod
        w = np.sqrt(np.linalg.norm(forward_vec)**2 * np.linalg.norm(dir_vec)**2) + dot_prod
        quat = np.array([x, y, z, w])
        return quat / np.linalg.norm(quat)
    
    def retransform_poses(self) -> v3r_pb2.CamPoses:
        cam_pos = [
            self.ref_pos.location.x, 
            self.ref_pos.location.y, 
            self.ref_pos.location.z 
        ]
        logging.info(f"Camera Position: {cam_pos}")

        cam_rot = [
            self.ref_pos.orientation.x,
            self.ref_pos.orientation.y,
            self.ref_pos.orientation.z,
            self.ref_pos.orientation.w
        ]

        # Invert camera rotation (assuming right-handed coordinate system)
        inv_rotation_matrix_unity = R.from_quat(cam_rot).inv().as_matrix()


        logging.info(f"Camera Rotation: {cam_rot}")
        cam_poses = v3r_pb2.CamPoses()
        for pose in self.local_poses:
            local_rot_quat = self.vector_to_quaternion(pose['rot'])
            # _pos = cam_pos + pose['pos'] # Initial Method
            _pos = inv_rotation_matrix_unity.dot(pose["pos"]) + cam_pos

            # _pos = cam_pos + pose['pos']
            # _rot = (R.from_quat(cam_rot) * R.from
            _rot = local_rot_quat
            # _rot = (R.from_quat(cam_rot) * R.from_quat(local_rot_quat)).as_quat()
            # _quat(local_rot_quat)).as_quat()
            # _rot = R.from_quat(local_rot_quat).as_quat()
            _location = v3r_pb2.Vec3(x=_pos[0], y=_pos[1], z=_pos[2])
            _orientation = v3r_pb2.Quat(
                w=_rot[3],
                x=_rot[0],
                y=_rot[1],
                z=_rot[2]
            )
            _eulers= v3r_pb2.Vec3(x=pose[0], y=pose[1], z=pose[2])
            cam_poses.Poses.append( v3r_pb2.CamPose(
                location=_location,
                orientation=_orientation,
                eulers=_eulers
            ) )
            # Poses.append({ "pos" : _pot, "rot" : _rot })
        return cam_poses
        
