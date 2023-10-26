using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThaIntersect.V3R
{
    public class SpiralFocusWaypointGenerator : CameraPoseControl
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
                var obj = new GameObject();
                obj.transform.position = new Vector3(x,y,z);
                obj.transform.LookAt(target.position);
                waypointTransforms.Add(new FakeTransform
                {
                    position = obj.transform.position,
                    rotation = obj.transform.rotation
                });
                Destroy(obj);
            }
        }
    }    
}
