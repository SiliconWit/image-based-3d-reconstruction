/*
 * Script Name: RadialWaypointGenerator.cs
 * Author: Andrew Kibor
 * Created: 06/29/2023
 * Description: This script creates waypoints for camera poses when capturing photos(screenshots) of a target object in the scene. 
*/
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.IO;

namespace ThaIntersect.V3R{
    public class RadialWaypointGenerator : CameraPoseControl
    {
        [Tooltip("Total number of poses")] public int elevations = 3;
        [Tooltip("Number of poses for a single y position")] public int c = 16;
        [Tooltip("Y Displacement")]public float mult_h = .2f;
        
        /// <summary>
        /// 2nd Commit Additions
        /// </summary>

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
            float r = Mathf.Sqrt( 
                (boundingBox.x * boundingBox.x) + (boundingBox.z * boundingBox.z)
            );
            transform.position = new Vector3( 
                target.localPosition.x,
                (target.localPosition.y - (boundingBox.y/2 * mult_h) ) ,
                target.localPosition.z
            );

            float y = transform.position.y;
            
            var y0 = target.position.y - (boundingBox.y * mult_h/2);
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
                    obj.transform.LookAt(target);
                    waypointTransforms.Add( new FakeTransform{
                        position = obj.transform.position,
                        rotation = obj.transform.rotation
                    });
                    Destroy(obj);
                    // octagonPoints.Add(point);
                }
            }
        }

        

       
        
       
    }

    
}
