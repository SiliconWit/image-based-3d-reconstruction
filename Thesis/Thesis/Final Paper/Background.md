- NBV planning involves selecting the next viewpoint that maximizes information gain while minimizing redundant data collection. The goal is typically to achieve efficient and complete three-dimensional reconstruction of objects or environments.
- This is often achieved through the analysis of the current state of the 3D model and the identification of unobserved regions, the observed and unobserved volumes\autocite{houVolumetricNextBest2019}
- Some of the Key components of NBV:
	1. Information gain metrics
	2. Representation of the environment
	3. Computation of the NBV
- \autocite{kriegelEfficientNextbestscanPlanning2015} provides justification for why autonomous scanning methods are needed.
- Our focus is on NBV planning for object modelling \autocite{kriegelEfficientNextbestscanPlanning2015}
- Dependence on a users skill can lead to inconsistencies \autocite{leeAutonomousViewPlanning2024}
- Object scanning goes beyond simply detecting the occupancy of an object in 3D space.
- Our Scope for object size is Small Object and Furniture size as per the definition of \autocite{leeAutonomousViewPlanning2024}
- Monocular sensors is a Traignulation type of sensor.
- volumetric approach does have its limitations. Specifically, a complex and detailed shape of a curved surface can only be expressed within the resolution of the voxel grid.


# Overview
- My focus is on object modelling through 3D reconstruction based on a single monocular camera.
- NBV approaches rely on an underlying representation of the target object to compute the optimal sensor view points.
- Volumetric representations and their associated techniques for computing new views can be computationally expensive and fail to identify intricate geometrical characteristics of the target objects thus achieving inaccurate 3D reconstruction of the target object
- This work proposes a Next Best View approach for data acquisition for photogrammetry based on depth representation derived from a deep depth estimator.
- Candidate viewpoints are instantiated based on the geometrical features of the object of reconstruction which are derived from the depth map(frontiers?)
- The method was tested in a realistic virtual environment developed in Unity Engine-HDRP using virtual 3D Models as ground truth,
- Evaluation was based on the Hausdorff distance of sections of the target object and Euclidean distances of the full reconstruction.
- By leveraging pixel depth information, this method provides an alternative to volumetric based approaches that yield more accurate reconstructions and computationally efficient.
