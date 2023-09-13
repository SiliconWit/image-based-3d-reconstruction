using System;
using System.Collections;
using System.Collections.Generic;
using ThaIntersect.V3R;
using UnityEngine;

namespace ThaIntersect
{
    public class MultiRadialWaypointGenerator : CameraPoseControl
    {
        [SerializeField] int levels;
        // Start is called before the first frame update
        new void Start()
        {
            base.Start();
            CreatePosePoints();
            InitialisePhotoShoot();  
        }

        private void CreatePosePoints()
        {
            float y_target = target.position.y;
            for (int i = 0; i < levels; i++)
            {
                if( i == 0 ){
                    var outterelevations = 1;
                    var outterimageNumber = 30;
                    for (int h = 0; h < outterelevations; h++)
                    {
                        for (int j = 0; j < outterimageNumber; j++)
                        {
                            float theta = 2.0f * Mathf.PI * j / outterimageNumber;
                            float x = target.position.x +  (mult_r * r * Mathf.Cos(theta));
                            float z = target.position.z +  (mult_r * r * Mathf.Sin(theta));
                            AppendtoWayPoints( new Vector3(x, y_target, z) );
                        }
                        
                    }
                } 
                
                if( i == 1 ) {
                    var inner_elevations = 3;
                    var innerimageNumber = 6;
                    var y0 = y_target -  (boundingBox.y * mult_h)/2;
                    for (int k = 0; k < inner_elevations; k++)
                    {
                        var y = y0 + ((boundingBox.y * mult_h/(3-1)) * k);
                        for (int l = 0; l < innerimageNumber; l++)
                        {
                            float theta = 2.0f * Mathf.PI * l / innerimageNumber;
                            float x = target.position.x +  (mult_r/1.333f * r * Mathf.Cos(theta));
                            float z = target.position.z +  (mult_r/1.333f * r * Mathf.Sin(theta));
                            AppendtoWayPoints( new Vector3(x, y, z) );
                        }

                    }
                }
            }
        }

        void AppendtoWayPoints( Vector3 _vector3 ){
            var obj = new GameObject();
            obj.transform.position = _vector3;
            obj.transform.rotation = LookAtTargetYAxisOnly( obj.transform.position );
            waypointTransforms.Add( new FakeTransform{
                position = obj.transform.position,
                rotation = obj.transform.rotation
            });
            Destroy(obj);
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
    
}
