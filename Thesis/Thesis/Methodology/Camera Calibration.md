From Image to Full Metric Reconstruction 2 things are needed:
	1. External Parameters of the camera
	2. Internal Parameters &rarr; how the camera maps points in the world onto the image plane via perspective projection.
Camera Calibration &rarr; determining extrinsic intrinsic parameters. A single image with known geometry is all that is needed to determine both. The result of this is a projection matrix.

Linear camera model ⇾ Project Matrix.
![[Pasted image 20231204113250.png]]
If we know the relative  position of the Camera Coordinate frame with respect to the World Coordinate frame, we can map point **P** to a point in the image plane.

![[Pasted image 20231204113636.png]]Perspective Projection is usually with respect to the camera coordinate frame. 
![[Pasted image 20231204114119.png]]
## Image Plane to Image Sensor Mapping
This is where pixel densities come into play with the densities varying over X and Y. We cannot assumed that the pixels are square shaped that is why there are a densities for both x and y separately.
The pixel densities are also not known.
![[Pasted image 20231204114443.png]]
But since it is not known where the principal point is, the equation for mapping mm to pixel units can be modified to:
![[Pasted image 20231204114606.png]]
$m_x$ and $f$ are unknown. They are the pixel density and focal length resp.
![[Pasted image 20231204114751.png]]
The expression above is however non-linear.
![[Pasted image 20231204115046.png]]![[Pasted image 20231204115220.png]]This is the intrinsic matrix.
![[Pasted image 20231204120203.png]]

# Camera Calibration Procedure
1. Find an object with known dimensions/geometry
2. Establish a coordinate frame based on the object.
3. Take image of the object, noting the corresponding points based on the established coordinate frame and pixels from the image.![[Screenshot from 2023-12-04 12-07-13 1.png]]
4. ![[Pasted image 20231204120828.png]]Scaling both camera and world yield not difference but may provide calculation convenience.
## Issues with Stereo Matching
1. Surface must have a (non-repetitive) texture
2. Foreshortening effect makes matching a challenge.
In feature matching, a small window gives high localisation but is susceptible to noise. A large window is blurred with poor localisation.
**Adaptive Window Method Solution** -> use multiple window sizes and use the disparity that is a result of the best similarity measure.

![[Pasted image 20231204124309.png]]