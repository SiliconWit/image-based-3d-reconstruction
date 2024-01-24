from common import *
import open3d as o3d

def isolate_image(image, roi):
    # Extracting the center (y, x) and radius
    y, x, radius = roi
    
    # Calculating the square bounds
    top_y = max(int(y - radius), 0)
    left_x = max(int(x - radius), 0)
    bottom_y = min(int(y + radius), image.shape[0])
    right_x = min(int(x + radius), image.shape[1])

    # Create 
    masked = np.zeros_like(image)    
    cropped_image = image[top_y:bottom_y, left_x:right_x]
    masked[top_y:bottom_y, left_x:right_x] = cropped_image

    return masked

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

    # Estimate Normals
    pcd.estimate_normals( search_param=o3d.geometry.KDTreeSearchParamHybrid(radius=.1, max_nn=30))
    pcd.orient_normals_towards_camera_location()

    # Create Numpy Arrays for the points and corresponding normals.
    pcd_p = np.asarray(pcd.points)
    pcd_n = np.asarray(pcd.normals)
    
    return pcd_p, pcd_n

def retrieve_normal(points, normals):
    # Calculate the centroid
    centroid = np.mean(points, axis=0)

    # Find the index of the point closest to the centroid
    distances = np.linalg.norm(points - centroid, axis=1)
    closest_point_index = np.argmin(distances)

    # Get the normal at the closest point
    closest_normal = normals[closest_point_index]
    
    return centroid, closest_normal

def normalise_vector(vec):
    norm = np.linalg.norm(vec)
    if norm == 0: 
        normalized_v = vec
    else:
        normalized_v = vec / norm
    
    return normalized_v