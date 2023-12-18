
Joseph Maina
3:43 PM
So do you consider constant radius for your data photos or will it be variable ?
	- The radius will be variable depending on the geometry of the object

Mercy
3:44 PM
What are the possible practical applications of your research?
	- They range from preservation archaeological artefacts to all kinds of metrological practices. In Manufacturing, this can include measurement of deformation after impact testing and quality inspection of machined parts.

Aggrey Omondi
3:46 PM
@Kibor are you using webot software for robot path planning... If so how will you integrate that with unity virtual environment
	- Yes. Webot will be integrated with Unity. I chose Webot because of it's available and has a library of standard robots that one can work with. It also has APIs that can allow it to work with software external to it. I have not explored this API deeply enough but I know it works with ROS and that can be a hub from which information between Unity and Webot flow.

Amos Munene
3:51 PM
Instead of monocular camera, can lidar be applicable instead?
	- Lidar can be used for 3D reconstruction. It is an alternative to Photogrammetry. However, my work may not benefit lidar that much as the algorithm I plan to develop will work exclusively with 2D images  from a monocular vision(RGB) sensor.

CMOS vs CCD cameras
The ideal camera for this application is one that is lightweight, offers a High Dynamic Range(HDR) and consumes low power since it is to be integrated with a robotic system. With CMOS sensor technology having bridged the gap between itself and the CCD counterparts, CMOS satisfies the aforementioned requirements better. A Sony IMX477, a CMOS sensor, is considered for the study as it is one of the most accessible sensors for embedded systems and robotics. The choice would allow for higher reproducibility of the results of the study.