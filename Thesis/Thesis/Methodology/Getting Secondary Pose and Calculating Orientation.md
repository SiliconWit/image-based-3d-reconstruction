#open3d #spatial-kinematics
The primary pose is that which is perpendicular to the Region of Interest(RoI) while the secondary pose is that which is offest from the primary by an arbitrary angle about the RoI's centroid on a plane containing the global centroid(P3), the RoI Centroid(P2) and the primary pose(P1). 
The secondary pose aims to increase the angle created by P1-P2 and P3-P2 or decrease the angle created by  P2-P1 and P3-P2. 

# Matrices vs Quaternion
Comparing the efficiency of using Open3D with matrices versus SciPy with quaternions for 3D rotations involves several factors, including computational complexity, readability and ease of implementation, and the specific use case.

1. **Computational Complexity**:    
    - **Open3D and Matrices**: The use of rotation matrices is straightforward in concept and is based on linear algebra operations. However, matrix multiplication (especially in 3D) can be computationally more intensive than quaternion operations. Open3D is optimized for 3D operations, but the underlying computation involves matrix algebra.
    - **SciPy and Quaternions**: Quaternion operations are generally more efficient for rotations in 3D space because they avoid the gimbal lock problem and require fewer computational resources compared to matrix operations. SciPy's implementation is optimized for performance.
2. **Readability and Implementation**:    
    - Open3D with matrices might be more intuitive for those familiar with traditional linear algebra and 3D transformations.
    - Quaternion-based approaches can be less intuitive but are more compact and often result in simpler code for complex rotations.
3. **Specific Use Case**:    
    - If you're working extensively with 3D data structures (like point clouds or meshes) and need additional 3D processing functionalities, Open3D offers a wide range of tools that might be beneficial beyond just rotation.
    - If your primary need is efficient rotation and you're working within the SciPy ecosystem for other numerical computations, using SciPy's quaternion might be more straightforward.
In terms of raw computational efficiency, especially for a large number of rotations or complex 3D transformations, the quaternion approach with SciPy is likely to be more efficient. However, the difference might not be significant for smaller-scale or less frequent computations. The choice also depends on the broader context of your project, such as the libraries you're already using and your familiarity with each method.

## Open3D

```python
import numpy as np
import open3d as o3d

# Original points
P1 = np.array([1.34136507, 0.68677158, 2.34911368])
P2 = np.array([0.62016513, 0.62849933, 2.69039799])
P3 = np.array([-0.07063117, 0.15295611, 2.05438438])

# Calculate vectors P1-P2 and P3-P2
v1 = P1 - P2
v2 = P3 - P2

# Compute the normal vector to the plane (perpendicular to the plane)
normal = np.cross(v1, v2)
axis_normalized = normal / np.linalg.norm(normal)

# Define the angle theta in degrees
theta = ...  # replace with the value of theta in degrees

# Convert theta to radians
theta_radians = np.radians(theta)

# Create a rotation matrix using Open3D or NumPy
R = o3d.geometry.get_rotation_matrix_from_axis_angle(theta_radians * axis_normalized)

# Rotate the vector P1-P2 using the rotation matrix
rotated_vector = np.dot(R, v1)

# Find the new position P1_new
P1_new = rotated_vector + P2

P1_new
```

```python
import numpy as np
from scipy.spatial.transform import Rotation as R

# Original points
P1 = np.array([1.34136507, 0.68677158, 2.34911368])
P2 = np.array([0.62016513, 0.62849933, 2.69039799])
P3 = np.array([-0.07063117, 0.15295611, 2.05438438])

# Calculate vectors P1-P2 and P3-P2
v1 = P1 - P2
v2 = P3 - P2

# Compute the normal vector to the plane (perpendicular to the plane)
normal = np.cross(v1, v2)

# Normalize the normal vector
axis_normalized = normal / np.linalg.norm(normal)

# Define the additional angle theta in degrees
theta = ...  # replace with the value of theta in degrees

# Convert theta to radians
theta_radians = np.radians(theta)

# Create a quaternion for the rotation
quat = R.from_rotvec(theta_radians * axis_normalized)

# Rotate the vector P1-P2
rotated_vector = quat.apply(v1)

# Find the new position P1_new
P1_new = rotated_vector + P2

P1_new

```

### Normalizing

Normalizing a vector in the context of using it as an orientation in 3D space can indeed be beneficial for several reasons:

1. **Uniform Length**: Normalization scales a vector to have a length (or magnitude) of 1. This is particularly useful when the vector's direction is important, but its magnitude is not. In 3D graphics and physics simulations, normalized vectors are often used to represent directions because they simplify calculations and provide consistency.
    
2. **Simplifies Calculations**: When working with unit vectors (vectors of length 1), many vector operations become simpler and more efficient. For instance, when calculating the dot product between unit vectors, the result directly gives the cosine of the angle between them.
    
3. **Avoids Scaling Issues**: If a vector used for orientation isn't normalized, its length might unintentionally scale other vectors or coordinates in calculations, leading to incorrect results. For example, in lighting calculations in computer graphics, using a non-normalized vector for light direction can unintentionally affect the intensity of the light.
    
4. **Necessary for Certain Operations**: Some operations require normalized vectors to work correctly. For instance, in quaternion rotation, the axis of rotation must be a normalized vector. Similarly, in many transformation matrices, the vectors representing direction must be normalized to maintain the correct scale.
    
5. **Representation of Pure Direction**: A normalized vector purely represents direction, devoid of any magnitude-related information. This is conceptually clearer and often necessary in operations where only direction matters, such as defining the orientation of an object in space.
    

Overall, normalizing a vector used as an orientation in 3D space ensures that it accurately represents a direction without introducing unintended scaling, making it a standard practice in fields like computer graphics, robotics, and physics simulations.