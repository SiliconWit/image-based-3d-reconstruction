import logging
import numpy as np
import open3d as o3d
import matplotlib.pyplot as plt
import copy
import json

def find_center_pose(points):
    '''
    Find point closest to the centroid
    Args:
        points (nd.array(_,3)): A list of camera view point coordinates
    '''
    centroid = np.mean(points, axis=0)
    distances = np.linalg.norm(points - centroid, axis=1)
    closest_index = np.argmin(distances)
    closest_point = points[closest_index]
    return closest_point, closest_index

def transform_point_cloud(pcd, sfm_file):
    pcd = copy.deepcopy(pcd)
    f = open(sfm_file)
    data = json.load(f)

    centers = []
    for pose in data['poses']:
        centers.append(np.asarray([ round(float(x),3) for x in pose['pose']['transform']['center'] ]))

    center, index = find_center_pose(centers)
    rotation = data['poses'][index]['pose']['transform']['rotation']
    rotation_matrix = np.array( rotation ).reshape(3,3)
    transformation_matrix = np.eye(4)
    transformation_matrix[:3, :3] = rotation_matrix
    transformation_matrix[:3, 3] = np.array(center)

    
    pcd = pcd.transform(-transformation_matrix)
    new_pc = o3d.geometry.PointCloud()
    new_pc.points = o3d.utility.Vector3dVector(pcd.points)
    return new_pc,center

def calc_nn(pcd, normal_radius):
    bounding_box = pcd.get_axis_aligned_bounding_box()
    return int(bounding_box.volume()/normal_radius *np.pi *2)



def camera_sfm(sfm_file):
    f = open(sfm_file)
    data = json.load(f)

    cam_pts = []
    for pose in data['poses']:
        cam_pts.append(np.asarray([ round(float(x),3) for x in pose['pose']['transform']['center'] ]))
      
    cam_pts = np.asarray(cam_pts)
    cam_pc = o3d.geometry.PointCloud()
    cam_pts[:, 1] *= -1
    cam_pc.points = o3d.utility.Vector3dVector(cam_pts)
    cam_pc.scale(0.491,(0,0,0))
    
    center, _ = find_center_pose(np.asarray((cam_pc.points)))

    return center

def clean_pointcloud(pcd, scale=0.534, pivot=(0,0,0) ):
    pcd_points = np.asarray(pcd.points)

    raw_mean_dist = np.mean(pcd.compute_nearest_neighbor_distance())
    z_threshold = np.mean(pcd_points, axis=0)[2]

    pcd_points[:,1] *= -1
    ref_pc = o3d.geometry.PointCloud()
    ref_pc.points = o3d.utility.Vector3dVector(pcd_points)
    
    with o3d.utility.VerbosityContextManager(o3d.utility.VerbosityLevel.Debug) as cm:
        labels = np.array( ref_pc.cluster_dbscan(
            eps=raw_mean_dist*np.pi, 
            min_points=20, print_progress=True)
        )
    max_label = labels.max()
    logging.info(f"point cloud has {max_label + 1} clusters")
    colors = plt.get_cmap("tab20")(labels / (max_label if max_label > 0 else 1))
    colors[labels < 0] = 0
    ref_pc.colors = o3d.utility.Vector3dVector(colors[:, :3])
    
    # Extract all non-noisy points
    unique_labels = np.unique(labels[labels != -1])
    all_pcs = np.array([])
    
    for label in unique_labels:
        segment_indices = np.where(labels == label)[0]
        segment_points = np.asarray(ref_pc.points)[segment_indices]
        if( (np.mean(segment_points, axis=0)[2] - z_threshold) > raw_mean_dist*10*np.pi ):
            continue
        if( all_pcs.size == 0  ):
            all_pcs = segment_points
        else:
            all_pcs = np.concatenate((all_pcs, segment_points), axis=0)

    segments_pcd = o3d.geometry.PointCloud()
    segments_pcd.points = o3d.utility.Vector3dVector(all_pcs)
    segments_pcd.scale( scale, center=pivot )
    return segments_pcd

def min_dist(pcd, ref_point=(0,0,0), nn=5):
    pcd_tree = o3d.geometry.KDTreeFlann(pcd)
    point = np.array(ref_point)
    [k, idx, _] = pcd_tree.search_knn_vector_3d(point, nn)
    print(f"k: {k} \nidx: {idx}")
    centroid = np.mean(np.asarray(pcd.points)[idx], axis=0)
    dist = np.linalg.norm(ref_point - centroid)
    return dist, centroid

def min_dist(pcd, ref_point=(0,0,0), nn=5):
    pcd_tree = o3d.geometry.KDTreeFlann(pcd)
    pcd_points = np.asarray(pcd.points)
    
    closest_z = np.min(pcd_points[:, 2])
    point = [ ref_point[0], ref_point[1], closest_z/1.5 ]
    
