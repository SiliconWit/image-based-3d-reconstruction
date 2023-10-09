**Rationale/Significance of the Study:**

Within the nuanced landscape of photogrammetry, challenges arising from manual intricacies often dilute the fidelity of 3D reconstructions. The drive to automate camera pose determination addresses these challenges, instilling consistency and precision into captures, refining depth, scale, and intricate detailing. When harmoniously integrated with robotic path planning, this endeavor extends beyond mere accuracy, enabling access to previously inaccessible or perilous viewpoints. This confluence of automation and robotics has broader implications, poised to influence applications ranging from heritage preservation, urban planning, to intricate manufacturing processes. Industries that rely heavily on accurate 3D modelling stand to gain substantially in terms of efficiency and output quality. Concurrently, the academic realm, especially disciplines intersecting photogrammetry, robotics, and computer vision, may witness shifts in methodologies and best practices. In essence, this research, by amalgamating automated pose estimation with robotic trajectory determination, paves the way for a transformative paradigm in close-range photogrammetry, with repercussions echoing across varied applications and practices.

## Scope Draft
The principle focus of this research work will be on the capture process that precedes the introduction of photos to a photogrammetry.
An open and complete reconstruction pipeline will be considered for the purpose of reconstruction to easily modify parameters and allow for seamless integration to custom-built software tools.
A realistic virtual environment designed in Unity Engine will be provided to simulate the real world. This will make use of Physically accurate lighting physics-ray tracing afforded by the Engine.
Both model-free and mode-based techniques for path planning will be considered as the model-free one will inform the model based one.
An open robotic simulation platform will be chosen also to allow for seamless integration. Also within the platform, a popular robot with already defined kinematics will be selected since my own kinematic configurations may be erroneuous. The dynamics of the robot will not be considered.
At the end of it all, there is Unity to simulate the virtual environement, a robot simulation plaform to simulate the robot. What I am developing(the novel algorithm) will recieve preliminary image frames from Unity, calculate optimal poses, then provided those to the robot simulation platform which will in turn control the camera transforms in Unity during the actual capture of photos to be reconstructed.
The reconstructions will be evaluated using Open software tools like CloudCompare or Open3D. These evaluations will be done on an iterative basis.
We will know we have achieved if our method offers better reconstruction results.

No real hardware will be used except a PC for simulation. 
The research will not go into optimising the reconstruction pipeline to improve reconstruction.

The target objects to be considered for reconstruction will not have a specular surface and will not absorbent of light hitting it.

### Refined Scope
Central to this investigation is the capture process in close-range monocular photogrammetry, preceding the assimilation of images into the reconstruction system. The research harnesses an open reconstruction pipeline, tailored for adaptability with custom software tools. For realism, a Unity Engine-based virtual environment is employed, capitalising on its advanced ray tracing for accurate lighting physics. Objects targeted for reconstruction are specifically non-specular and non-absorbent to light, ensuring only subjects favourable for photogrammetry are considered. Path planning will explore both model-free and model-based techniques, with the former informing the latter. An integration-friendly robotic simulation platform is adopted, utilising a robot with pre-established kinematic definitions, while consciously excluding its dynamics.

The operational sequence is as follows: Unity renders preliminary frames, processed by the algorithm to determine optimal poses. These are then relayed to the robot simulation platform, directing camera transformations in Unity during the capture. Reconstructions, subsequently, undergo iterative evaluations via open-source tools, like CloudCompare or Open3D. The scope's boundaries are clear: success is benchmarked by enhanced reconstruction outcomes, hardware is restricted to PC simulations, and direct enhancements of the reconstruction pipeline are not the research's focal point.

## Assumptions
1. The reconstruction pipeline will not have any biases towards the virtual photos from Unity compared to that of real photos.
2. The kinematic structure of the simulated robot will be accurately replicate that of an existing physical robot. 
