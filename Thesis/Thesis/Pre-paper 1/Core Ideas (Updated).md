# Concept
- I propose a convergent capture pattern with orbital Movement with a fixed radius coupled with vertical translation.
- This pattern will ensure overlap of images are maintained across the lateral and vertical axes of the target object, while allowing fewer capture poses.
### Background
- Applications of photogrammetry:
	- Metrology
	- Inspection
	- Reverse Engineering
	- Gaming
## Justifications
- Pitched cameras even at one(high up) position give better results - 10.1007/s00603-022-02789-9
- Resolution is not the end-all be-all, there is a point of diminishing returns.
- Evaluation by accuracy alone-RMSE and Entropy.
- Generic Objects other than what is covered in Literature
- Using Game Engines easily allows obtaining certain geometries of the object and consistently.
## Literature Review Counters and Contributions
### Optimal Lateral Displacement
- **Camera Pitch** &rarr; poses at different heights faced at the center of the object. *Our pitch varies across different heights*
- **Camera Network Design** &rarr; 2 networks configurations are proposed, with the latter being selected. 
	1. The first involved focusing the camera pose at the centre of the object for different elevations and rotating the object by 90 degrees to get full coverage.
	2. The second involved 4 sets of images at different heights tilting towards the targets' centroid to maintain a "higher overlap", there is no quantification for such an overlap
- Working distance was set to 20cm? *This is ill-posed, we propose heuristics for this*
- 
## Methodology
- Based on Unity Engine, HDRP, Ray-Tracing, Garage Scene.
- Make use of at least 3 target 3D models, Stanford Bunny, Utah Teapot, and a Real World Object-.
- Factors to Consider: Background<Plain, not Plain>, Frame Occupancy<object__radius>, Lateral Displacement, Camera Network Design. 
- *Experiment 1:
	- Compare No. of images at a single vertical position - or use what is proposed in optimal Lateral Displacement paper. Consider only 2 levels
### Experimental Setup:
- Identify 2 specimens: Stanford Bunny and [Engine Block](https://sketchfab.com/3d-models/2-stroke-engine-block-7daa64f3a607474b98f92dab44475de6) or [Transmission 3D Scan - Artec Spider](https://sketchfab.com/3d-models/transmission-3d-scan-artec-spider-9e12a9e6275e4380ab5d51d047d5ee18)
#### Experiment 1
- Based on Circular Convergent Camera Network Design
- Factors:
	- Lateral displacements: ${ \frac{3\pi}{60} }$ and ${ \frac{6\pi}{60} }$ per orbit
	- Longitudinal Displacements: $\text{norm\_radius}$ based on the objects bounding box cross-section and Camera's Field of View
	- Vertical Convergence Ratio: ${0}$% vs ${33}$%
#### Experiment 2
- Based on the Spiral Convergent Network Design
- The top treatments from Experiment 1 will be used.
# Results
- Performance Metrics: Error & uncertainty
- Use of Box Plots all through
- Comparison of the Best from Experiment 2 vs Experiment 1
- Qualitative Comparison between Ground Truth and Best Reconstruction Result.