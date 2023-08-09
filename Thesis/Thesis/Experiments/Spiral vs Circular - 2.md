#experiment #colmap 

### Introduction
The second experiment was an attempt to fix some issues with the first while also adding standardisation. The issue fixed was that of incorrect spiral waypoints, while the standardisation done was to eliminate distortion of the camera. The latter was made possible by disabling the volume with the `Lens Distortion Override` and setting the `Distortion Initialisation Mode` to None. This was done as there is not reference to the magnitude of Lens distortion and it was better to assume that calibration had been performed on the images before getting them into Meshroom.

### Methodology
48 images with metdata were created for each pattern.
The images were then imported into both Meshroom and Colmap for reconstruction.
For **Meshroom** the default settings were maintained except for the `DepthMapFilter` whose **Min Consistent Cameras** was set to **5**.
For **Colmap** the Automatic Reconstruction workflow was selected and only the only field altered was the **Shared Intrinsics** which was checked.

### Results and Discussion.

The [Spiral vs Circular - 1]("Experiments/Sprial vs Circular - 1") revealed the following Flawed Spiral Way Points - Fixed and the inconsistency of SFM results(landmarks and camera poses). 
Those first was easy to solve as it involved rectifying the code which was a success.
The Second begged the question
	`Considering that the main results of an SFM are the camera poses and the landmarks obtrained, what outputs should I consider valid for SFM results?`
In attempt to answer the second question, the use of a different reconstruction pipeline was considered - COLMAP.