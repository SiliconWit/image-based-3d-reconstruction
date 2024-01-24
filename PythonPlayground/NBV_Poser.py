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


        mask = create_mask(img_col)
        self.img_col_m = mask_out(mask, img_col)
        self.img_dep_m = mask_out(mask, self.img_dep)

        self.Initialise_Params()
        self.intrinsics = self.CalcIntrinsics()

    def Initialise_Params(self):
        # Calculate Occupancy
        area = self.img_dep_m[self.img_dep_m > 0].size
        occupancy = area/self.img_dep_m.size
        self.occ_R = int(np.sqrt(area / np.pi))
        
        # Initialise Sigma Values
        self.min_sigma = int(self.occ_R*.05)
        self.max_sigma = int(self.occ_R*.15)


    def CalcIntrinsics(self, focal_l=6, sensor_width=6.287):
        img_width = self.img_col.shape[1]
        img_height = self.img_col.shape[0]
        fx, fy = (focal_l *  img_width/sensor_width, focal_l *  img_height/sensor_width)
        cx, cy = ( img_width/2, img_height/2 )
        return o3d.camera.PinholeCameraIntrinsic(img_width, img_height, fx, fy, cx, cy)
    
    def RegionsofInterest(self):
        edge_map = adjust_log(sobel(self.img_dep_m, mask=None,  axis=[0,1]))

        # Compute blobs from the Depth Map
        depth_map_blobs = blob_dog(self.img_dep_m, min_sigma=self.min_sigma, max_sigma=self.max_sigma, threshold=.1)

        # Compute blobs from the Edge Map
        edge_map_blobs = blob_dog(edge_map, min_sigma=self.min_sigma, max_sigma=self.max_sigma, threshold=.001)
        
        all_blobs = np.concatenate( (depth_map_blobs, edge_map_blobs) )

        selected_bandwidth = self.max_sigma * np.pi

        # Applying Mean Shift Clustering
        meanshift = MeanShift(bandwidth=selected_bandwidth)
        meanshift.fit(all_blobs[:,:2])
        labels = meanshift.labels_
        cluster_centers = meanshift.cluster_centers_
        n_clusters = len(cluster_centers)

        blobs_dict = dict()

        for label, blob in zip(labels, all_blobs):
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
        # Get Baselines for Blob Radius and Primar Pose Distance
        # WD = np.min(self.img_dep_m[self.img_dep_m > 0])
        WD = np.min(self.img_dep_m[self.img_dep_m > 0])
        Rad = np.max(self.blobs[:,2])

        self.viewpoint_points, self.viewpoint_normals = create_point_cloud( [ self.img_col_m, self.img_dep_m ], self.intrinsics )
        self.viewpoint_centroid = np.mean(self.viewpoint_points, axis=0)
        self.local_poses = list()

        for blob in self.blobs:
            # Get Blob Centroid and Normal
            roi_col = isolate_image( self.img_col_m, blob )
            roi_dep = isolate_image( self.img_dep_m, blob )
            points, normals = create_point_cloud( [ roi_col, roi_dep ], self.intrinsics )
            centroid, normal = retrieve_normal(points, normals)
            

            # Calculate Primary Pose Position and Orientation
            wd = np.clip( (int(blob[2])/Rad) * WD, WD/2, WD )
            priPose_loc = (normal * wd) + centroid
            priPose_rot = normalise_vector(centroid - priPose_loc)
            self.local_poses.append({ "pos" : priPose_loc, "rot" : priPose_rot })


            # Calculate Angle Delta
            v1 = priPose_loc - centroid
            v2 = self.viewpoint_centroid - centroid
            dot_product = np.dot(v1, v2)
            mag_v1 = np.linalg.norm(v1)
            mag_v2 = np.linalg.norm(v2)
            angle_radians = np.arccos(dot_product / (mag_v1 * mag_v2))

            # Calculate Secondary Pose
            normal = np.cross(v1, v2)
            rot_axis = normal / np.linalg.norm(normal)
            theta = -(np.pi - angle_radians)/2
            quat = R.from_rotvec(theta * rot_axis) # Create a quaternion for the rotation
            secPose_loc = quat.apply(v1) + centroid
            secPose_rot = normalise_vector( centroid - secPose_loc )
            self.local_poses.append({ "pos" : secPose_loc, "rot" : secPose_rot })
    
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
        logging.info(f"Camera Rotation: {cam_rot}")
        cam_poses = v3r_pb2.CamPoses()
        for pose in self.local_poses:
            local_rot_quat = self.vector_to_quaternion(pose['rot'])
            _pos = cam_pos + pose['pos']
            _rot = (R.from_quat(cam_rot) * R.from_quat(local_rot_quat)).as_quat()
            # _pos = pose['pos']
            # _rot = R.from_quat(local_rot_quat).as_quat()
            _location = v3r_pb2.Vec3(x=_pos[0], y=_pos[1], z=_pos[2])
            _orientation = v3r_pb2.Quat(
                w=_rot[3],
                x=_rot[0],
                y=_rot[1],
                z=_rot[2]
            )
            
            cam_poses.Poses.append( v3r_pb2.CamPose(
                location=_location,
                orientation=_orientation
            ) )
            # Poses.append({ "pos" : _pot, "rot" : _rot })
        return cam_poses
        


