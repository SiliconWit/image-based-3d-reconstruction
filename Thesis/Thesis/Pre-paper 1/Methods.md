#unity 
# Virtual Scene Setup
- [Simple Garage](https://assetstore.unity.com/packages/3d/props/interior/simple-garage-197251) by AiKodex was used as the environment for the scene. The asset features a clean white garage interior with various prop-objects including drawers, machine parts, boxes, shelves, etc. The interior was stripped of all props to have the walls and floor bear. The garage door, walls, floor, ceiling lights and an Emissive window were retained windows. All materials from the asset were converted to HDRP, their mesh renderers set to Contribute to Global Illumination while Receiving Global Illumination through Lightmaps. The floor and Garage Door materials were each adjusted to have a smoothness of 0.4.
- A HDRP scene was created with Ray Tracing effects enabled. A local post-process volume setup with the following Overrides
	- **Visual Environment**: Sky Type ⇾ *None*.
	- **Screen Space Global Illumination:** State → Enabled; Tracing → Ray Tracing (Preview)
	- **Screen Space Ambient Occlusion:** Ray Tracing (Preview) → Checked
	- **Screen Space Reflection:** State(Opaque) → Enabled; Tracing → Ray Tracing (Preview); Mode → Quality
	- **Exposure**: Mode → *Physical Camera*
- Four [Studio light](https://sketchfab.com/3d-models/simple-studio-light-771d351d2329444fab348909ebfd160b) props were added to the scene each having an area light attached to a descendant Game Object. The area light size was 0.5 by 0.5, range of 15 and an intensity of 2000 Lumen.
- One camera was introduced to the scene with the Physical Camera attributes enabled. The [IMX477](https://www.sony-semicon.com/files/62/pdf/p-13_IMX477-AACK_Flyer.pdf) sensor attribute coupled with a [6mm](https://www.adafruit.com/product/4563) lens were used as the basis of the camera configuration. The output images were set to a resolution matching a **Full (4:3) Binning (Normal)** drive mode. The shutter speed and ISO were set to 1/350 and 1200 respectively.
- 
# Target Objects
1. [Transmission 3D Scan - Artec Spider](https://sketchfab.com/3d-models/transmission-3d-scan-artec-spider-9e12a9e6275e4380ab5d51d047d5ee18)  -> The specular value was set to <spec_value> and the Metallic Value to 0.5
2. Stanford Bunny