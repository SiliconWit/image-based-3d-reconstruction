using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using DG.Tweening;
using System;

namespace ThaIntersect.V3RLite
{
    public class SpiralSnaps : MonoBehaviour
    {
        [SerializeField] CameraUnit cameraUnit;
        [SerializeField] MeshRenderer mesh;
        [ReadOnly] [SerializeField] float norm_rad;
        [SerializeField] int heights = 1;
        [SerializeField] int steps;
        [SerializeField] float rad_mul;
        Vector3 centroid, bounds;
        Vector3 top_pitch_target_pt, btm_pitch_target_pt;
        float pitch_disp;
        [SerializeField] string basename;
        enum LongitudinalLevel{ bareMiss, PerfFit, ExtraLong }
        [SerializeField] LongitudinalLevel longitudinalLevel;
        [SerializeField] float pitching_scale = 0.5f;
        [ReadOnly][SerializeField] float long_rad;
        float pitch_allowance;
        private float pitch_ypos;
        [SerializeField] private float top_offset_ratio = 1.15f;

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
            GetPoses(); 
            
            var widest =  Mathf.Pow( ( Mathf.Pow(bounds.x, 2) + Mathf.Pow(bounds.z, 2) ), 0.5f );
            print($"Widest Edge: {widest}");

            // calc NormRadius
            norm_rad = widest * 1/Mathf.Tan( Mathf.Deg2Rad * cameraUnit.Get_hFov()/2 ) * 0.5f;
            print($"Normal Radius: {norm_rad}");

            switch (longitudinalLevel)
            {
                case LongitudinalLevel.bareMiss:
                    long_rad = widest/2f + cameraUnit.Get_NearPlane();
                    break;

                case LongitudinalLevel.PerfFit:
                    long_rad = norm_rad + cameraUnit.Get_NearPlane();
                    break;
                
                case LongitudinalLevel.ExtraLong:
                    long_rad = (4*norm_rad)/3 + cameraUnit.Get_NearPlane();
                    break;

                default:
                    long_rad = widest + cameraUnit.Get_NearPlane();
                    break;
            }

            /// <summary>
            /// Find pitching scale
            /// It is a function of the h and H
            /// Where:
            ///     h is the vertical distance length covered by long_rad
            ///     H is the vertical length of the target Object
            /// </summary>
            var h = long_rad * Mathf.Tan( .5f * cameraUnit.Get_vFov() * Mathf.Deg2Rad );
            pitching_scale = 1 - ((1.8f * h)/bounds.y) * pitch_allowance;
            heights = (int)Mathf.Ceil( (3*bounds.y)/(4*h) );

            pitch_ypos = pitching_scale * bounds.y/2;

            
            btm_pitch_target_pt = new Vector3(
                centroid.x,
                centroid.y - pitch_ypos,
                centroid.z                
            );

            top_pitch_target_pt = new Vector3(
                centroid.x,
                centroid.y + pitch_ypos,
                centroid.z                
            );

            
        }
        
        [Button]
        void GetBottomIncline(){            
            var bottom_pos = new Vector3(
                centroid.x,
                centroid.y - bounds.y/2,
                centroid.z
            );
            Vector3 bottom_dir = bottom_pos - cameraUnit.transform.position;
            print($"Print {bottom_pos}");
            // bottom_dir.y = 0;
            // Create a rotation based on the direction
            Quaternion rotation = Quaternion.LookRotation(bottom_dir);

            transform.rotation = Quaternion.LookRotation(bottom_dir);


            var top_pos = new Vector3(
                centroid.x,
                centroid.y + bounds.y/2,
                centroid.z
            );
            Vector3 top_dir = top_pos - cameraUnit.transform.position;
            top_dir.y = 0; 
            transform.rotation = Quaternion.LookRotation(top_dir);
        }

        [Button]
        IEnumerator GetPoses(){
            var step_size = 2*Mathf.PI/steps;
            var per = (1f - pitching_scale) * (bounds.y*pitching_scale)/2;
            var vert_step = bounds.y/(heights + 1);
            var y0 = (centroid.y - bounds.y/2) + vert_step;
            var yL = bounds.y * top_offset_ratio;
            for (int i = 0; i < heights; i++)
            {        
                for (int j = 0; j < steps; j++)
                {
                    // Getting Position
                    var x =  norm_rad * Mathf.Cos(j * step_size);
                    var z =  norm_rad * Mathf.Sin(j * step_size);
                    float lerp = (float)((i*steps) + j)/((heights*steps)-1);
                    var y = Mathf.Lerp(y0, yL, lerp);

                    var next_pos = new Vector3( x, y, z );
                    
                    var yt = Mathf.Lerp(
                        btm_pitch_target_pt.y,
                        top_pitch_target_pt.y,
                        lerp
                    );
                    var target = new Vector3(centroid.x, yt, centroid.z);
                    var dir = target - next_pos;
                    
                    // transform.position = next_pos;
                    // transform.rotation = Quaternion.LookRotation(dir);
                    
                    transform.DOMove(next_pos,.7f);
                    transform.DORotate(Quaternion.LookRotation(dir).eulerAngles,.7f);
                    
                    yield return new WaitForSeconds(1f);
                    cameraUnit.SingleSnap(((i*steps) + j).ToString(), basename);
                }
                
            }
        }
    }
    
}
