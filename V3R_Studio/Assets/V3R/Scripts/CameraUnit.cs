using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace ThaIntersect.V3R
{
    [RequireComponent(typeof(Camera))]
    public class CameraUnit : MonoBehaviour
    {
        [SerializeField] Metadata metadata;
        [SerializeField] float vFov; 
        Camera _camera;
        private RenderTexture renderTexture;
        private Texture2D tex;

        private void Awake() {
            _camera = GetComponent<Camera>();
            SetupMetadata();
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        void SetupMetadata(){
            metadata.ImageHeight = Screen.height;
            metadata.ImageWidth = Screen.width;
            metadata.FNumber = _camera.aperture;
            metadata.FocalLength = _camera.focalLength;
            metadata.ISOSpeedRatings = _camera.iso;
        }

        // Written Metadata pertaining to the camera.
        public Metadata GetMetaData() =>  metadata;

        // Translates and Rotatest the Camera
        public void MoveCamera(Vector3 pos, Quaternion rot, float t = 1f){
            transform.DOMove( pos, t);
            transform.DORotateQuaternion(rot, t);
        }

        // Get the Vertical FoV
        public float Get_vFov() => vFov;

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
    }
}
