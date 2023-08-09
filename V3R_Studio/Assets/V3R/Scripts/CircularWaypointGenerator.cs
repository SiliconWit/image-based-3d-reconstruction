/*
    --------------------------------------------------------------------------------------
    Script Name: CircularWaypointGenerator.cs
    Author: Andrew Kibor
    Date: 21 Jul 2023
    
    Description:
    This script generates a circular pattern of waypoints around a target object at 
    different elevations. The waypoints are defined in 3D space and the camera, 
    inheriting from CameraPoseControl, moves through these points taking screenshots.

    Features:
    1. Define circular waypoints based on the target's bounding box.
    2. Automatically sets up the camera to start the photo shoot based on these waypoints.
    3. Customizable parameters like the number of elevations, Y-displacement, and radial distance.
    4. The script also calculates the orientation of the camera to look at the target 
       from each waypoint.
    5. An option to tilt the camera (though the functionality is implied and not detailed in this script).

    Components:
    - elevations: Total number of circular patterns at different heights.
    - c: Defines the number of poses for a single Y position in a circular pattern.
    - mult_h: Adjusts the Y-displacement of the waypoints.
    - mult_r: Adjusts the radial distance from the target to the camera.
    - tilt: Boolean value to check if the camera should be tilted or not.

    Methods:
    1. Start(): Overrides the base class's Start() method to generate waypoints and initializes the photo shoot.
    2. CreatePosePoints(): Generates the circular pattern of waypoints at different elevations based on the given parameters.

    Inheritance:
    This script inherits from the CameraPoseControl class. It leverages its methods, fields, and properties, 
    especially the ones related to camera positioning and capturing.

    Note:
    Each waypoint is represented by a temporary GameObject which is destroyed after the waypoint's
    position and rotation are stored.
    --------------------------------------------------------------------------------------
*/
using UnityEngine;

namespace ThaIntersect.V3R{
    public class CircularWaypointGenerator : CameraPoseControl
    {
        [Tooltip("Total number of poses")] public int elevations = 3;
        [Tooltip("Number of poses for a single y position")] public int c = 16;

        private void Awake() {

        }

        // Start is called before the first frame update
        new void Start()
        {
            base.Start();
            CreatePosePoints();
            InitialisePhotoShoot();            
        }

        // Creates positions and orientations in 3D space of the camera during "Capture"
        void CreatePosePoints()
        {
            float y = target.position.y;
            
            var y0 = y -  (boundingBox.y * mult_h)/2;

            for (int i = 0; i < elevations; i++)
            {
                y = y0 + ((boundingBox.y * mult_h/(elevations-1)) * i);
                for (int j = 0; j < c; j++)
                {
                    float theta = 2.0f * Mathf.PI * j / c;
                    float x = target.position.x +  (mult_r * r * Mathf.Cos(theta));
                    float z = target.position.z +  (mult_r * r * Mathf.Sin(theta));
                    
                    var obj = new GameObject();
                    Vector3 point = new Vector3(x, y, z);
                    obj.transform.position = point;
                    obj.transform.rotation = LookAtTargetYAxisOnly( obj.transform.position );
                    waypointTransforms.Add( new FakeTransform{
                        position = obj.transform.position,
                        rotation = obj.transform.rotation
                    });
                    Destroy(obj);
                }
            }
        } 
    }    
}
