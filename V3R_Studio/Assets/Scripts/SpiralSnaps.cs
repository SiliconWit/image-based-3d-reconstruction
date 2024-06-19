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
        [SerializeField] int elevations = 1;
        [SerializeField] int steps;
        [SerializeField] float rad_mul;
        Vector3 centroid, bounds;
        Vector3 top_pitch_target_pt, btm_pitch_target_pt;
        float pitch_disp;
        [SerializeField] string basename;

        [SerializeField] float pitching_scale = 0.5f;
        
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
            
            norm_rad =  Mathf.Pow( (Mathf.Pow(bounds.x, 2) + Mathf.Pow(bounds.z, 2) ), 1f/2 )/2;
            print($"Normal Radius: {norm_rad}");
            pitch_disp = pitching_scale * bounds.y/2;

            
            btm_pitch_target_pt = new Vector3(
                centroid.x,
                centroid.y - pitch_disp,
                centroid.z                
            );

            top_pitch_target_pt = new Vector3(
                centroid.x,
                centroid.y + pitch_disp,
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
            var y0 = btm_pitch_target_pt.y - per;
            var yL = top_pitch_target_pt.y + per;
            norm_rad = 2;
            for (int i = 0; i < elevations; i++)
            {
        
                for (int j = 0; j < steps; j++)
                {
                    // Getting Position
                    var x =  norm_rad * Mathf.Cos(j * step_size);
                    var z =  norm_rad * Mathf.Sin(j * step_size);
                    float lerp = (float)((i*steps) + j)/((elevations*steps)-1);
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
