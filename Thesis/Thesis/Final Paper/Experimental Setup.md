
#unity #computervision 
# Virtual Scene Setup
- [Simple Garage](https://assetstore.unity.com/packages/3d/props/interior/simple-garage-197251) by AiKodex was used as the environment for the scene. The asset features a clean white garage interior with various prop-objects including drawers, machine parts, boxes, shelves, etc. The interior was stripped of all props to have the walls and floor bear. The garage door, walls, floor, ceiling lights and an Emissive window were retained windows. All materials from the asset were converted to HDRP, their mesh renderers set to Contribute to Global Illumination while Receiving Global Illumination through Lightmaps. The floor and Garage Door materials were each adjusted to have a smoothness of 0.4.
- A HDRP scene was created with Ray Tracing effects enabled. A local post-process volume setup with the following Overrides
	- **Visual Environment**: Sky Type ⇾ *None*.
	- **Screen Space Global Illumination:** State → Enabled; Tracing → Ray Tracing (Preview)
	- **Screen Space Ambient Occlusion:** Ray Tracing (Preview) → Checked
	- **Screen Space Reflection:** State(Opaque) → Enabled; Tracing → Ray Tracing (Preview); Mode → Quality
	- **Exposure**: Mode → *Physical Camera*
- One camera was introduced to the scene with the Physical Camera attributes enabled. The [IMX477](https://www.sony-semicon.com/files/62/pdf/p-13_IMX477-AACK_Flyer.pdf) sensor attribute coupled with a [6mm](https://www.adafruit.com/product/4563) lens were used as the basis of the camera configuration. The output images were set to a resolution matching a **Full (4:3) Binning (Normal)** drive mode. The shutter speed and ISO were set to 1/350 and 1200 respectively.
## Capture Protocol
### Control dataset
The convergent image capture approach was chosen for creating the control datasets for each target object.
Each dataset consist of 3 sets of N uniformly displaced viewpoints such that the angular spacing $\frac{2π}{N}$ results in an even number of subdivisions, ensuring that $N$ is an even integer divisor of $2\pi$ and the yields a lateral displacement $b$ across successive viewpoints resulting in a quality factor of ~6.22 for a given camera-distance $r$ which determined by the target objects bounding box cross-section diagonal. Each set is at a different elevation with a pitch biased towards the centroid of the target object.

### NBV Augmented Datasets
A second dataset would comprise of a hybrid approach that utilizes 40% of the control viewpoints, half of the upper and lowest sets of images with uniform lateral displacement. This results in an effectively wider lateral displacement $\frac{4π}{N}$ and lower quality factor $b$. These viewpoints would be combined with M viewpoints from the Novel View Synthesis step to form the second dataset.

## Talking points for Capture Protocol
### Epipolar Geometry
Epipolar geometry emerges naturally from the physical positioning of cameras and is mathematically represented through calculated matrices. Here's how it works:
The epipolar geometry itself is a physical reality that exists whenever two cameras view the same scene from different positions. It's not something that's "created" but rather a geometric relationship that's inherent to multiple-view scenarios.
**Key Concepts:**
- **Epipole:** The image of one camera's center in the other camera's image plane.
- **Epipolar Plane:** A plane that contains the baseline (the line connecting the two camera centers) and a 3D point in space. Each epipolar plane intersects the image planes of both cameras, creating **epipolar lines**.
- **Epipolar Line:** The line in one image where the corresponding point in the other image must lie, given a specific point in the first image.
![[Pasted image 20250303215438.png]]
The relationship between corresponding points across two images can be described using a mathematical construct known as the **fundamental matrix**. This matrix encapsulates the intrinsic projective geometry between the two views and is crucial for establishing point correspondences.
What we calculate are mathematical representations of this geometry:
1. **The Fundamental Matrix (F)** - This 3×3 matrix encodes the epipolar geometry between two uncalibrated cameras. It maps points in one image to corresponding epipolar lines in the other image.
2. **The Essential Matrix (E)** - For calibrated cameras, this 3×3 matrix represents the same epipolar relationships but in normalized image coordinates.
3. **The Relative Orientation** - This describes the rotation and translation between camera positions, from which the essential matrix can be derived.