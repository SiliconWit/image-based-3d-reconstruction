/*
    --------------------------------------------------------------------------------------
    Script Name: SpiralWaypointGenerator.cs
    Author: Andrew Kibor
    Date: [Script's Last Modified Date]

    Description:
    This script generates a spiral pattern of waypoints around a target object. The waypoints
    are defined in 3D space and the camera, inheriting from CameraPoseControl, moves 
    through these points taking screenshots.

    Features:
    1. Define spiral waypoints based on the target's bounding box.
    2. Automatically sets up camera to start the photo shoot based on these waypoints.
    3. Customizable parameters like Y-displacement, number of photos, and radial distance.
    4. The script also calculates the orientation of the camera to look at the target 
       from each waypoint.

    Components:
    - mult_h: Adjusts the Y-displacement of the waypoints.
    - num_photos: Specifies the number of photos (waypoints) the camera should take.
    - c: Defines the number of poses for a single Y position.
    - mult_r: Adjusts the radial distance from the target to the camera.

    Methods:
    1. Start(): Overrides the base class's Start() method to generate waypoints and initializes the photo shoot.
    2. CreatePosePoints(): Generates the spiral pattern of waypoints based on the given parameters.

    Inheritance:
    This script inherits from the CameraPoseControl class. As a result, it can make use of 
    its methods, fields, and properties, especially the ones related to camera positioning and capturing.

    Note:
    Each waypoint is represented by a temporary GameObject which is destroyed after the waypoint's
    position and rotation are stored.
    --------------------------------------------------------------------------------------
*/
using UnityEngine;

namespace ThaIntersect.V3R
{
    public class SpiralWaypointGenerator : CameraPoseControl
    {
    
        [Tooltip("Number of poses for a single y position")] public int c = 16;        

        // Start is called before the first frame update
        new void Start()
        {
            base.Start();
            CreatePosePoints();
            InitialisePhotoShoot();           
        }

        void CreatePosePoints()
        {
            var y0 = target.position.y -  (boundingBox.y * mult_h)/2;
            for( int i=0; i < num_photos; i++ )
            {
                float theta = 2.0f * Mathf.PI * (i % c) / c;

                float x = target.position.x + (mult_r * r * Mathf.Cos(theta));
                float y = y0 + i * (boundingBox.y * mult_h) / (num_photos - 1);
                float z = target.position.z + (mult_r * r * Mathf.Sin(theta));
                UpdateWayPointsAboutY(x, y, z);
            }
        }        
    }    
}
