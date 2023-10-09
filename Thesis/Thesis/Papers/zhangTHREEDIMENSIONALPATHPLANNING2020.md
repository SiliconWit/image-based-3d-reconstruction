---
Year: 2020
tags: Source 
Authors: S. Zhang, C. Liu, N. Haala
---

Title:: THREE-DIMENSIONAL PATH PLANNING OF UAVS IMAGING FOR COMPLETE PHOTOGRAMMETRIC RECONSTRUCTION
URL: https://isprs-annals.copernicus.org/articles/V-1-2020/325/2020/
Zotero Link: [Zhang et al. - 2020 - THREE-DIMENSIONAL PATH PLANNING OF UAVS IMAGING FO.pdf](zotero://select/library/items/QE4NXMLH)


However, the completeness and precision of complex environment or targets highly rely on the flight planning due to the self-occlusion of structures.” Yellow Highlight [Page 1](zotero://open-pdf/library/items/QE4NXMLH?page=1&annotation=X8JH64QY) 
 
initial flight as prior knowledge and estimates its completeness and precision” Yellow Highlight [Page 1](zotero://open-pdf/library/items/QE4NXMLH?page=1&annotation=J9CLMJHA) 
 
However, complex structures are often concave and have the nature of self-occlusion” Yellow Highlight [Page 1](zotero://open-pdf/library/items/QE4NXMLH?page=1&annotation=IFWQA2IY) 
 
complex structures are often concave and have the nature of self-occlusion.” Yellow Highlight [Page 1](zotero://open-pdf/library/items/QE4NXMLH?page=1&annotation=TEHBKRNL) 
 
 
 
A nadir image is <b>a satellite image or aerial photo of the Earth taken vertically</b>. 
 
nadir images” Orange Highlight [Page 1](zotero://open-pdf/library/items/QE4NXMLH?page=1&annotation=TM7MJNI3) 
 
planning viewpoints in three-dimensional space would be the best solution for lightweight UAVs” Yellow Highlight [Page 1](zotero://open-pdf/library/items/QE4NXMLH?page=1&annotation=4NRSXCZC) 
 
 
 
Model-based path planning uses pre-existing knowledge of the environment to determine a path, while model-free planning learns to navigate through direct interactions, typically without prior knowledge. 
 
s mapping complex structures” Yellow Highlight [Page 1](zotero://open-pdf/library/items/QE4NXMLH?page=1&annotation=LZAMXVVQ) 
 
UAV mapping has mainly two categories: model-free and model-based. The main difference between these two categories is whether prior knowledge, i.e. a coarse model, is required.” Yellow Highlight [Page 1](zotero://open-pdf/library/items/QE4NXMLH?page=1&annotation=8DHIX4HW) 
 
next-best-view” Green Highlight [Page 1](zotero://open-pdf/library/items/QE4NXMLH?page=1&annotation=RT3TPXJX) 
 
For example the angle between the viewpoint look-at direction and the normal of the surface (Hoppe et al., 2012), parallax angle and distance (Hepp et al., 2018, Smith et al., 2018), or other quality indices (Roberts et al., 2017, Peng, Isler, 2019).” Green Highlight [Page 1](zotero://open-pdf/library/items/QE4NXMLH?page=1&annotation=MXK3VUS4) 
 
Even though the completeness of the refined model is increased by considering the overlap between images, the precision is not guaranteed.” Yellow Highlight [Page 1](zotero://open-pdf/library/items/QE4NXMLH?page=1&annotation=8BKUEIAQ) 
 
Photogrammetric quality indices are introduced to guide the planning” Yellow Highlight [Page 1](zotero://open-pdf/library/items/QE4NXMLH?page=1&annotation=Z3EQLWM2) 
 
mutual information obtained by all viewpoints never decreases and the objective function related to information is submodu-” Green Highlight [Page 1](zotero://open-pdf/library/items/QE4NXMLH?page=1&annotation=CKWZBDH4) 
 
lar (Krause, Golovin, 2014). This allows adding viewpoints iteratively while considering global optimization, which significantly reduces the amount of computation. Therefore, the problem can be formulated as an orienteering problem or be solved using mixed-integer programming, downhill simplex, or genetic algorithm (Smith et al., 2018, Martin et al., 2016).” Green Highlight [Page 2](zotero://open-pdf/library/items/QE4NXMLH?page=2&annotation=DRZDDBAB) 
 
photogrammetric constraint” Yellow Highlight [Page 2](zotero://open-pdf/library/items/QE4NXMLH?page=2&annotation=FGM6CHBY) 
 
 
 
The proposed solution requires 3 inputs, a DIM point cloud, triangulated Mesh and the Camera Network. I get the impression that there was lot already done before and so the paper seeks to "perfect" a previous reconstruction. 
 
he input of the method is the dense image matching (DIM) point cloud, reconstructed triangulated mesh and corresponding camera network from a prior flight. We firstly analyze the completeness of the point cloud” Yellow Highlight [Page 2](zotero://open-pdf/library/items/QE4NXMLH?page=2&annotation=984ZXQVS) 
 
The incomplete part is marked out by point cloud index calculation and filtering” Blue Highlight [Page 2](zotero://open-pdf/library/items/QE4NXMLH?page=2&annotation=8276L2U2) 
 
first phase is adding viewpoints to make the point cloud complete, which results in a ‘strengthened camera network’” Yellow Highlight [Page 2](zotero://open-pdf/library/items/QE4NXMLH?page=2&annotation=XNHM66JA) 
 
econd phase is to increase the photogrammetric precision and an ‘optimized camera network’ is finally acquired” Blue Highlight [Page 2](zotero://open-pdf/library/items/QE4NXMLH?page=2&annotation=ESLFMBHL) 
 
In order to get a complete 3D reconstruction, the incomplete area of the initial flight should be detected beforehand. Considering the characteristics of the DIM point cloud, the main reason for incompleteness is occlusion” Yellow Highlight [Page 2](zotero://open-pdf/library/items/QE4NXMLH?page=2&annotation=D9LKASEB) 
 
To find and locate the incompleteness (holes), the input DIM point cloud is compared with corresponding triangulated mesh. To make it easier when comparing point cloud with mesh” Yellow Highlight [Page 2](zotero://open-pdf/library/items/QE4NXMLH?page=2&annotation=3I3J8L7X) 
 
A triangulated mesh facilitates the detection of holes. When combined with Poisson Disk Sampling. Poisson Disk Sampling is a technique used to sample a space such that no two samples are closer than a specified minimum distance from each other, while also ensuring that the samples are not too far apart. 
 
 
 
Section 2.1
<b>Objective</b>: The primary aim, as outlined in the excerpt, is to detect and locate areas of incompleteness (often referred to as "holes") in a Dense Image Matching (DIM) point cloud.<b>Comparative Analysis</b>: To achieve this objective, the DIM point cloud is being compared with a corresponding triangulated mesh. A triangulated mesh is a representation of a surface or a 3D object that consists of interconnected triangles. If the mesh is complete and accurate, then areas where the DIM point cloud lacks data (or has "holes") will be evident when contrasted against the mesh.<b>Use of Poisson Disk Sampling</b>: Now, directly comparing a point cloud with a mesh can be a complex task due to the nature of the data structures. To simplify this process, the Poisson Disk Sampling method is employed.<b>Poisson Disk Sampling on Mesh</b>: This sampling method is used to extract sample points, denoted as "s", from the surface of the mesh. Instead of working with the entire mesh, you're effectively working with a set of well-spaced, representative points from the mesh.<b>Ground Sampling Distance (GSD)</b>: The radius of the Poisson disks (i.e., the minimum distance between any two sample points) is set equal to the ground sampling distance. GSD is a term often used in remote sensing and photogrammetry to indicate the distance between two consecutive pixel centers as projected on the ground. By setting the radius of the Poisson disks to the GSD, the sampling method ensures that the extracted points from the mesh are spaced in a manner consistent with the resolution of the original data.In simpler terms, the excerpt is talking about a method to identify missing data or gaps in a 3D point cloud. To do this, they compare the point cloud with a 3D mesh. To make the comparison process easier and more efficient, they use the Poisson Disk Sampling method to extract a representative set of points from the mesh, ensuring these points are spaced based on the resolution of the original imagery or data (the GSD). 
 
nts s from the mesh surface, the radius of Poisson disks equals to the ground sampling distance (GSD)” Green Highlight [Page 2](zotero://open-pdf/library/items/QE4NXMLH?page=2&annotation=VWQITQ6A) 
 
Given Poisson Disk Sample point s, we firstly search for all DIM point p in the k-nearest neighbor of s. Vector ~ np and ~ ns are normals of corresponding points. The hole index fh is related to the dot product of ~ np and ~ ns, which is shown in Equation 1. If the mesh surface is consistent with the DIM point cloud, the dot product will result in a larger value. Otherwise, the result in the neighborhood varies where holes exist, which leads to a smaller fh.” Yellow Highlight [Page 2](zotero://open-pdf/library/items/QE4NXMLH?page=2&annotation=6ISD6BLG) 
 
he curvature index fc represents the local curvature around the sample point s. It sums up the normal change rate rp of the DIM point cloud within the R radius neighbor of s (Equation 2). The radius R equals to the radius of the Poisson disk. A larger fc indicates a higher local normal change rate, which should be considered as unreconstructable vegetation and filtered out.” Yellow Highlight [Page 2](zotero://open-pdf/library/items/QE4NXMLH?page=2&annotation=5HBPA9WS) 
 
By combining the hole index and the curvature index, a filter is created to filter the sample point cloud and the incomplete area is then marked out.” Green Highlight [Page 3](zotero://open-pdf/library/items/QE4NXMLH?page=3&annotation=TF2IPLIY) 
 
the noise of the DIM point cloud is inevitable though” Yellow Highlight [Page 3](zotero://open-pdf/library/items/QE4NXMLH?page=3&annotation=EFUFSN7V) 
 
The inner precision is estimated by computing the error ellipsoid of each point, using the multi-view intersection.” Green Highlight [Page 3](zotero://open-pdf/library/items/QE4NXMLH?page=3&annotation=THARCPU5) 
 
they are located along the normal of each point in the incomplete area, with their viewing directions pointing towards corresponding points.” Yellow Highlight [Page 3](zotero://open-pdf/library/items/QE4NXMLH?page=3&annotation=UAFRKPRP) 
 


%% Import Date: 2023-09-22T17:12:34.860+03:00 %%
