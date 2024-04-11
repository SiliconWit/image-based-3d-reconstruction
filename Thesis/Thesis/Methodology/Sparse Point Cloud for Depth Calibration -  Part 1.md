Correct absolute depth values are imperative for creating new camera poses. This is made possible through Open3D which has the ability to generate point clouds from the RGB and depth images. From the point cloud, location and orientation of surface normals can be easily obtained and this serve as the basis for new poses. However, the depth values received from the depth maps are not reliable and thus poses generated are consistently out of place. 

One solution towards this would be to register the point cloud from  Open3D's RGB-D object with a reference. The reference can be a sparse point cloud that is generated through an SfM process with very few viewpoints.

## Global vs Local Registration
>During the registration between the point clouds, common algorithms such as Iterative Closest Point (ICP), do not converge to the global error minimum and instead converge to a local minimum, hence ICP registration is referred to as a local registration. It relies on an approximate alignment as an initial step.

> Another category of registration methods is global registration. These algorithms do not depend on alignment for initialization. Typically, they produce less precise alignment outcomes but make aid in converging to the global minimum and hence are employed as the initial step for local methods.
# Experimental Setup
**Goal:** To find the optimal number of images, viewpoints and configuration for obtaining the best point clouds.
 The factors are:
 1. **Viewpoints:**   
	 1. Left, Right, Center
	 2. Left, Top, Right
	 3. Left, Right
	 4. Left, Top, Right, Center
 2. Feature Extractor:
	 1. SIFT
	 2. DSP-SIFT
 3. Configuration:
	 1. Full Metadata
	 2. Raw Metadata - Full without make and model
	 3. No Metadata

## Data Collection
The main requirements for data collection are
	1. Four images of the target at different viewpoints. NB: The camera's transform is defined about the origin of the scene, as well as its Center of Mass.
			- Center: 20&deg; about the origin's Y axis and 0.5 units above origin's XZ plane
		- Left: 20&deg; about the origin's Y axis and 0.5 units above origin's XZ plane
		- Right: 20&deg; about the origin's Y axis and 0.5 units above origin's XZ plane
		- Top: 20&deg; pitch and 1.2 units above origin's XZ plane
	2. Images with Full Metadata, Images with Full metadata but without Make and Model, images without Metadata
This makes a total of 12 images in total.

## Experimental Design
1. **Compare sets of 3** 
		- Center vs Top
		- All cases for Metadata
		- 6 experiments in total
		