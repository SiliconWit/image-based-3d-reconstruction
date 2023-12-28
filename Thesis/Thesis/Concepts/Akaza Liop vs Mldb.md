  #feature-extractor #meshroom 
Both AKAZE (Accelerated-KAZE) and LIOP (Local Intensity Order Pattern) are descriptors used in computer vision for feature extraction and matching in images. AKAZE itself is a feature detection and description method, and it can be combined with different descriptors for different purposes.

AKAZE_MLDB stands for "Modified Local Difference Binary," which is a binary descriptor employed with the AKAZE feature detector. It focuses on creating binary descriptors that are more robust to various transformations such as rotation, scale, and viewpoint changes.

On the other hand, AKAZE_LIOP stands for "Local Intensity Order Pattern," combining the AKAZE detector with the LIOP descriptor. LIOP uses the local intensity order to encode information about the local structure around keypoints.

In terms of performance, the choice between AKAZE_MLDB and AKAZE_LIOP often depends on the specific application, the nature of the images being processed, and the computational resources available. AKAZE_MLDB tends to offer robustness to various transformations, while AKAZE_LIOP might be more effective in scenarios where encoding local intensity order patterns is beneficial.

Experimentation and testing on specific datasets or applications would be the best way to determine which one suits your needs better, as their performance can vary based on the characteristics of the images and the tasks at hand.