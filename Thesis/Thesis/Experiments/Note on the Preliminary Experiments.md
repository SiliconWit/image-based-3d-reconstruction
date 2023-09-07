- Preliminary experiments are a run-through of the whole photogrammetric reconstruction process from capture to reconstruction.
- Being that the project is all about the capture process, the preliminary experiments' main focus is on parameters that affect the capture process that precedes the computation for reconstruction.
-  Generally, the preliminary experiments involves the following
	1. Set up a Virtual Environment within the Unity Engine.
	2. Deploy a camera system that mimics a real world camera of sensor and lens properties.
	3. Adding a target object(3D model) of interest to the scene. 
	4. Setting parameters for a capture process. The parameters include, radius, camera vertical displacement, and a capture pattern.
	5. Photos are then taken with respect to parameters at step 3 and metadata added as per the lens and camera sensor specifications.
	6. The images are then reconstructed using Meshroom.
	7. The Reconstruction is compared to the 3D model in Step 3 using Cloud Compare.
	8. The process is then repeated from Step 4 - 7 with different parameters.

The likely noise(unwanted factors) are the 3D model itsetlf and the scene. To remedy the possible effects of the noise, an ideal scene and model had to be sought after.
An ideal scene incorporates the following [[reconstruction.bucchiRecommendationsImprovingPhoto2020a]]
	1. Even and Diffuse Lighting
	2. A contrasting background 
An ideal model should have fairly simple shapes with the following characteristics:
	1. Rough Material to prevent specular surfaces
	2. Textures that are not narrowly tiled
	3. 
Initially the UtahTeapot was considered then then the


Image overlap is the main factor that should be considered in place of 