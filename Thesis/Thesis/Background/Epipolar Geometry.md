#computervision #photogrammetry #multiview

The statement "From the sequence of images, epipolar geometry can be found, and 3D geometry can be reconstructed" describes a fundamental process in computer vision for reconstructing 3D objects or scenes using monocular cameras. Here’s an explanation of its meaning and significance:

## **Epipolar Geometry in Monocular Vision**

Epipolar geometry defines the geometric relationship between two views (or frames) of the same scene captured from different positions or orientations of a camera. It is independent of the actual 3D structure of the scene and depends only on the relative positions and orientations of the camera views and their intrinsic parameters (e.g., focal length). Key concepts include:
- **Epipolar Plane**: The plane defined by a 3D point, the camera centers for the two views, and their projections.
- **Epipolar Lines**: The intersection of the epipolar plane with each image plane. These lines constrain where a point in one image must lie in the other image.
- **Fundamental Matrix (F)**: Encodes the epipolar constraints between two views when intrinsic parameters are unknown.
- **Essential Matrix (E)**: Encodes these constraints when intrinsic parameters are known[1](https://web.stanford.edu/class/cs231a/course_notes/03-epipolar-geometry.pdf)[2](https://perso.ensta-paris.fr/~manzaner/Cours/ROB313/ROB313_3d_2.pdf)[3](https://www.robots.ox.ac.uk/~vgg/hzbook/hzbook2/HZepipolar.pdf).
![[Pasted image 20250105132718.png]]
In practice, epipolar geometry simplifies finding correspondences between points in two images. For instance, if a point pp in one image corresponds to a point p′p′ in another image, p′p′ must lie on the epipolar line associated with pp. This reduces the search space for matching points from a 2D region to a 1D line.

## **3D Reconstruction Using Monocular Cameras**

Monocular cameras capture only 2D projections of 3D scenes, so reconstructing 3D geometry requires multiple views (a sequence of images). The process involves:

1. **Feature Matching**: Identifying corresponding points across images using feature detectors (e.g., SIFT or ORB).
2. **Estimating Camera Motion**: Using epipolar constraints to compute relative camera poses (rotation RR and translation TT) between frames.
3. **Triangulation**: Reconstructing 3D points by intersecting projection rays from corresponding points in different views[
    
    1
    
    ](https://web.stanford.edu/class/cs231a/course_notes/03-epipolar-geometry.pdf)[
    
    6
    
    ](https://arxiv.org/html/2406.04301v1)[
    
    9
    
    ](https://courses.cs.duke.edu/compsci527/spring22/slides/s_10_reconstruction.pdf).

## **Challenges and Solutions**

- **Scale Ambiguity**: Monocular systems cannot determine absolute scale (e.g., how far an object is), but they can recover relative depth up to a scale factor[
    
    2
    
    ](https://perso.ensta-paris.fr/~manzaner/Cours/ROB313/ROB313_3d_2.pdf)[
    
    8
    
    ](http://stanford.edu/class/ee367/Winter2017/ewald_ee367_win17_report.pdf).
- **Sparse Correspondences**: In texture-less regions, matching features can be difficult. Techniques like dense optical flow or neural methods address this limitation[
    
    4
    
    ](https://aim-uofa.github.io/FrozenRecon/)[
    
    6
    
    ](https://arxiv.org/html/2406.04301v1).
- **Camera Calibration**: Accurate intrinsic parameters are crucial for precise reconstruction. When unavailable, methods like structure-from-motion estimate these parameters alongside 3D structure[
    
    8
    
    ](http://stanford.edu/class/ee367/Winter2017/ewald_ee367_win17_report.pdf).

## **Applications**

This approach is widely used in:

- Robotics for navigation and mapping (e.g., SLAM).
- Augmented reality to overlay virtual objects on real scenes.
- Autonomous vehicles for understanding environments.

In summary, epipolar geometry provides constraints that enable efficient correspondence matching across images captured by a monocular camera. By leveraging these constraints, it becomes possible to reconstruct 3D geometry from 2D image sequences, despite challenges like scale ambiguity and sparse data.