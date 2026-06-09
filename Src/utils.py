#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
Utility functions for Depth Guided Next-Best-View Planning

This module provides helper functions for image processing, point cloud generation,
pose calculation, and visualization for NBV planning.

Author: Andrew Chesang
Date: February 2026
"""

# Copyright 2026 Andrew Chesang
#
# Licensed under the Apache License, Version 2.0 (the "License");
# you may not use this file except in compliance with the License.
# You may obtain a copy of the License at
#
#     http://www.apache.org/licenses/LICENSE-2.0
#
# Unless required by applicable law or agreed to in writing, software
# distributed under the License is distributed on an "AS IS" BASIS,
# WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
# See the License for the specific language governing permissions and
# limitations under the License.

import numpy as np
import matplotlib.pyplot as plt
import matplotlib.patheffects as PathEffects
from skimage.filters import sobel
from skimage.feature import blob_dog, blob_doh
from rembg import remove, new_session
import open3d as o3d
from sklearn.cluster import MeanShift
from scipy.spatial.transform import Rotation as R
import pyvista as pv

def add_patches_to_plot(ax, blob_list, index=False):
    """Add circular patches to matplotlib axis for each blob."""
    for idx, blob in enumerate(blob_list):
        y, x, r = blob
        c = plt.Circle((x, y), r, color='red', linewidth=2, fill=False)
        ax.add_patch(c)
        if index:
            txt = ax.text(x, y, str(idx), color='white', fontsize=12, 
                ha='center', va='center')
            txt.set_path_effects([
                PathEffects.withStroke(linewidth=3, foreground='black')
            ])

normalize_depths = lambda dep_img: (dep_img - dep_img.min()) / (dep_img.max() - dep_img.min())

def create_mask(image, post_process=True):
    """Create foreground mask using rembg."""
    model_name = "sam"
    session = new_session(model_name)
    mask = remove(image, only_mask=True, post_process_mask=post_process)
    norm_mask = (mask - mask.min()) / (mask.max() - mask.min())
    return mask

def mask_out(mask, _img):
    """Apply mask to image."""
    to_mask = np.copy(_img)
    to_mask[mask == 0] = 0
    return to_mask

def filter_blobs(blobs, mask):
    """Filter blobs to keep only those within the mask."""
    filtered_blobs = [blob for blob in blobs if mask[int(blob[0]), int(blob[1])] > 0]
    return filtered_blobs

def compute_blobs(img, min_sigma, max_sigma):
    """Detect blobs using Difference of Gaussians."""
    return blob_dog(img, min_sigma=min_sigma, max_sigma=max_sigma, threshold=.001)

def cluster_blobs(filtered_blobs, bandwidth):
    """
    Cluster blobs using Mean Shift algorithm and compute cluster radii.

    Args:
        filtered_blobs: Array of blobs (y, x, radius)
        bandwidth: Bandwidth parameter for Mean Shift clustering

    Returns:
        blobs: Array of clustered blobs (y, x, radius)
        labels: Cluster labels for each input blob
        n_clusters: Number of clusters found
    """
    meanshift = MeanShift(bandwidth=bandwidth)
    meanshift.fit(filtered_blobs[:, :2])
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
    for key, blobs_list in blobs_dict.items():
        _blobs = np.asarray(blobs_list)
        if len(blobs_list) == 1:
            radius = blobs_list[0][2]
        else:
            x_len = max(_blobs[:, 0]) - min(_blobs[:, 0])
            y_len = max(_blobs[:, 1]) - min(_blobs[:, 1])
            radius = np.sqrt(y_len**2 + x_len**2) / 2 + np.mean(_blobs[:, 2:])

        blobs_radii[key] = radius

    blobs = np.column_stack([cluster_centers, np.array(list(blobs_radii.values()), dtype=float)])

    return blobs, labels, n_clusters

def isolate_image(image, roi):
    """Extract ROI from image based on blob."""
    y, x, radius = roi

    top_y = max(int(y - radius), 0)
    left_x = max(int(x - radius), 0)
    bottom_y = min(int(y + radius), image.shape[0])
    right_x = min(int(x + radius), image.shape[1])
    masked = np.zeros_like(image)    
    cropped_image = image[top_y:bottom_y, left_x:right_x]
    masked[top_y:bottom_y, left_x:right_x] = cropped_image

    return masked

def extract_edge(img):
    """Apply Sobel edge detection."""
    edges = sobel(img)
    return edges

def create_point_cloud(extractions, intrinsics,  focal_l = 6):
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
    
#     intrinsics = o3d.camera.PinholeCameraIntrinsic(o3d.camera.PinholeCameraIntrinsicParameters.PrimeSenseDefault)
    # Create Point Cloud
    pcd = o3d.geometry.PointCloud.create_from_rgbd_image( rgbd_image_m, intrinsics )

    # Estimate Normals
    pcd.estimate_normals( search_param=o3d.geometry.KDTreeSearchParamHybrid(radius=.1, max_nn=30))
    pcd.orient_normals_towards_camera_location()

    # Create Numpy Arrays for the points and corresponding normals.
    pcd_p = np.asarray(pcd.points)
    pcd_n = np.asarray(pcd.normals)
    
    return pcd, pcd_p, pcd_n


def retrieve_normal(points, normals):
    # Calculate the centroid
    centroid = np.mean(points, axis=0)
    normal = np.mean(normals, axis=0)

    # Find the index of the point closest to the centroid
    distances = np.linalg.norm(points - centroid, axis=1)
    closest_point_index = np.argmin(distances)

    # Get the normal at the closest point
    closest_normal = normals[closest_point_index]
    
    return centroid, (closest_normal+normal)/2

def add_normal_to_plotter(plotter, point, normal, col="red"):
    arrow = pv.Arrow(start=point, direction=normal, scale=.2)  # Adjust scale as needed
    plotter.add_mesh(arrow, color=col)

def normalise_vector(vec):
    norm = np.linalg.norm(vec)
    if norm == 0: 
        normalized_v = vec
    else:
        normalized_v = vec / norm
    
    return normalized_v
 

def angle_difference(v1, v2):
    '''
        Returns: angle_degrees and angle_radians
    '''
    dot_product = np.dot(v1, v2)

    magnitude_v1 = np.linalg.norm(v1)
    magnitude_v2 = np.linalg.norm(v2)

    # Calculate the cosine of the angle between the vectors
    cosine_theta = dot_product / (magnitude_v1 * magnitude_v2)

    # Use arccosine to get the angle in radians
    angle_radians = np.arccos(cosine_theta)

    # Convert the angle to degrees
    angle_degrees = np.degrees(angle_radians)

    return angle_degrees, angle_radians
