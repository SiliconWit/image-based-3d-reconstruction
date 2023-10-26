using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThaIntersect.V3R
{    
    public class CircularFocusWaypointGenerator : CircularWaypointGenerator
    {
        // Start is called before the first frame update
        new void Start()
        {
            base.Start();
            CreatePosePoints();
            InitialisePhotoShoot();            
        }        
        

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
                    var forward = target.position - point;
                    obj.transform.position = point;
                    obj.transform.rotation = Quaternion.LookRotation(forward); //LookAtTargetYAxisOnly( obj.transform.position );
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

