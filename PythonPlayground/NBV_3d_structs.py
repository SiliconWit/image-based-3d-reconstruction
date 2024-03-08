from common import *
import open3d as o3d



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