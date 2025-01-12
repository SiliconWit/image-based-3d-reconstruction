using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using DG.Tweening;
using Newtonsoft.Json;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace ThaIntersect.V3RLite
{
    [RequireComponent(typeof(Camera))]
    public class CameraUnit : MonoBehaviour
    {
        [SerializeField] Metadata metadata;
        [SerializeField] float vFov;
        [SerializeField] float hFov;
        [SerializeField] string StereoSavePath =  "ImageCaptures/Stereo";
        [SerializeField] string SavePath =  "ImageCaptures/";
        [SerializeField] string basePath = "ImageCaptures/";
        [SerializeField] bool depthCam = true;
        string saveDir;
        private string BASE_DIR;

        [SerializeField] Camera _camera;        
        private RenderTexture renderTexture;
        private Texture2D tex;
        
        #if UNITY_EDITOR_LINUX
            public string pythonExecutablePath = @"ImageCaptures/V3RENV/bin/python"; // e.g., "C:\Python38\python.exe"
            public string scriptPath = @"ImageCaptures/append_metadata.py"; // e.g., "Assets/Scripts/append_metadata.py"
        #else
            public string pythonExecutablePath = @"ImageCaptures/V3RENV/Scripts/python.exe"; // e.g., "C:\Python38\python.exe"
            public string scriptPath = @"ImageCaptures/append_metadata.py"; // e.g., "Assets/Scripts/append_metadata.py"
        #endif
        

        private void Awake() {
            if( _camera == null )
                _camera = GetComponent<Camera>();
            
            if( depthCam ) _camera.depthTextureMode = DepthTextureMode.Depth;


            SetupMetadata();            
        }

        void Start(){
            BASE_DIR = $"{Directory.GetParent(Application.dataPath)}";
        }

        public string get_IMG_CAP_DIR() => BASE_DIR+"/"+basePath;

        void SetupMetadata()
        {
            var diagL= metadata.DiagonalLength;
            var cf = (float)(43.27 / 7.857);
            metadata.ImageHeight = Screen.height;
            metadata.ImageWidth = Screen.width;
            metadata.FNumber = _camera.aperture;
            metadata.FocalLength = _camera.focalLength;
            metadata.ISOSpeedRatings = _camera.iso;
            metadata.ExposureTime = (int)(1 / _camera.shutterSpeed);
            metadata.FocalLengthIn35mmFilm = (int)(_camera.focalLength * cf);
        }

        

        // Written Metadata pertaining to the camera.
        public Metadata GetMetaData() =>  metadata;

        // Translates and Rotatest the Camera
        public void MoveCamera(Vector3 pos, Quaternion rot, float t = .5f){
            transform.DOMove( pos, t);
            transform.DORotateQuaternion(rot, t);
        }

        public Vector3 TranslateBy(float displacement, Vector3 direction){
            direction = transform.TransformDirection(direction);
            var pos = direction * displacement;
            transform.position += pos;
            return transform.position;
        }

        // Get the Vertical FoV
        public float Get_vFov() => _camera.fieldOfView;

        // Get the Horizontal FoV
        public float Get_hFov() => _camera.fieldOfView * (4f/3);

        public float Get_NearPlane() => _camera.nearClipPlane;

        internal byte[] TakeSnap()
        {
            renderTexture = new RenderTexture( Screen.width, Screen.height, 32 );
        
            // Convert RenderTexture to Texture2D
            tex = new Texture2D(Screen.width, Screen.height, TextureFormat.ARGB32, false);
            _camera.targetTexture = renderTexture;
            _camera.Render();
            RenderTexture.active = renderTexture;
            tex.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
            tex.Apply();
            
            // Encode Texture2D to JPG
            byte[] bytes = tex.EncodeToJPG();
            Destroy(tex);
            _camera.targetTexture = null;
            
            return bytes;
        }

        public void SetupCapture(string savedir){
            saveDir = $"{basePath}{savedir}";
            if( !Directory.Exists( saveDir ) ) {
                var dirInfo = Directory.CreateDirectory(saveDir);
                // If the directory is read-only, make it writable
                dirInfo.Attributes &= ~FileAttributes.ReadOnly;
            }else{
                UnityEngine.Debug.Log($"The Folder {saveDir} already exits");
            }
        }

        public void CompleteCapture(string savedir){
            CreateMetadataFile(saveDir);
            AppendMetadata(saveDir);
        }

        public void SingleSnap(){
            var now = System.DateTime.UtcNow;
            var ts = ((System.DateTimeOffset)now).ToUnixTimeSeconds().ToString();
            var savedir = $"{basePath}{ts}";

            CreateMetadataFile(savedir);

            byte[] _snappedimage = TakeSnap();

            SaveImage( _snappedimage, savedir, $"{ts}" );

            AppendMetadata(savedir);
        }

        public void SingleSnap(string label, string parDir=""){
            saveDir = basePath+parDir;
            

            byte[] _sappedimage = TakeSnap();
            
            CreateMetadataFile(saveDir);
            SaveImage( _sappedimage, saveDir, $"{label}" );
            AppendMetadata(parDir);
        }

        public IEnumerator StereoSnap(float baseline){
            var now = System.DateTime.UtcNow;
            var ts = ((System.DateTimeOffset)now).ToUnixTimeSeconds().ToString();
            var savedir = $"{StereoSavePath}/{ts}";

            CreateMetadataFile(savedir);

            Vector3 left = _camera.transform.position + _camera.transform.transform.right * -(baseline/2);
            Vector3 right = _camera.transform.transform.position + _camera.transform.transform.right * (baseline/2);

            MoveCamera( left, _camera.transform.rotation,.5f );
            yield return new WaitForSeconds(1f);
            byte[] _leftimage = TakeSnap();
            SaveImage( _leftimage, savedir, $"{ts}_left" );
            

            MoveCamera( right, _camera.transform.rotation,.5f );
            yield return new WaitForSeconds(1f);
            byte[] _rightimage = TakeSnap();
            SaveImage( _rightimage, savedir, $"{ts}_right" );

            AppendMetadata(savedir);
        }

        public void SaveImage(byte[] _image, string dir, string _filename){
            // Write to a file 
            File.WriteAllBytes($"{dir}/{_filename}.jpg", _image);
            
        }

        void AppendMetadata(string savedir){
            


            if (!File.Exists($"{BASE_DIR}/{pythonExecutablePath}"))
            {
                UnityEngine.Debug.LogError("Python executable not found at: " + pythonExecutablePath);
                return;
            }

            if (!File.Exists($"{BASE_DIR}/{scriptPath}"))
            {
                UnityEngine.Debug.LogError("Python script not found at: " + scriptPath);
                return;
            }

            try
            {
                
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = pythonExecutablePath,
                    Arguments = $"{scriptPath} {savedir}",
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

        void CreateMetadataFile(string savedir){
            if( !Directory.Exists( savedir ) ) {
                var dirInfo = Directory.CreateDirectory(savedir);
                // If the directory is read-only, make it writable
                dirInfo.Attributes &= ~FileAttributes.ReadOnly;
            }else{
                // UnityEngine.Debug.Log($"The Folder {savedir} already exits");
            }

            var metadata = GetMetaData();
            string json = JsonConvert.SerializeObject(metadata, Formatting.Indented);
            File.WriteAllText( savedir+"/metadata.json", json);
        }
        

    }
    
    [System.Serializable]
    public class Metadata
    {
        public int ImageWidth;
        public int ImageHeight;
        public string Make;
        public string Model;
        public float FocalLength;
        public int FocalLengthIn35mmFilm;
        public float FNumber;
        public int ExposureTime;
        public int ISOSpeedRatings;
        public string DateTimeOriginal;
        public string DateTimeDigitized;
        public int DiagonalLength;
        public int Flash;
        public int PixelXDimension;
        public int PixelYDimension;
    }
    
}