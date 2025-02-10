### The Initial Scan/First-pass
This initial scan is crucial as it provides a preliminary model of the object, which can then be refined through subsequent scanning processes. The first pass allows for the collection of essential geometric information about the object, which is necessary for planning the next best views (NBVs) for further scanning (Kriegel et al., 2013). This process involves creating a rough representation of the object's surface, which can be iteratively improved as more data is collected from different angles (Chen et al., 2016)[https://www.mdpi.com/2076-3417/6/5/132]. The initial scan serves as a foundational step that informs the scanning algorithm about the object's structure and occlusions, enabling it to make informed decisions about where to scan next (Prieto et al., 2019)[https://www.mdpi.com/1424-8220/19/21/4740].

Points to capture:
- How are Geometries of Interest identified.

> This challenge is known as the NBV planning problem as each view is chosen  subsequent to evaluating the information obtained from previous views and in  some cases a priori scene knowledge.
# Scene/Object Representation
- Depth maps as a form of surface representation. Depth maps are simplistic and comparisons can be made to voxel maps.
- Working with 2D representations is also easier as there are mature algorithms to work with 2D data.
- Depth map represent the spatial arrangement of surfaces in a scene by encoding the distance from a viewpoint to various points on those surfaces.
- Depth maps are capable of presenting object understanding in terms of its geometry of scene elements captured by a viewpoint \autocite{zhengPointCloudsScene2013,linDesignImplementationPlenoptic2023}
- The two-dimensional data structure of a depth map is simple and it is mature enough such that there are plenty of algorithms to works with it. Computation is faster even when it comes to perception of three dimensional features.
- This work leverages the pros of depth maps to determine Geometries of Interest onf that target objects of a captured viewpoint towards a Next Best View apporach for photogrammetric data capture. 
- First, a reference viewpoint with target object at the heart of the image is captured. The depth maps of the same is computed and a mask for the target object is computed. Out of the mask, a radius of pixels **occp_ratio** is determined that would form an area equivalent to the number of pixels that the target object occupies.
- 
# Viewpoint Selection
- Views can be represented based on blob detection. `get to the nitty gritty of blob detection adding elements relevant to geometries of interest.`
- The 2D arrays is 