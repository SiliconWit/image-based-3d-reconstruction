using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using NaughtyAttributes;
using Unity.Mathematics;
using UnityEditor.EditorTools;
using System;

namespace ThaIntersect.V3RLite
{
    public class CircularSnaps : MonoBehaviour
    {
        [SerializeField] CameraUnit cameraUnit;
        [SerializeField] MeshRenderer mesh;
        [ReadOnly] [SerializeField] float norm_rad;
        
        [Tooltip("H : Vertical Levels")]
        [SerializeField] int heights = 3;
        [SerializeField] int steps;
    
        Vector3 centroid, bounds;
        [SerializeField] string basename;
        

        enum LongitudinalLevel{ bareMiss, PerfFit, ExtraLong }
        [SerializeField] LongitudinalLevel longitudinalLevel;
        
        [Tooltip("R' : Distance from the camera to a plane tangent to the furthest point from the medial axis of the target object and perpendicular to the ground ")]
        [ReadOnly][SerializeField] float shortest_dist;
        
        [Tooltip("R : Distance from the medial axis to the camera plane")]
        [ReadOnly][SerializeField] float long_rad;

        [Tooltip("residual from vertical overlap")]
        [ReadOnly][SerializeField] float vert_residual;

        [Tooltip("y_l = Lowest focus point along the medial axis")]
        [ReadOnly][SerializeField] float yl;
        [Tooltip("y_t = Highest focus point along the medial axis")]
        [ReadOnly][SerializeField] float yt;

        [Tooltip("y_0 = Lowest point along the medial axis of the target object")]
        [ReadOnly][SerializeField] float y_0;
        
        [Tooltip("y_H = Highest point along the medial axis of the target object")]
        [ReadOnly][SerializeField] float y_H;
        
        [Tooltip("y_H' = Highest point along the medial axis of the target object")]
        [ReadOnly][SerializeField] float y_H_;

        [Tooltip("Y = The vertical length of the target object")]
        [ReadOnly][SerializeField] float Y;

        [Tooltip("h0 = The vertical length of the target object")]
        [ReadOnly][SerializeField] float h0;

        [Tooltip("An offset to allow multiviewpoints on the top most point")]
        [SerializeField] Transform bottom_tip;
        [Tooltip("Y': Vertical overlap")]
        [SerializeField] float vert_overlap = 0.5f;
        [SerializeField] private bool spiral = true;

        void Start() {
            GetBoundingBox();    
        }

        [Button]
        void GetBoundingBox(){
            if (mesh == null)
            {
                Debug.LogError("Mesh is null");
                return;
            }
            
            bounds = mesh.bounds.size;
            Debug.Log($"Bounds: {bounds}");
            
            centroid = mesh.bounds.center;
            Debug.Log($"Centroid: {centroid}");
            
            var widest =  Mathf.Pow( ( Mathf.Pow(bounds.x, 2) + Mathf.Pow(bounds.z, 2) ), 0.5f );
            
            // calc NormRadius
            norm_rad = widest * 1/Mathf.Tan( Mathf.Deg2Rad * cameraUnit.Get_hFov()/2 ) * 0.5f;

            switch (longitudinalLevel)
            {
                case LongitudinalLevel.bareMiss:
                    long_rad = widest/2f + cameraUnit.Get_NearPlane();
                    break;

                case LongitudinalLevel.PerfFit:
                    long_rad = norm_rad;
                    break;
                
                case LongitudinalLevel.ExtraLong:
                    long_rad = (4*norm_rad)/3 + cameraUnit.Get_NearPlane();
                    break;

                default:
                    long_rad = widest + cameraUnit.Get_NearPlane();
                    break;
            }

            shortest_dist = long_rad - widest/2;
            
            // Define the lowest and upmost limit Y of the camera
            y_0 = bottom_tip.position.y;
            h0 = shortest_dist * Mathf.Tan( Mathf.Deg2Rad * cameraUnit.Get_vFov() );
            y_H = y_0 + bounds.y;
            y_H_ = y_H - (vert_overlap * h0) + h0;
            // Y = y_H_ - y_0;
            Y = y_H;

            var heights_0 = Y/( (1-vert_overlap) * h0 );
            heights = (int)Mathf.Ceil( heights_0 );

            vert_residual = heights - heights_0; 

            yl = long_rad * Mathf.Tan(Mathf.Deg2Rad * cameraUnit.Get_vFov() * 0.5f) + y_0;
            yt = y_H - (yl - y_0); 

        }
        

        [Button]
        IEnumerator GetPoses(){
            var step_size = 2*Mathf.PI/steps;
            
            var vert_step = Y/(heights - 1);
            
            for (int i = 0; i < heights; i++)
            {                
                var y_ = y_0 + (vert_step * i);
                var y_target = ( i * (yt-yl)/(heights-1) ) + yl; 

    
                for (int j = 0; j < steps; j++)
                {
                    if( spiral ){
                        y_ = Mathf.Lerp( y_0, y_H, (float)(j +(i*steps))/(heights * steps) );
                        y_target = Mathf.Lerp( yl, yt, (float)(j +(i*steps))/(heights * steps) );

                    }


                    // Getting Position
                    var x =  long_rad * Mathf.Cos(j * step_size) + centroid.x;
                    var z =  long_rad * Mathf.Sin(j * step_size) + centroid.z;

                    var next_pos = new Vector3( x, y_, z );
                    

                    var target = new Vector3(centroid.x, y_target, centroid.z);
                    var dir = target - next_pos;
                    
                    
                    
                    transform.DOMove(next_pos,.4f);
                    transform.DORotate(Quaternion.LookRotation(dir).eulerAngles, .4f);
                    
                    yield return new WaitForSeconds(1.5f);
                    cameraUnit.SingleSnap(((i*steps) + j).ToString(), basename);
                }
                
            }
            // Quit Play Mode
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }
    }
    
}
