#objectives 
## Project Description

A study on the photo capture process and how to improve it for the sole purpose of photogrammetry-based 3D reconstruction.

The study will be based on Virtual Environments that will be simulated by the Unity Game Engine.

The primary objective is the deduction of the best extrinsic parameters of the camera for photo capture.

The main deliverable is a framework that allows for the autonomous deduction of camera poses, which will allow for the best fidelity of the reconstruction while utilising an optical number of images for a given object of interest

## Main Objective

To develop and evaluate a novel algorithm for the autonomous determination of optimal camera poses for effective 3D object reconstruction in close-range photogrammetry, leveraging Unity 3D for environment simulation and integration with robotic systems.

## Specific Objectives

1. **Objective 2:** "To create a wide variety of virtual environments using Unity 3D, considering different materials and lighting conditions that closely mimic real-world scenarios."
2. **Objective 3:** "To simulate diverse camera systems in Unity 3D, experimenting with various settings and configurations relevant to close-range photogrammetry."
3. **Objective 4:** "To design and implement an algorithm for autonomously determining the optimal camera poses for effective 3D reconstruction in the simulated environments."
4. **Objective 5:** "To integrate the developed pose estimation algorithm into a robotic path planning system for practical implementation."
5. **Objective 6:** "To assess the performance and reliability of the developed algorithm by comparing the 3D reconstructions from autonomously determined camera poses with those from manually determined poses."
6. **Objective 7:** "To investigate potential improvements, future research directions, and practical applications of the developed algorithm in the field of close-range photogrammetry."

### Revised Specific Objectives

1. To investigate the reconstructability of 3D models from virtual images captured from various Unity 3D scenes.
    
    Explanation: Perform 3D reconstruction of a single object in different simulated scenes to assess the ability of various reconstruction pipelines to utilize virtual images to reconstruct 3D objects.
    
2. To investigate the presence of any systematic biases on the virtual images and establish suitable 3D reconstruction pipeline(software) for processing the virtual images captured with Unity 3D.
    
    Explanation: Identify a physical object whose 3D model exists. Obtain images of both the physical and virtual object and reconstruct them to identify the presence of any s
    
    _Perform 3D reconstruction from both real and virtual images of a target object to reveal any systematic biases or errors associated with using virtual images for photogrammetry._
    
3. To design and implement an algorithm for autonomously determining the camera poses for effective 3D reconstruction in the simulated environments.
    
4. To integrate the developed pose estimation algorithm into a robotic path planning system for practical implementation.
    
5. To assess the performance and reliability of the developed algorithm by comparing the 3D reconstructions from autonomously determined camera poses with those from manually determined poses.
    
6. To investigate potential improvements, future research directions, and practical applications of the developed algorithm in the field of close-range photogrammetry.
    

### Revised Specific Objectives - 2

1. To investigate the reconstruction ability of various 3D reconstruction pipelines on virtual photos from Unity 3D, while establishing if there are any systematic biases or limitations of the tested pipelines on the virtual images.
    
    _**Explanation**: Identify a target physical object whose corresponding 3D(virtual) model exists. Perform 3D reconstructions of the target virtual object in at least 2 different simulated scenes as well as the physical target object in at least 2 different physical scenes. In other words, 4 sets of images(2 virtual and 2 physical) will be input into various reconstruction pipelines and the results compared. The results will be ranked and the expectation is that the performance of a pipeline “A” in the physical image reconstruction should correspond to its performance in virtual image reconstruction when compared to the other pipelines._
    
    - _For either case(virtual or physical), the 2 scenes will be outdoor and indoor._
    - _The position of the camera in both cases will be done manually through human effort. For the virtual case, a VR HMD and controllers would be utilised._
    
    _****************************************************The objective is not intended to be concise but rather to give an approximation.****************************************************_
    
2. To design and implement an algorithm for autonomously determining the camera poses for effective 3D reconstruction in the simulated environments.
    
3. To integrate the developed pose estimation algorithm into a robotic path planning system for practical implementation.
    
4. To assess the performance and reliability of the developed algorithm by comparing the 3D reconstructions from autonomously determined camera poses with those from manually determined poses.
    

CHATGPT → [https://chat.openai.com/share/7983e13d-ba97-4fb4-a999-ee0546eca97a](https://chat.openai.com/share/7983e13d-ba97-4fb4-a999-ee0546eca97a)