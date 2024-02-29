import cv2
from rembg import remove, new_session
from sklearn.cluster import MeanShift
import torch
from zoedepth.models.builder import build_model
from zoedepth.utils.config import get_config
import matplotlib.pyplot as plt
import numpy as np
from midas.model_loader import load_model
from qreader import QReader
import gc         # garbage collect library
import logging

from skimage.feature import blob_dog
from skimage.filters import sobel
from skimage.exposure import adjust_log

from scipy.ndimage import binary_erosion


logging.basicConfig(level=logging.INFO)

def load_image(img_name, img_dir="testImages/"):
    """
    Loads in an image from the file system

    Args:
    - img_name (string): File name of the image.
    - img_dir (string): The relative directory in which the image is contained.

    Returns:
    - numpy.ndarray: The image as a 3D numpy array
    """
    return cv2.cvtColor(cv2.imread(img_dir+img_name), cv2.COLOR_BGR2RGB)

def compute_depth(image, zoe, dev="cpu"):
    """
    Loads in an image from the file system

    Args:
    - img_name (string): File name of the image.
    - img_dir (string): The relative directory in which the image is contained.

    Returns:
    - numpy.ndarray: The image as a 2D numpy array
    """
    DEVICE = "cuda" if dev == "cuda" and torch.cuda.is_available() else "cpu"
    zoe.to(DEVICE)
    depth_numpy = zoe.infer_pil(image)  # as numpy
    zoe.to('cpu')
    torch.cuda.empty_cache()  # Clear unused cached memory
    return depth_numpy

def compute_depth_standalone(image, dev="cpu"):
    """
    Loads in an image from the file system

    Args:
    - img_name (string): File name of the image.
    - img_dir (string): The relative directory in which the image is contained.

    Returns:
    - numpy.ndarray: The image as a 2D numpy array
    """
    if 'zoe' not in globals():
        conf = get_config("zoedepth", "infer")
        zoe = build_model(conf)

    DEVICE = "cuda" if dev == "cuda" and torch.cuda.is_available() else "cpu"
    zoe.to(DEVICE)
    depth_numpy = zoe.infer_pil(image)  # as numpy
    zoe.to('cpu')
    torch.cuda.empty_cache()  # Clear unused cached memory
    return depth_numpy

def show_image(image, label, colorbar=True):
    """
    Display an image with an optional colorbar.

    Parameters:
    image : array-like
        The image data. This can be any array-like object that is interpretable by `imshow`.
    label : str
        The title label for the image. This text will be displayed above the image.
    colorbar : bool, optional
        A flag to indicate whether a colorbar should be displayed alongside the image.
        If True (default), a colorbar is displayed. If False, no colorbar is shown.

    """
    _, ax = plt.subplots(layout="constrained")
    imgPlot = ax.imshow(image)
    ax.set_title(label)
    if colorbar: plt.colorbar( imgPlot, ax=ax )

def display_image(image):
    if image is None or len(image.shape) < 2:
        print("Could not read the image. Please check the file path or type.")
        return

    # Display the image in a window
    cv2.imshow('Image', image)

    # Wait for a key press and close the window when a key is pressed
    cv2.waitKey(0)
    cv2.destroyAllWindows()        

def create_mask(image, post_process=True):
    '''
        Create a Mask out of an image
    '''
    model_name = "u2net" # sam, u2net, silueta, isnet-general-use
    session = new_session(model_name)
    mask = remove(image, only_mask=True, post_process_mask=post_process)
    return mask
    
def mask_out(mask, _img):
    '''
        Masks out part of the image
    '''
    to_mask= np.copy(_img) # create a copy of the depth map
    to_mask[mask == 0] = 0
    return to_mask

def grayscale_to_rgb(grayscale_image):
    """
    Convert a grayscale image to an RGB image by replicating the grayscale values across all three channels.

    Parameters:
    - grayscale_image (numpy.ndarray): The input grayscale image.

    Returns:
    - rgb_image (numpy.ndarray): The resulting RGB image.
    """
    # Stack the grayscale image across three channels
    rgb_image = np.stack((grayscale_image,) * 3, axis=-1)
    return rgb_image



def process(image, model, model_type, device, target_size, optimize):
    """
    Run the inference and interpolate.

    Args:
        device (torch.device): the torch device used
        model: the model used for inference
        model_type: the type of the model
        image: the image fed into the neural network
        input_size: the size (width, height) of the neural network input (for OpenVINO)
        target_size: the size (width, height) the neural network output is interpolated to
        optimize: optimize the model to half-floats on CUDA?

    Returns:
        the prediction
    """
    sample = torch.from_numpy(image).to(device).unsqueeze(0)

    if optimize and device == torch.device("cuda"):
        print("  Optimization to half-floats activated. Use with caution, because models like Swin require\n"
                    "  float precision to work properly and may yield non-finite depth values to some extent for\n"
                    "  half-floats.")
        sample = sample.to(memory_format=torch.channels_last)
        sample = sample.half()

    prediction = model.forward(sample)
    prediction = (
        torch.nn.functional.interpolate(
            prediction.unsqueeze(1),
            size=target_size[::-1],
            mode="bicubic",
            align_corners=False,
        )
        .squeeze()
        .cpu()
        .numpy()
    )
    
    gc.collect()
    torch.cuda.empty_cache() 

    return prediction

