#publication
The first paper is a concoction of the first and second milestones of WorkPlan 2.
These are:
	1. Obtaining Geometrical Data and Raw Scan
	2. Calculate Camera Poses from the Raw Scan
In a nutshell the on successful execution of the 2 milestones, the result is a Next Best View algorithm that outputs camera poses that would get the most information for a complete 3D reconstruction.
**Contribution** ⇾ A heuristic method for determining optimal camera poses for 3D reconstruction.
The steps for this are:
	1. Have a 3D object within a scene ready.
	2. Get a bounding box data of the object in the scene.
	3. Take an image of the Object.
	4. Feature extraction from the image.
	5. Compute depth values of the object.
	7. Extract geometric features of interest(from the depth map): edges and blobs
	8. Create 3D coordinate system and pose points
	9. Repeat  from Step 3.

## Things worth capturing in the paper.
### Preliminary Experiment data
Preliminary experiments - The preliminary experiments investigated various factors in the capture process that affect the 3D reconstruction result. The conclusions from the preliminary experiments have allowed for the following:
	1. **Justification of having bounding box data**: 
		1. From the data it was established that a tilt of the camera towards the centroid yields better images for 3D reconstruction. The centroid can be determined through the bounding box dimensions
		2. The images guiding the model free scan need to have reference of the object position. The bounding box is best suited to do this.
	2. **Feature extraction of the original image**: There have been 3 sets of experiments. The first and second one which had the 3D object named RandomShape and the textured UtahTeapot seemed to suggest that a greater working distance yields better results. However, in the 3rd set of experiments, suggests that a closer working distance is more often better than a larger one. The disparity in those conclusions may be arising from the object used in those respective cases. It could be said that the Modedcube has sufficient features for the reconstruction pipeline to work with and that is why it could do better at shorter working distances whereas for objects with limited or confusing features, a larger working distance yields better results as the surrounding scene helps.
### Extraction of Geometrical Features from Depth maps
This research focuses on the capturing phase of the photogrammetric 3D reconstruction process. It further focuses on the geometrical and not-optical aspects of the target object. The Optical aspects of the object may include, reflections, textures and colours. In this regard, the research will assume to a great extent that the object has optical properties that favour 3D reconstruction using photogrammetry. To that end a single monocular camera is used as a sensor and thus depth maps will have to be computed from an RGB image taken by the monocular camera. Since the main focus is the geometrical feature of the object, a depth map should suffice for determining such values from the pixel values of the depth map. Feature in question will include:
	1. Surface Normals
	2. Occlusion Edges
	3. Blobs
### Creating a 3D coordinate system and Camera Poses
The poses are a function of the geometrical features of the target object. This means that when a significant feature is detected at a given point in the depth map, corresponding camera poses "looking at" that point have to be established. This camera poses have to placed on a 3D coordinate system. 
### Evaluation
The poses generated will be evaluated against generic poses-circular and spiral. 
Multiple target objects will be considered for the evaluation.
The metric to consider will be the RMSE agains tha ground truth models.

# Journals to Consider
1. [**Journal of Image and Vision Computing**](https://www.sciencedirect.com/journal/image-and-vision-computing) - 4.7 impact factor, 6 days to first decision, 97 days review time, Science Direct.
2. [Journal of Intelligent & Robotic Systems](https://www.springer.com/journal/10846/) - 3.3 impact factor, 6 days to first decision,
3. [IEEE Robotics and Automation Letters](https://ieeexplore.ieee.org/xpl/RecentIssue.jsp?punumber=7083369) - 5.2 impact factor