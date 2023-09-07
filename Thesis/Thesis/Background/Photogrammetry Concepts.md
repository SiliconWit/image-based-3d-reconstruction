#sfm #camera-poses #incremental-sfm #descriptors

#### Definitions
**Definition of Photogrammetry**: Photogrammetry is a survey technique that allows for the building of 3D models starting from digitized output data (2D images) (Mikhail et al. 2001).

- The process of deriving metric information about an object from a 2-dimensional graphical image of the same object.
- The process of deriving metric information about an object through measurements of photos
- The inverse process of photography.



**Definition of SfM**: Local motion signals are determined through a technique called Structure-from-Motion (SfM) in which the camera is fixed and the target rotates or the camera moves around the fixed target (Schonberger and Frahm 2016).

Photogrammetry is afforbable, powerful, readily accessible and versaatile in appliocatioes where surface data acquisition is requried. It oofers recsonstruction results equivalen to other techniques that are more expensive and cumbersome. 
## Incremental Structure from Motion
Incremental Structure from Motion (SfM) is a computer vision technique used to reconstruct a 3D structure from a sequence of 2D images. The "motion" part of the name indicates that it often involves analyzing the relative motion of the camera and the scene across these images. The "incremental" part means that the 3D structure is built progressively, one image at a time.

Here's a brief overview of how Incremental SfM typically works:

1. **Feature Extraction**: For each image, detect key points (or features) and compute their descriptors. This can be done using methods such as SIFT (Scale-Invariant Feature Transform) or ORB (Oriented FAST and Rotated BRIEF).
    
2. **Pairwise Matching**: Find matches between key points in pairs of images. This involves comparing feature descriptors from one image to another to find correspondences.
    
3. **Initial Reconstruction**: Select a pair of images with a sufficient number of matches and use them to compute the relative pose (position and orientation) of the cameras and the 3D position of the matched key points. This initial pair of images forms the seed for the incremental reconstruction.
    
4. **Incremental Addition of Images**: For each subsequent image:    
    - Find matches with the already reconstructed points.
    - Estimate the pose of the camera for this image.
    - Triangulate to find new 3D points.
5. **Bundle Adjustment**: This is an optimization step. Given the current estimates of 3D structure and camera poses, bundle adjustment refines these estimates by **minimizing the reprojection error, which is the difference between the observed positions of key points in the images and the projected positions of the corresponding 3D points.** triggsBundleAdjustmentModern2000

Incremental **SfM is particularly useful in scenarios where processing power or memory is limited** since it doesn't require all images to be loaded and processed simultaneously. However, it can be more sensitive to errors than global methods, where all images are considered simultaneously. If a mistake is made early in the incremental process, it can propagate and affect the accuracy of the entire reconstruction.

## Descriptors
In computer vision and image processing, a descriptor refers to a compact representation of interesting information or characteristics about a certain part of an image, often centered around keypoints or regions of interest. **Descriptors are used to describe and differentiate regions or features in images so that they can be matched or compared across different images**.

Here's a breakdown of how descriptors work and why they are useful:

1. **Key Point Detection**: Before we get to descriptors, the first step is often to detect keypoints in an image. Keypoints are salient points that can be reliably and repeatably detected across different views of the same object or scene. For instance, corners, edges, and other unique features in an image often serve as keypoints.
    
2. **Descriptor Extraction**: Once keypoints are detected, a descriptor captures a patch around the keypoint and converts it into a compact, often fixed-size vector. The goal is for this descriptor to be:
    
    - **Distinctive**: It should be different for different features in the image.
    - **Invariant**: It should remain as consistent as possible despite transformations like rotation, scaling, or lighting changes.
3. **Common Descriptors**: Several types of descriptors have been proposed in the literature, each with its strengths and weaknesses:
    
    - **SIFT (Scale-Invariant Feature Transform)**: Produces descriptors that are invariant to scale and orientation changes.
    - **SURF (Speeded-Up Robust Features)**: Faster than SIFT and also invariant to scale and rotation.
    - **ORB (Oriented FAST and Rotated BRIEF)**: Combines the FAST keypoint detector and the BRIEF descriptor and adds rotation invariance. It's faster than both SIFT and SURF and is suitable for real-time applications.
    - **BRIEF (Binary Robust Independent Elementary Features)**: Generates a binary descriptor very quickly, but it's not rotation invariant (which ORB addresses).
4. **Applications**:    
    - **Image Matching**: By comparing descriptors from two images, one can determine which keypoints match, facilitating tasks like image stitching.
    - **Object Recognition**: Descriptors can help in recognizing specific objects in an image ==**if you have a database of descriptors for known objects**==.
    - **3D Reconstruction**: As in Structure from Motion (SfM), descriptors allow for point <span style="background-color:#007700">correspondences across multiple images, enabling 3D triangulation</span>.
5. **Comparison and Matching**: <mark style="background: #BBFABBA6;">Once descriptors are extracted from images, they can be compared using various distance metrics. For instance, the Euclidean distance can be used for SIFT or SURF descriptors, while the Hamming distance is suitable for binary descriptors like ORB and BRIEF</mark>. #Evaluation-metrics 
In essence, descriptors provide a way to represent and compare image features in a manner that's robust to various changes and transformations, making them invaluable in numerous computer vision tasks.

## Perspective-n-Point
#PnP
The Perspective-n-Point (PnP) problem is a fundamental question in computer vision, especially in tasks involving 3D reconstruction or object pose estimation. Here's a simple explanation:

Imagine you have a toy on your table, and you've taken a photo of it with your camera. Now, you know certain points on the toy (like the tip of its ears, nose, and tail) and their positions in the real world (in 3D space). From the photo, you can also see where these points appear on the image (in 2D space). The PnP problem is essentially asking: "Given these points, can you figure out where and at what angle you held the camera when you took the photo?"

In more technical terms, the PnP problem is about finding the position and orientation (pose) of a camera in 3D space, given a set of n 3D points in the world and their corresponding 2D projections (locations) in an image.

The solution to this problem is fundamental in many applications, like augmented reality (where you need to overlay digital images on the real world in a consistent manner) or robotics (where a robot might need to understand its position relative to an object it sees).