#     point = np.array(ref_point)
    [k, idx, _] = pcd_tree.search_knn_vector_3d(point, nn)
    print(f"k: {k} \nidx: {idx}")
    centroid = np.mean(np.asarray(pcd.points)[idx], axis=0)
    dist = np.linalg.norm(ref_point - centroid)
    return dist, centroid

def create_point_cloud(extractions, intrinsics):
    img_rgb, img_dep = extractions
    
    # Scaling factor for the depth image
    _scale = 1000
    
    color_raw_m = o3d.geometry.Image(img_rgb)
    depth_raw_m =  o3d.geometry.Image(img_dep*_scale) 

    # Create RGBD 
    rgbd_image_m = o3d.geometry.RGBDImage.create_from_color_and_depth(
        color_raw_m, 
        depth_raw_m
    )

    # Create Point Cloud
    pcd = o3d.geometry.PointCloud.create_from_rgbd_image( rgbd_image_m, intrinsics )
    
#     pcd.transform([[1, 0, 0, 0], [0, -1, 0, 0], [0, 0, -1, 0], [0, 0, 0, 1]])
    pcd_points = np.asarray(pcd.points)
    pcd_points[:,1] *= -1
    inv_pc = o3d.geometry.PointCloud()
    inv_pc.points = o3d.utility.Vector3dVector(pcd_points)
    
    # Estimate Normals
    inv_pc.estimate_normals( search_param=o3d.geometry.KDTreeSearchParamHybrid(radius=.1, max_nn=30))
    inv_pc.orient_normals_towards_camera_location()

    # Create Numpy Arrays for the points and corresponding normals.
    pcd_p = np.asarray(inv_pc.points)
    pcd_n = np.asarray(inv_pc.normals)
    
    return inv_pc, pcd_p, pcd_n


def prep_pointcloud(pcd, normal_radius, fpfh_nn_multiplier=5):
    '''
        Computes the normals and fast point feature historgrams of a given point cloud
        
        Args:
            pcd (o3d.geometries.PointCloud): Point Cloud Object
            fpfh_nn_multiplier (int): Radius
        
        Returns:
            o3d.geometries.PointCloud: A point cloud object with normals
            open3d.pipelines.registration.Feature: FPFH of the input point cloud
    '''
    nearest_neighbours = calc_nn(pcd,normal_radius)
    
    # Estimate Normals
    pcd.estimate_normals(
        o3d.geometry.KDTreeSearchParamHybrid(radius=normal_radius, max_nn=nearest_neighbours))
    pcd.orient_normals_towards_camera_location()
    
    # Compute Fast Point Feature Histogram
    pcd_fpfh = o3d.pipelines.registration.compute_fpfh_feature( 
        pcd,
    o3d.geometry.KDTreeSearchParamHybrid(radius=normal_radius*1, max_nn=nearest_neighbours*fpfh_nn_multiplier))
    
    centroid = np.mean( np.asarray(pcd.points), axis=0 )
    normal = np.mean( np.asarray(pcd.normals), axis=0 )
    
    return pcd, pcd_fpfh

def execute_fast_global_registration(source_down, target_down, source_fpfh,
                                     target_fpfh, voxel_size):
    distance_threshold = voxel_size * 5
    print(":: Apply fast global registration with distance threshold %.3f" \
            % distance_threshold)
    result = o3d.pipelines.registration.registration_fgr_based_on_feature_matching(
        source_down, target_down, source_fpfh, target_fpfh,
        o3d.pipelines.registration.FastGlobalRegistrationOption(
            maximum_correspondence_distance=distance_threshold))
    return result

def get_normal(pcd, normal_radius):
    
    # estimate radius
    bounds = pcd.get_max_bound()-pcd.get_min_bound()
    _radius = np.linalg.norm(bounds)/7
    nearest_neighbours = int(len(pcd.points)/3)
    # Estimate Normals
    pcd.estimate_normals(
        o3d.geometry.KDTreeSearchParamHybrid(radius=_radius, max_nn=nearest_neighbours))
    pcd.orient_normals_towards_camera_location()

    centroid = np.mean( np.asarray(pcd.points), axis=0 )
    normal = np.mean( np.asarray(pcd.normals), axis=0 )

    return centroid, normal
    

def retrieve_normal(points, normals):
    # Calculate the centroid
    centroid = np.mean(points, axis=0)

    # Find the index of the point closest to the centroid
    # distances = np.linalg.norm(points - centroid, axis=1)
    # closest_point_index = np.argmin(distances)

    # # Get the normal at the closest point
    # closest_normal = normals[closest_point_index]
    normal = np.mean(normals, axis=0)
    
    return centroid, normal

def normalise_vector(vec):
    norm = np.linalg.norm(vec)
    if norm == 0: 
        normalized_v = vec
    else:
        normalized_v = vec / norm
    
    return normalized_v

def simple_transformation(pcd, scale, vector):
    pcd.scale(scale, (0,0,0))
    trans_matrix =np.identity(4)
    trans_matrix[:3,3] = vector
    pcd.transform(trans_matrix)
    return pcd
                       