def compute_depth_relative(image, device="cpu"):
    # device = torch.device("cuda" if torch.cuda.is_available() else "cpu")
    # device = "cpu"
    logging.info("Device: %s" % device)
    # model_type = "dpt_beit_large_384"
    model_type = "dpt_swin2_large_384"
    # model_type = "dpt_beit_large_512"
    logging.info("Model: %s" % model_type)
    model_path = f"Models/{model_type}.pt"
    model, transform, net_w, net_h = load_model(
        device, 
        model_path, 
        model_type, 
        optimize=True, 
        height=None, 
        square=False
    )

    original_image_rgb = image/255.0
    image = transform({"image": original_image_rgb})["image"]

    with torch.no_grad():
        prediction = process(image,
                             model, 
                             model_type, 
                             device,  
                             original_image_rgb.shape[1::-1],
                             True
                        )
    return prediction

def linear_regression_coefficients(D, P):
    # Reshape D to a column vector
    D = D.reshape(-1, 1)

    # Create the design matrix
    X = np.hstack((D, np.ones_like(D)))

    # Perform linear regression to find coefficients s and t
    coefficients, _, _, _ = np.linalg.lstsq(X, P, rcond=None)

    # Extract coefficients
    s, t = coefficients

    return s, t

def compute_depth_midas(img, cam_pose):
    depth_map  = compute_depth_relative(img).astype(np.float32)
    qreader = QReader()
    res = qreader.detect_and_decode(image=img,return_detections = True )
    detections = [index for index, element in enumerate(res[0]) if element != None]

    points = list()
    for detection in detections:
        location = np.asarray([float(val) for val in res[0][detection].split(',')]) - cam_pose
        x, y = [ int(px_i) for px_i in res[1][detection]["cxcy"] ]
        points.append({
            # "depth"   : np.round(np.linalg.norm(location),6),
            # "pixel"   : [ int(px_i) for px_i in res[1][detection]["cxcy"] ],
            "pred"    : depth_map[y,x],
            "inv_dep" : 1/np.linalg.norm(location)
            # "inv_dep" : np.round(1/np.linalg.norm(location),6)
        })

    # Using Least Squares
    D = np.array([pt['inv_dep'] for pt in points])
    P = np.array([pt['pred'] for pt in points])

    s, t = linear_regression_coefficients(D, P)

    t_arr = np.full(img.shape[:2],t)
    abs_depth = 1/((depth_map - t_arr)/s)

    return abs_depth.astype(np.float32)
    
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
    

def extract_roi(img_rgb, blob_details):    
        
    y, x, r = blob_details
    y, x, r = int(y), int(x), int(r)
    
    # Ensure the coordinates and radius do not go beyond image boundaries
    x_min = max(x - r, 0)
    x_max = min(x + r, img_rgb.shape[1])
    y_min = max(y - r, 0)
    y_max = min(y + r, img_rgb.shape[0])    
    
    # Extract the region
    extracted_region = img_rgb[y_min:y_max, x_min:x_max]
    
    return extracted_region

def shrink_mask(mask, pixels_to_shrink):
    mask = np.asarray(mask, dtype=bool) # Ensure the input mask is binary
    structure = np.ones((2 * pixels_to_shrink + 1, 2 * pixels_to_shrink + 1), dtype=bool) # Create a structuring element for binary erosion
    shrunk_mask = binary_erosion(mask, structure=structure) # Erode the binary mask
    
    return shrunk_mask

def filter_blobs_by_mask(blobs, mask):
    # Extract x, y coordinates and sizes from blobs
    x, y, sizes = blobs[:, 0].astype(int), blobs[:, 1].astype(int), blobs[:, 2]
    
    # Ensure that the indices are within the mask boundaries
    x = np.clip(x, 0, mask.shape[1] - 1)
    y = np.clip(y, 0, mask.shape[0] - 1)
    
    # Get the indices of blobs that fall within the mask region
    indices_inside_mask = mask[y, x]
    
    # Filter blobs based on the mask
    filtered_blobs = blobs[indices_inside_mask]
    
    return filtered_blobs

def ComputeFeatures(image_dep_m, occ_R, mask):
    
    min_sigma = int(occ_R*.05)
    max_sigma = int(occ_R*.15)
    # Compute the Edge map
    edge_map = adjust_log(sobel(image_dep_m, mask=None,  axis=[0,1]))

    # Compute blobs from the Depth Map
    depth_map_blobs = blob_dog(image_dep_m, min_sigma=min_sigma, max_sigma=max_sigma, threshold=.1)

    # Compute blobs from the Edge Map
    edge_map_blobs = blob_dog(edge_map, min_sigma=min_sigma, max_sigma=max_sigma, threshold=.001)


    fig, axs = plt.subplots(1, 3, figsize=(9.6,3), layout="constrained")


    all_blobs = np.concatenate( (depth_map_blobs, edge_map_blobs) )

    shrunk_mask = shrink_mask(mask, int(min_sigma*2))

    filtered_blobs = filter_blobs_by_mask(all_blobs, shrunk_mask)

    selected_bandwidth = max_sigma * 4/3

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
    return np.column_stack([cluster_centers, np.array( list(blobs_radii.values()), dtype=float )])
