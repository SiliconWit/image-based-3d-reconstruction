/*
 * Script Name: RadialWaypointGenerator.cs
 * Author: Andrew Kibor
 * Created: 06/29/2023
 * Description: This script creates waypoints for camera poses when capturing photos(screenshots) of a target object in the scene. 
*/
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace ThaIntersect.V3R{
    public class RadialWaypointGenerator : MonoBehaviour
    {
        [Tooltip("Object to Reconstruct")] [SerializeField] Transform target;
        [Tooltip("Reference to the camera doing the capture")] [SerializeField] Transform photoCam;
        [Tooltip("Total number of poses")] public int n = 50;
        [Tooltip("Number of poses for a single y position")] public int c = 16;
        [Tooltip("Y Displacement")]public float h = .2f;
        [Tooltip("Radial distance from the target to the camera")] public float r = 1f;
        List<Vector3> octagonPoints = new List<Vector3>();
        List<FakeTransform> octagonTransforms = new List<FakeTransform>();

        // Start is called before the first frame update
        void Start()
        {
            CreatePosePoints();
            StartCoroutine(poseCamera());
        }

        // Creates positions and orientations in 3D space of the camera during "Capture"
        void CreatePosePoints()
        {
            for (int i = 0; i < n / c; i++)
            {
                float y = target.position.y + (i * h);
                for (int j = 0; j < c; j++)
                {
                    float theta = 2.0f * Mathf.PI * j / c;
                    float x = target.position.x +  (r * Mathf.Cos(theta));
                    float z = target.position.z +  (r * Mathf.Sin(theta));
                    var obj =new GameObject();
                    Vector3 point = new Vector3(x, y, z);
                    obj.transform.position = point;
                    obj.transform.LookAt(target);
                    octagonTransforms.Add( new FakeTransform{
                        position = obj.transform.position,
                        rotation = obj.transform.rotation
                    });
                    Destroy(obj);
                    // octagonPoints.Add(point);
                }
            }
        }

        // Positions the camera and captures screenshots of the gameview
        IEnumerator poseCamera(){
            for (int i = 0; i < octagonTransforms.Count; i++)
            {
                photoCam.position = octagonTransforms[i].position;
                photoCam.rotation = octagonTransforms[i].rotation;
                yield return new WaitForSeconds(1);
                ScreenCapture.CaptureScreenshot($"ImageCaptures/{i.ToString()}.png", 1);
                yield return new WaitForSeconds(1);            
            }
        }
    }

    // A class analogous to Unty's Transform for storing Position and Rotation only 
    public class FakeTransform{
        public Vector3 position;
        public Quaternion rotation;
    }
}
