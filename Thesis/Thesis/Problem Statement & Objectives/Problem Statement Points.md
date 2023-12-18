## Things to Consider Highlighting
1. Automation of the camera
2. The significance of camera poses in the capture phase of the photogrammetric process.
3. What are the things that inform the camera poses in the capture phase of close-range photogrammetry,

## Automation of the Process
* Automation of the close-range photogrammetry process requires automation of image acquisition, orientation and correlations. [[samaanCLOSERANGEPHOTOGRAMMETRICTOOLS2013]]
* Automation should consider, occlusion, distortion and the shape of the object.[[samaanCLOSERANGEPHOTOGRAMMETRICTOOLS2013]]
## Precision Digital Measurement Tools
- Therefore, how to develop a precise digital measurement system has become a major research issue in the field of industrial manufacturing.[[wangCloserangeIndustrialPhotogrammetry2020]]

### Random Points
High resolution images are not always appropriate depending on the application.

## Rotating Objects
- The target objects is placed on rotary platform which in some cases is a turntable.
- A backdrop with a plain color is provided.
- Lighting may be uniform or may rotate with the objects
- The camera is stationary.
- This method may not account for occlusions well enough as well as intricate shapes of the target object.


## Rationale

To improve the automation of 3-D scanning, it is necessary to plan the path for the robot, making it maximize the information gained about the objects or scenes.[[fanPathPlanningAutonomous2023]]

The task of collecting extensive sensor information about an environment efficiently requires to plan paths for the robot that maximize the information gain about the unknown environment. This problem is also known as informative path planning (IPP).[[naazareOnlineNextBestViewPlanner2022]]

- SfM relies heavily on the viewpoints provided. Lack of data creates holes in the 3D reconstruction. More images may remedy this but may also be computationally expensive.Heinly, J., Schonberger, J.L., Dunn, E., Frahm, J.M.: Reconstructing the world* in six days*(as captured by the yahoo 100 million image dataset). In: Proceedings of the IEEE Conference on Computer Vision and Pattern Recognition. pp. 3287–3295 (2015)

# Solutions to the Problem 
>it is necessary to solve the problems of the optimal choice of the place of photo or video shooting, the direction and distance between the objects under study and the mobile video system, as well as the number of photos and videos frames... The result obtained largely depends on their quality. The determining factors are not only the parameters of the cameras used, but also the conditions and methods of shooting, geometric parameters of shooting [[mezheninUsingVirtualScenes2021]]

## Why Path planning
1. However, the completeness and precision of complex environment or targets highly rely on the flight planning due to the self-occlusion of structures.[[zhangTHREEDIMENSIONALPATHPLANNING2020]]
2. It is important to note that this initial reconstruction is highly inaccurate and incomplete since the viewpoints stem from <mark style="background: #FFF3A3A6;">a simple, regular pattern</mark> flown at relatively high altitude to avoid collisions.[[heppPlan3DViewpointTrajectory2018]]
3. Furthermore, it has been shown that, at some point, adding views yields diminishing returns (Hornung et al. 2008; Seitz et al. 2006;Waechteretal.2014). Exctracted from [[heppPlan3DViewpointTrajectory2018]]

## Monocular RGB and Close-Range
1. Active 3D modeling systems put data acquisition in the optimization loop, using partial results to guide further data acquisition. This problem has been studied with depth sensors [14], [15], [16] on small scale objects. Generalizing this idea to color cameras and to outdoor architectures is much harder.huangActiveImagebasedModeling2018