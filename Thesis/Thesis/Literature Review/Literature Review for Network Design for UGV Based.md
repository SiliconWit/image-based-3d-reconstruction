The contributions of Paper2, titled "An Imaging Network Design for UGV-Based 3D Reconstruction of Buildings" by Hosseininaveh and Remondino, are as follows:

1. **Development of a View Planning Method**: The paper introduces a method for planning camera views for 3D reconstruction of buildings using terrestrial images. This method is designed for use with an Unmanned Ground Vehicle (UGV) equipped with a digital camera.    
2. **Camera Pose Suggestions for Robots and Human Operators**: The method proposes a way to suggest camera poses that are attainable by either a robot, in the form of numerical pose values, or a human operator, using vectors on a metric map.    
3. **Placement of Initial Viewpoints on a Two-Dimensional Map**: Unlike traditional imaging network design methods that place initial viewpoints on an ellipse or sphere, this method places them within maximum and minimum optimal ranges on a two-dimensional map.    
4. **Range-Related Constraints and Clustering Approach**: The method incorporates range-related constraints in defining the suitable range from the building. It uses a visibility matrix based on a four-zone cone for clustering and selecting viewpoints, which is a different approach from traditional methods that use three-ray coverage filtering.    
5. **Simulated Environment for Method Evaluation**: The paper provides a simulated environment, including textured buildings in ROS Gazebo, along with a ROS-based simulated UGV equipped with a 2D LiDAR, DSLR camera, and an IMU. This environment is available for other researchers to use for method evaluation.    

The paper presents the new imaging network design method, its implementation, and results from both simulation and real experiments for façade 3D modeling. It concludes with a discussion, final considerations, and suggestions for future work​​.

