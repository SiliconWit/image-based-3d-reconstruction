/*
    --------------------------------------------------------------------------------------
    Script Name: CameraPoseControl.cs
    Author: Andrew Kibor
    Date: 21 Jul 2023

    Description:
    This script is base class for other classes. It controls a camera's positioning and captures screenshots of the game view.
    Features:
    1. Sets the camera's waypoints or positions.
    2. Take screenshots at each waypoint and save them.
    3. Allows camera to look at a specified target while only rotating around the Y-axis.
    4. Organize the saved images based on the specified directory and photo set name.

    Components:
    - Camera poseCamera: The camera performing the captures.
    - List<FakeTransform> waypointTransforms: A list of waypoints for the camera.
    - RenderTexture renderTexture: A texture used to capture the camera's view.
    - Texture2D tex: Texture representation of the captured view.
    - base_dir: Base directory where the screenshots will be saved.
    - PhotoSetName: Name of the photo set, used as a subdirectory within the base directory.
    - boundingBox: A 3D vector specifying the bounding box.
    - Transform target: The object that the camera should focus on or reconstruct.

    Methods:
    1. Start(): Initialization of camera transform.
    2. PositionCamera(): A coroutine that iteratively positions the camera at each waypoint, takes a screenshot, and saves it.
    3. InitialisePhotoShoot(): Setup and start the photoshoot.
    4. LookAtTargetYAxisOnly(Vector3 from): Rotates the object so it faces the target, but only around the Y-axis.

    Note:
    The camera uses the DOTweening library for smooth transitions between waypoints.
    --------------------------------------------------------------------------------------
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using System.Diagnostics;
using UnityEditor;
using ThaIntersect;

namespace ThaIntersect.V3R{    
    public class CameraPoseControl : MonoBehaviour {
        [Tooltip("Reference to the camera unit doing the capture")] [SerializeField] CameraUnit cameraUnit;
        public string pythonExecutablePath = @"ImageCaptures/V3RENV/Scripts/python.exe"; // e.g., "C:\Python38\python.exe"
        public string scriptPath = @"ImageCaptures/append_metadata.py"; // e.g., "Assets/Scripts/append_metadata.py"
        public string photoSetDir = "";
        protected List<FakeTransform> waypointTransforms = new List<FakeTransform>();
        [SerializeField] protected string base_dir = "ImageCaptures";
        protected string savePath, saveDirname;
        [SerializeField] protected Vector3 boundingBox;
        [Tooltip("Object to Reconstruct")] [SerializeField] protected Transform target;
        [Tooltip("Radial distance from the target to the camera")] public float mult_r = 2f;
        [Tooltip("Y Displacement")]public float mult_h = 1f;
        [Tooltip("Number of Photos #")]public float num_photos = 50f;
        [ReadOnly] [SerializeField] float hr_ratio;
        public float r;

        protected void Start()
        {
            if (target == null) target = this.transform;
            saveDirname = $"{photoSetDir}_{num_photos.ToString()}_{mult_h.ToString()}_{mult_r.ToString()}";
            savePath = Directory.GetParent(Application.dataPath) + $"/{base_dir}/{saveDirname}";
            Calc_HR_Ratio();

        }

        void Calc_HR_Ratio()
        {
            r = Mathf.Sqrt(
                            (boundingBox.x * boundingBox.x) + 
                            (boundingBox.z * boundingBox.z)
                        )/2.0f;

            var vfov = cameraUnit.Get_vFov();

            float h_2 = boundingBox.y / 2f;

            float fovL = h_2 / Mathf.Tan(Mathf.Deg2Rad * vfov)/2;

            hr_ratio = fovL/r;
        }





        // Positions the camera and captures screenshots of the gameview
        public IEnumerator PositionCamera(){
            for (int i = 0; i < waypointTransforms.Count; i++)
            {
                
                cameraUnit.MoveCamera( waypointTransforms[i].position, waypointTransforms[i].rotation );    

                yield return new WaitForSeconds(2);


                byte[] bytes = cameraUnit.TakeSnap();

                // Write to a file 
                File.WriteAllBytes($"{savePath}/{(i+1f).ToString()}.jpg", bytes);
                yield return new WaitForSeconds(2);            
            }
            AppendMetadata();
            CreateWaypointsRefFile();

            #if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
            #else
            Debug.Log( "Failed to Exit Playmode" );
            #endif
        }

        private void CreateWaypointsRefFile()
        {
            var GTPoses = new List<GTPose>();
            foreach (var t in waypointTransforms)
            {
                GTPoses.Add(
                    new GTPose(){
                        position = t.position.ToString(),
                        rotation = t.rotation.ToString()
                    }
                );

            }
            string json = JsonConvert.SerializeObject(GTPoses, Formatting.Indented);
            File.WriteAllText(Path.Combine(Application.dataPath, savePath+"/waypoints.json"), json);
            
        }

        void AppendMetadata(){
            var BASE_DIR = Directory.GetParent(Application.dataPath);


            if (!File.Exists($"{BASE_DIR}/{scriptPath}"))
            {
                UnityEngine.Debug.LogError("Python executable not found at: " + pythonExecutablePath);
                return;
            }

            if (!File.Exists(scriptPath))
            {
                UnityEngine.Debug.LogError("Python script not found at: " + scriptPath);
                return;
            }

            try
            {
                
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = pythonExecutablePath,
                    Arguments = $"{BASE_DIR}/{scriptPath} {saveDirname}",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                };

                using (Process _process = new Process { StartInfo = startInfo })
                {
                    _process.Start();

                    // You can read the standard output if needed
                    string _result = _process.StandardOutput.ReadToEnd();

                    _process.WaitForExit();
                    UnityEngine.Debug.Log(_result.ToString()); // Print the output of the Python script to the Unity console
                }


            }
            catch (System.Exception e)
            {
                UnityEngine.Debug.LogError("Failed to run Python script: " + e.Message);
            }
        }

        public void InitialisePhotoShoot(){
            

            if( !Directory.Exists( savePath ) ) {
                var dirInfo = Directory.CreateDirectory(savePath);
                // If the directory is read-only, make it writable
                dirInfo.Attributes &= ~FileAttributes.ReadOnly;
            }else{
                UnityEngine.Debug.Log($"The Folder {savePath} already exits");
            }
            CreateMetadataFile();
            StartCoroutine( PositionCamera() );
        }


        void CreateMetadataFile(){
            var metadata = cameraUnit.GetMetaData();
            string json = JsonConvert.SerializeObject(metadata, Formatting.Indented);
            File.WriteAllText(Path.Combine(Application.dataPath, savePath+"/metadata.json"), json);
        }

        public Quaternion LookAtTargetYAxisOnly(Vector3 from)
        {
                // Compute direction from this object to the target
                Vector3 directionToTarget = target.position - from;

                // Set the direction's y component to zero to only rotate around the Y-axis
                directionToTarget.y = 0;

                // Calculate the rotation
                Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

                // Apply the rotation
                // transform.rotation = targetRotation;
                return targetRotation;            
        }

        protected void UpdateWayPointsAboutY(float x, float y, float z)
        {
            var obj = new GameObject();
            Vector3 point = new Vector3(x, y, z);
            obj.transform.position = point;
            obj.transform.rotation = LookAtTargetYAxisOnly(obj.transform.position);
            waypointTransforms.Add(new FakeTransform
            {
                position = obj.transform.position,
                rotation = obj.transform.rotation
            });
            Destroy(obj);
        }
    }

    public class GTPose{
        public string position;
        public string rotation;
    }

}
