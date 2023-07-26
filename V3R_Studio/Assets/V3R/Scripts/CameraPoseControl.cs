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
using DG.Tweening;
using Newtonsoft.Json;
using System.Diagnostics;

namespace ThaIntersect.V3R{    
    public class CameraPoseControl : MonoBehaviour {
        Transform camTransform;
        [SerializeField] Metadata metadata;
        [Tooltip("Reference to the camera doing the capture")] [SerializeField] Camera poseCamera;
        public string pythonExecutablePath = @"ImageCaptures/V3RENV/Scripts/python"; // e.g., "C:\Python38\python.exe"
        public string scriptPath = @"ImageCaptures/append_metadata.py"; // e.g., "Assets/Scripts/append_metadata.py"
        public string photoSetDir = "";
        protected List<FakeTransform> waypointTransforms = new List<FakeTransform>();
        private RenderTexture renderTexture;
        private Texture2D tex;
        [SerializeField] protected string base_dir = "ImageCaptures";
        protected string savePath;
        [SerializeField] protected Vector3 boundingBox;
        [Tooltip("Object to Reconstruct")] [SerializeField] protected Transform target;
        [Tooltip("Radial distance from the target to the camera")] public float mult_r = 2f;

        protected void Start() {
            camTransform = poseCamera.transform;
            if( target == null ) target = this.transform;
            
        }

        void setupMetadata(){
            metadata.Make = "RaspberryPi";
            metadata.Model = "RP_imx477";
            metadata.FNumber = poseCamera.aperture;
            metadata.FocalLength = poseCamera.focalLength;
            metadata.ISOSpeedRatings = poseCamera.iso;
            CreateMetadataFile();
        }

        
     
        // Positions the camera and captures screenshots of the gameview
        public IEnumerator PositionCamera(){
            for (int i = 0; i < waypointTransforms.Count; i++)
            {
                
                // camTransform.position = waypointTransforms[i].position;
                camTransform.DORotateQuaternion(waypointTransforms[i].rotation,2);
            
                camTransform.DOMove( waypointTransforms[i].position,2);
                

                yield return new WaitForSeconds(2);
                // ScreenCapture.CaptureScreenshot($"{dir}/{i.ToString()}.png", 1);
                
                renderTexture = new RenderTexture( Screen.width, Screen.height, 24 );
        
                // Convert RenderTexture to Texture2D
                tex = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
                poseCamera.targetTexture = renderTexture;
                poseCamera.Render();
                RenderTexture.active = renderTexture;
                tex.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
                tex.Apply();
                
                // Encode Texture2D to JPG
                byte[] bytes = tex.EncodeToJPG();
                Destroy(tex);
                poseCamera.targetTexture = null;

                // Write to a file 
                File.WriteAllBytes($"{savePath}/{((i+1f)/100f).ToString()}.jpg", bytes);
                yield return new WaitForSeconds(2);            
            }
            AppendMetadata();
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
                    Arguments = $"{BASE_DIR}/{scriptPath} {photoSetDir}",
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
            savePath = Directory.GetParent(Application.dataPath )+"/"+base_dir+"/"+photoSetDir;
            
            if( !Directory.Exists( savePath ) ) {
                var dirInfo = Directory.CreateDirectory(savePath);
                // If the directory is read-only, make it writable
                dirInfo.Attributes &= ~FileAttributes.ReadOnly;
            }else{
                UnityEngine.Debug.Log($"The Folder {savePath} already exits");
            }
            setupMetadata();
            StartCoroutine( PositionCamera() );
        }

        void CreateMetadataFile(){
            
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
    }


}
