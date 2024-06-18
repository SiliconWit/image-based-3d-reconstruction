### AUTOMATING PHOTOGRAMMETRY FOR THE 3D DIGITISATION OF SMALL ARTEFACT COLLECTIONS
- Main focus was on a rig
- Points & Triangle counts were used as a metric, which was not in order.
- Predefined distribution of poses
### Simulation of Photogrammetry-Based 3D Data Acquisition
- Number of Photos and distance from target and orientation is not definitive.
- Target is a human body
- Solution to the lack of definitiveness is to create a simulator that can provided an optimized workflow.
- 
### Analysis of the Influence of the Geometrical Parameters of the Body Scanner on the Accuracy of Reconstruction of the Human Figure Using the Photogrammetry Technique
- Subject of scanning is a human body
- Experimental Research
	- The vertical distribution of camera viewpoints (on the z axis); 
	- The pitch angles of cameras;
	- The number of camera positions around the scanned object n;
	- The number of camera positions along the mast (vertically) k.
- A high resolution has less significance to accuracy.
- A focus on geometrically complex regions yielded better results.
![[Pasted image 20240417215731.png]]

### Optimal Lateral Displacement in Automatic Close-Range Photogrammetry
- Measurement uncertainty is in terms of micrometers.
>"The final step was to estimate the standard deviation of error of each cloud versus the corresponding reference model"
- Lateral displacement is the distance between 2 adjacent images.
- Image Capturing Protocol - everything to do with capturing images e.g. camera settings, lighting , patterns etc.
- The authors used Metashape Pro.
- Evaluations was done using Cloud Compare.
>The residual deviation of the 3D coordinates gathered with photogrammetry from the reference mesh was then statistically analyzed to calculate RMS error, mean error, and error histogram. We made sure that the scaling/orientation step was iterated until the mean value was less than 10 μm, for each dense cloud.
- Investigated only one parameter.