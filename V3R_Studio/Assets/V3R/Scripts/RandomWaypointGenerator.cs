using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThaIntersect.V3R
{
    public class RandomWaypointGenerator : CameraPoseControl
    {
        [Tooltip("Number of Segments")]public float segments = 4;
        [Tooltip("Number of poses for a single y position")] public int c = 16;
        public bool onlyrandomelevation = false;

        // Start is called before the first frame update
        new void Start()
        {
            base.Start();
            CreatePosePoints();
            InitialisePhotoShoot();
        }

        private void CreatePosePoints()
        {
            float r = Mathf.Sqrt( 
                (boundingBox.x * boundingBox.x) + (boundingBox.z * boundingBox.z)
            );

            var y0 = target.position.y -  (boundingBox.y * mult_h)/2;

            if( onlyrandomelevation ){
                for( int i=0; i < num_photos; i++ )
                {
                    float theta = 2.0f * Mathf.PI * (i % c) / c;
                    float y = y0 + UnityEngine.Random.Range(0, boundingBox.y * mult_h);

                    float x = target.position.x + (mult_r * r * Mathf.Cos(theta));
                    float z = target.position.z + (mult_r * r * Mathf.Sin(theta));
                    UpdateWayPointsAboutY(x, y, z);
                }
            }else{
                var step_angle = (2*Mathf.PI)/segments;
                for( int i = 0; i < segments; i++ ){
                    for( int j = 0; j < num_photos/segments; j++ ){
                        var randomTheta = UnityEngine.Random.Range(i*step_angle, (i+1) * step_angle);
                        float x = target.position.x +  (mult_r * r * Mathf.Cos(randomTheta));
                        float y = y0 + UnityEngine.Random.Range(0, boundingBox.y * mult_h);
                        float z = target.position.z +  (mult_r * r * Mathf.Sin(randomTheta));

                        UpdateWayPointsAboutY(x, y, z);
                    }
                }
            }
            

            


        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
    
}
