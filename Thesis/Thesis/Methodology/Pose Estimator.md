#feature-extractor
# Overview
The pose estimator is a critical component in the project, perhaps the most critical. The pose estimator is supposed to, s the name suggest, determine camera poses that comprehensively capture images of the target object for photogrammetric reconstruction.

The original image is first loaded in and its equivalent depth map computed. With the depth map different kinds of operations are carried out against it in order to obtain the most information about the image. The most important information to be deduced include general surface orientation and blobs from the depth map only.

