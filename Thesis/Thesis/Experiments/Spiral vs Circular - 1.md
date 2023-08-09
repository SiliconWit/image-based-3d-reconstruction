#meshroom #experiment
## Description
The target object was the UtahTeapotSkewedTex.
Bounding Box is 0.41, 0.32 & 0.65.
The target scene is the Garage

## Parameters Capture
	- Photos per 2Pi(C) => 16
	- Photos $ => 48 photos
	- Mult_r => 1.5
	- Mult_h => 1.2
	- Elevations => 3(circular)
## Parameters in Meshroom 2023
For SFM, all nodes were left at their default parameters except for the `Camera Init` node where the Distortion Initialization Mode was changed to "estimated".


| Output Param | Circular | Spiral |
|------------| ------------| ------------ |
| Feature Extraction Time(s) | 90.87 | 97.64 |
| Total to SFM Time (s) | 252.23 | 280.08 |
| Feature Matching Time (s)| 4.38 | 4.55 |
| SFM Time (s) | 29.76 | 50.62 |
| Poses | 48 | 48 |
| Landmarks | 40635 | 48857 |
| Texture Files | 2 | 1 |
| Triangles | 704780 | 715221 |

## Discussion
Despite the Spiral pattern having more landmarks, the camera poses were not correct. At pose .42 the spiral camera pose changes drastically and the effect of that can be seen in the reconstruction as an inset on the surface. It appears that pose .48 should have followed .41 then .47 and finally finishing up with .32
![[Pasted image 20230726231607.png]]
	
The Circular reconstruction also has some of these insets. However, it has more complete surfaces than the spiral 

**The Spiral Pattern has to be fixed first**
It turns out that calculation of the angle theta in the `SpiralWaypointGenerator` script was the cause of the problem.

