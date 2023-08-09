---
Year: 2014
tags: Source 
Authors: Claire Lartigue, Yann Quinsat, Charyar Mehdi-Souzani, Alexandre Zuquete-Guarato, Shadan Tabibian
---
#path-planning

Title:: Voxel-based Path Planning for 3D Scanning of Mechanical Parts
URL: http://www.cad-journal.net/files/vol_11/Vol11No2.html
Zotero Link: [Lartigue et al. - 2014 - Voxel-based Path Planning for 3D Scanning of Mecha.pdf](zotero://select/library/items/Z4BV6GTB)


The size of each voxel is defined according to the sensor FOV” Yellow Highlight [Page 1](zotero://open-pdf/library/items/Z4BV6GTB?page=1&annotation=SIFPYNYA) 
 
path planning is defined as a set of ordered points of view allowing the part digitizing without collision.” Yellow Highlight [Page 1](zotero://open-pdf/library/items/Z4BV6GTB?page=1&annotation=K8L3PYFK) 
 
Definition of Path Planning. 
 
As the common objective is to minimize cycle time,” Yellow Highlight [Page 1](zotero://open-pdf/library/items/Z4BV6GTB?page=1&annotation=C999GYIZ) 
 
, the main difficulty linked to scan path planning is to define a sensor trajectory, free from collision, that leads to a good surface representation, i.e. slightly noisy and complete.” Yellow Highlight [Page 1](zotero://open-pdf/library/items/Z4BV6GTB?page=1&annotation=WGZTJ7J5) 
 
Most of the methods are based on the concept of visibility. The visibility can be considered either from the part standpoint what are the directions from which a point of the part surface is visible – or from the sensor standpoint what is the surface area visible from the sensor point of view. Visibility is generally linked with the local normal at the point.” Yellow Highlight [Page 1](zotero://open-pdf/library/items/Z4BV6GTB?page=1&annotation=MB2KBFSY) 
 
Visibility is a comman factor for path planning, be it from the perspective of the sensor or the part 
 
propose to plan the path of a laser sensor by using the concept of measurability matrix. The measurability matrix results from the intersection of two matrices, one corresponding to the laser visibility the second one defining the CCD visibility.” Green Highlight [Page 1](zotero://open-pdf/library/items/Z4BV6GTB?page=1&annotation=WF3YA3AZ) 
 
What is CCD? 
 
A point of view is considered as optimal if the view angle, defining the scanning direction, and the scanning distance lie within admissible intervals.” Yellow Highlight [Page 2](zotero://open-pdf/library/items/Z4BV6GTB?page=2&annotation=NHSMXQ7Z) 
 
The CAD model is reduced into a coarse voxel-map allowing the calculation of the local normal vectors, and defining the space volume to calculate collision free trajectory” Orange Highlight [Page 2](zotero://open-pdf/library/items/Z4BV6GTB?page=2&annotation=S349QD22) 
 
What do they mean the CAD model?
This method is however tested on Laser scanners 
 
propose to subdivide the surface into portions that are included in the scanner field of view, and for which the variations of the normal vectors do not exceed a threshold.” Yellow Highlight [Page 2](zotero://open-pdf/library/items/Z4BV6GTB?page=2&annotation=I2QWXSWV) 
 
The next step of the process is point clustering. The points that can be seen from the same point of view are gathered according two conditions: the maximum of the distance between 2 points belonging to the same cluster must be less than the scanner field of view, and the maximum angle between normal vectors must remain less than 90◦.” Green Highlight [Page 2](zotero://open-pdf/library/items/Z4BV6GTB?page=2&annotation=VCBPC4VU) 
 
each sensor is modeled as cone with regard to its field of view as displayed in Fig. 2.” Green Highlight [Page 2](zotero://open-pdf/library/items/Z4BV6GTB?page=2&annotation=G4GKJMUN) 
 
voxel-map allows the identification of empty or surface voxels. In this paper, the concept of voxel-space introduced in [3] is adopted” Blue Highlight [Page 2](zotero://open-pdf/library/items/Z4BV6GTB?page=2&annotation=KXUISJCA) 
 
What do you mean by emtopy of surface voxels 
 
 
 
A surface voxel includes a discrete number of facets, which means that each voxel represents a small portion of the object surface. The vector normal of a voxel is defined by considering the mean value of the normal vectors to each facet included in the voxel. Therefore, facets are used to define the surface of the object, and voxels are used to represent the surface in a 3D space. 
 
 
 
An area under a voxel may have a portion that has an abrupt local curvature which may be rationalised by the voxel normal. Consistency analysis aims to deal with such cases.It takes into account the local normal vector and the mean normal vector(of the voxel) and finds the angular difference. A threshold angle is provided for the same. 
 
![[Media/Papers/lartigueVoxelbasedPathPlanning2014/image-4-x53-y293.png]] 
 
The value of θthreshold is defined from the cone used to describe the sensor. Usually, the value of θthreshold is chosen equal to a quarter of the angle defining this cone.” Green Highlight [Page 4](zotero://open-pdf/library/items/Z4BV6GTB?page=4&annotation=BTLAIH7R) 
 
 
 
Voxel qualification is all about qunatifying how viewable the object is from the sensor. The viewability is based on an angle phi. The angle is a function of the the vector d from the sensor and the voxel normal. The voels considred are only those that have passed consistency tests. 
 
A voxel V will be said seen from a point of view p(M ,   d) if it is the first voxel intercepted by the line issued from M and directed along   d (Fig. 3).” Green Highlight [Page 4](zotero://open-pdf/library/items/Z4BV6GTB?page=4&annotation=VMYPB893) 
 
Also refer to the last image of Fig 2 to get a proper idead of what is M and d. 
 
In fact, if the angle between the digitizing direction and the vector normal to the voxel is greater than a threshold, the quality of the collected points may be affected in terms of trueness or in terms of digitizing noise [4] [1].” Green Highlight [Page 4](zotero://open-pdf/library/items/Z4BV6GTB?page=4&annotation=D78Z9LKF) 
 
The part is modeled as a voxel-map for which the voxel width is given by the size of the Field of View (FOV).” Green Highlight [Page 4](zotero://open-pdf/library/items/Z4BV6GTB?page=4&annotation=CFLUW6IE) 
 
The algorithm consists of 4 main steps: 1. Definition of the voxel-map of the surface and determination of the set of initial points of view 2. Adaptive refinement of the voxel-map to ensure consistency of the normal vectors” Yellow Highlight [Page 4](zotero://open-pdf/library/items/Z4BV6GTB?page=4&annotation=CHM2GZM7) 
 
Qualification of the voxels (well-seen, poorlyseen, not-seen) according to the initial set of points of view” Yellow Highlight [Page 5](zotero://open-pdf/library/items/Z4BV6GTB?page=5&annotation=VQS8TXNN) 
 
Determination of additional points of view for not-seen voxels” Yellow Highlight [Page 5](zotero://open-pdf/library/items/Z4BV6GTB?page=5&annotation=HF2NQP2U) 
 
Voxelization of the part: the size of each voxel is given by the width Lopt of the FOV as proposed in figure 2;” Blue Highlight [Page 5](zotero://open-pdf/library/items/Z4BV6GTB?page=5&annotation=FANBLH26) 
 
The sizing of the voxel may be inconvenient for a camera with a large FOV. 
 
Determination of the set of points of view: the initial voxel-map is included in a parallelepiped the faces of which are perpendicular to the 6 initial directions (Fig. 3); this parallelepiped is enlarged by a distance equal to the optimal digitizing distance Dopt , and the center of each voxel is projected onto the nearest face of the enlarged parallelepiped defining the origin of the point of view M; the direction of the point of view   d is given by the vector normal to the pane” Blue Highlight [Page 5](zotero://open-pdf/library/items/Z4BV6GTB?page=5&annotation=RWJYGMP3) 
 
What I have understood so far, is that a parallelpipe is a cube made of paralelogram faces. It does not make sense for all faces of the parallepiped to be perpendicular to the 6 initial directions. 
 
If the consistency is not ensured, the voxel size is divided by two, and the consistency is assessed for each one of the two new voxels” Green Highlight [Page 5](zotero://open-pdf/library/items/Z4BV6GTB?page=5&annotation=4YYZ7L57) 
 
Nevertheless, a shutoff parameter must be considered here to avoid too small voxel-size” Yellow Highlight [Page 5](zotero://open-pdf/library/items/Z4BV6GTB?page=5&annotation=ZVVBUYTE) 
 
the average mesh size of” Green Highlight [Page 5](zotero://open-pdf/library/items/Z4BV6GTB?page=5&annotation=SIXZKFSE) 
 
The mesh size is calculated by dividing the total surface area by the number of triangles. 
 
two different voxel lists are created in function of each voxel qualification: a list of the seen voxels LS and a list of the LNS not-seen voxels” Green Highlight [Page 5](zotero://open-pdf/library/items/Z4BV6GTB?page=5&annotation=WUNA8VR4) 
 
At the end of the previous step, the list LNS may be not empty, that means that some voxels remain not-seen or poorly-seen. Finding additional directions is thus necessary.” Yellow Highlight [Page 5](zotero://open-pdf/library/items/Z4BV6GTB?page=5&annotation=IS8P796Q) 
 
Finding additional directions is thus necessary” Yellow Highlight [Page 5](zotero://open-pdf/library/items/Z4BV6GTB?page=5&annotation=47CDAXRM) 
 
This solution works as for most of the sensors” Blue Highlight [Page 6](zotero://open-pdf/library/items/Z4BV6GTB?page=6&annotation=M3F5QLGY) 
 
The method is applied to two different sensor technologies: a laser-scanner (Kreon Zephyr KZ 25) and a light-projection system (Atos – GOM)” Yellow Highlight [Page 6](zotero://open-pdf/library/items/Z4BV6GTB?page=6&annotation=4J935YVZ) 
 
The simplicity of the sensor modeling shows its limits as it does not account for the actual sensor visibility” Yellow Highlight [Page 7](zotero://open-pdf/library/items/Z4BV6GTB?page=7&annotation=9L9X69FJ) 
 


%% Import Date: 2023-07-31T16:00:19.979+03:00 %%
