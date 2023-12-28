import cv2
from rembg import remove, new_session
import torch
from zoedepth.models.builder import build_model
from zoedepth.utils.config import get_config
import matplotlib.pyplot as plt
import numpy as np

def load_image(img_name, img_dir="testImages/"):
    """
    Loads in an image from the file system

    Args:
    - img_name (string): File name of the image.
    - img_dir (string): The relative directory in which the image is contained.

    Returns:
    - numpy.ndarray: The image as a 2D numpy array
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
        

def create_mask(image):
    '''
        Create a Mask out of an image
    '''
    model_name = "u2net" # sam, u2net, silueta, isnet-general-use
    session = new_session(model_name)
    mask = remove(image, only_mask=True, post_process_mask=True)
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