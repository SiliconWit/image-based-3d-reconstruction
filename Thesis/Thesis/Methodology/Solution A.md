## Overview
The problem at hand is determination of optimal camera poses for photogrammetry. With such camera poses, the number of images can be minimized while still providing an accurate reconstruction. The camera poses are to be determined autonomously and be transformed spatially to a robotic manipulator's pose.

## Methodology
- Setup a Scene and Camera system in Unity.
- Create a program(in Python) that hosts the algorithm - Pose Determiner(PD).
- Select a suitable robot to work with from Webot's asset library.
- Establish a mesh-like channel of communication between the Scene View in Unity, Webots and the Pose Determiner.

## Pose Determiner
- The pose determiner(PD) does much of the work.
- Components of the pose determiner include a unity-bridge and webots-bridge.
- The PD triggers a model-free scan of the target object in the scene. 
- The model free scan entails a quick scan round the object in question.
- The Pose Determiner gets frames of the scene from the model-free scan.
- The frames are passed through a metric depth estimation pipeline which produces depth maps of the image.
- The depth maps are then used to create voxels based on the depth values. The voxel's normals should align with those of the surface as presented on the depth map.
- After the model-free scan, the voxels' spatial distribution will serve as the basis for determining the optimal camera poses.

## Communication Network
The scene view is received by PD.
Webots controls the camera transformation in Unity.
PD determines the kinematic poses of the robot in WeBots
The communication network can be simplified by implementing

## Work Packages
### Setup Communication Channels
Timeline: 2 Weeks
- There are three parts to the whole system: 
	- Pose Determiner
	- Scene and Object (Virtual Scene in Unity)
	- The robotic system(Webots)
- Send images from the Unity Camera to the Pose Determiner.
- Command the Robot in Webots from the Pose Determiner.
- Apply transformations of the robot's end effector to the camera in Unity
### Model Free Scan
Timeline: 3 weeks
This is a scan of an object where its geometry is unknown.
- Transform the camera about the object in one sweep.
- Obtain depth data from images from the sweep.
- Compute the depth data into a 3D model based on primitives shapes.
### Model Based Scan
Timeline: 3 weeks
This is a scan whereby information about the target object already exists.
- Come up with an algorithm to get camera poses from the model free scan.
- Test the determined poses by using them in a reconstruction process.
### Robot Integration
Timeline: 2 weeks
- To translate new camera poses to equivalent poses for selected robot.
- Validating the robot-camera poses by using them in a reconstruction process.

