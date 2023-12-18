Blob detection is a common feature extraction technique in computer vision used to identify and mark regions in an image that differ in properties, like brightness or color, compared to surrounding regions. These regions are typically referred to as "blobs". The goal of blob detection is to identify and mark these regions of interest in an image. There are several algorithms for blob detection, each with its own methodology and use cases. Here are some of the prominent ones:

1. **Laplacian of Gaussian (LoG)**:    
    - The LoG method involves applying the Laplacian operator to the image after smoothing it with a Gaussian filter.
    - This method is effective in detecting blobs where there is a rapid intensity change, and the Gaussian filter helps in suppressing noise.
    - The algorithm looks for zero crossings in the second derivative of the image to locate blobs.
2. **Difference of Gaussians (DoG)**:    
    - DoG is a simplification of LoG and is computationally more efficient.
    - It involves the subtraction of one blurred version of an original image from another, less blurred version of the original.
    - This method approximates the Laplacian of Gaussian, which detects blobs by identifying regions where the difference in Gaussian blurs varies.
3. **Determinant of Hessian (DoH)**:    
    - This method uses the determinant of the Hessian matrix to detect blob-like structures.
    - The Hessian matrix is used in this context to compute the second-order derivatives of the image intensity values.
    - The determinant of this matrix is then used to find regions in the image where the intensity changes significantly in two directions, which are indicative of blobs.
4. **Maximally Stable Extremal Regions (MSER)**:
    
    - MSER detects blobs as stable regions in an image over varying thresholds.
    - It identifies areas in an image that stay almost the same (are stable) over various levels of thresholding.
    - MSER is particularly effective in scenarios where blobs have uniform intensity and are distinct from their background.

```
import matplotlib.pyplot as plt
from skimage import feature, io, color
from skimage.feature import blob_dog, blob_log, blob_doh

def detect_blobs(image, blob_method='log', min_sigma=1, max_sigma=30, num_sigma=10, threshold=.1):
    """
    Perform blob detection on an image.

    Parameters:
    - image (numpy.ndarray): The input image in numpy array format.
    - blob_method (str): Method for blob detection ('log', 'dog', 'doh').
    - min_sigma (float): The minimum standard deviation for Gaussian Kernel. Used in blob detection.
    - max_sigma (float): The maximum standard deviation for Gaussian Kernel. Used in blob detection.
    - num_sigma (int): The number of intermediate values of standard deviations to consider between min_sigma and max_sigma.
    - threshold (float): The absolute lower bound for scale space maxima. Lower values mean more blobs.

    Returns:
    - blobs: A numpy array with each row representing 3 values for each blob: (y-coordinate, x-coordinate, radius)
    - Plots the image with blobs marked.
    """

    if blob_method == 'log':  # Laplacian of Gaussian
        blobs = blob_log(image, min_sigma=min_sigma, max_sigma=max_sigma, num_sigma=num_sigma, threshold=threshold)
    elif blob_method == 'dog':  # Difference of Gaussian
        blobs = blob_dog(image, min_sigma=min_sigma, max_sigma=max_sigma, threshold=threshold)
    elif blob_method == 'doh':  # Determinant of Hessian
        blobs = blob_doh(image, min_sigma=min_sigma, max_sigma=max_sigma, threshold=threshold)
    else:
        raise ValueError("Invalid blob detection method")

    # Plotting
    fig, ax = plt.subplots(figsize=(6, 6))
    ax.imshow(image, interpolation='nearest')
    for blob in blobs:
        y, x, r = blob
        c = plt.Circle((x, y), r, color='red', linewidth=2, fill=False)
        ax.add_patch(c)
    ax.set_title('Blob Detection')
    ax.axis('off')
    plt.show()

    return blobs

# Example usage:
# image = io.imread('path_to_your_image.jpg')
# blobs = detect_blobs(image)
```

## Parameter Explanation
1. **Min and Max Sigma**:    
    - These parameters define the range of the standard deviation (sigma) for the Gaussian kernel used in the blurring process.
    - **Min Sigma**: Sets the smallest size of the blobs that can be detected. Lower values detect smaller blobs.
    - **Max Sigma**: Sets the largest size of the blobs that can be detected. Higher values detect larger blobs.
    - These parameters essentially control the scale of the blobs that the algorithm will be sensitive to.
2. **Num Sigma (Number of Sigma Values)**:
    - This parameter determines how many intermediate values of sigma will be used between `min_sigma` and `max_sigma`.
    - A higher number provides a more detailed scale analysis but increases computational cost.
3. **Threshold**:
    
    - This value sets the threshold for the algorithm to decide whether a particular region of the image is a blob or not.
    - A higher threshold value results in fewer blobs being detected, as only those with stronger (more distinct) features are identified.
    - A lower threshold value may detect more blobs, including potentially false positives or less significant features.
4. **Overlap**:
    
    - In the context of blob detection, overlap refers to how much one blob can overlap with another.
    - This parameter is used to control whether two nearby blobs should be considered as one single blob or two separate blobs.
    - A lower value of overlap means that blobs that only slightly overlap will be regarded as separate blobs, while a higher value allows for more overlap before two blobs are considered one.
5. **Thresholding Method**:
    
    - Some blob detection algorithms allow you to specify a method for thresholding. This can be a fixed value, a dynamically calculated threshold, or other more sophisticated methods.
    - The choice of thresholding method can significantly affect the detection of blobs, especially in images with varying lighting conditions or contrast levels.
6. **Filtering by Area, Circularity, Convexity, and Inertia**:
    
    - These parameters are often used in blob detectors like OpenCV’s `SimpleBlobDetector`.
    - **They** allow the algorithm to filter out detected blobs based on their shape and size characteristics.
    - For example, you can specify to only detect blobs that are approximately circular, or within a certain size range, which can be crucial for specific applications.
### Parameters of Interest
- threshold -> to be determined programmatically
- min_sigma &rarr; a matter of resolution
- max_sigma -> matter of resolution

