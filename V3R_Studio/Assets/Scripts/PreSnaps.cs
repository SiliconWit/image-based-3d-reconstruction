using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System.Data.Common;
using System.Threading;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEditor;
using JetBrains.Annotations;
using System.Collections;


namespace ThaIntersect.V3RLite{
    public class PreSnaps : MonoBehaviour
    {
        [SerializeField] CameraUnit cameraUnit;
        [SerializeField] string saveDir;
        [SerializeField] string filename;
        Vector3 origin_pos;
        Quaternion origin_rot;
        bool ref_photo;
        [SerializeField] List<FakeTransform> PrePoses = new List<FakeTransform>();
        [SerializeField] int counter;
        private Thread meshroomThread;
        bool isThreadRunning = false;

        Transform yaw, pitch;
        

        [Button("Set PrePose")]
        void SetPose(){
            PrePoses.Add(
                new FakeTransform{
                    position = transform.position,
                    rotation = transform.rotation
                 }    
            );
        }

        [Button("TakeSnaps")]
        void TestSnaps(){
            StartCoroutine(nameof(TakeSnaps));
        }

       
        void Start()
        {
            pitch = (transform.parent == null) ? transform : transform.parent ;
            yaw = pitch.parent;
            cameraUnit = GetComponent<CameraUnit>();            
            origin_pos = transform.position;
            origin_rot = transform.rotation;
        }

        // Update is called once per frame
        void Update()
        {
            // if( Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.N)){                
            //     if( !ref_photo ){
            //         cameraUnit.SingleSnap($"{filename}",saveDir);
            //         ref_photo = true;
            //     }else{                    
            //         if( counter < 4 ) cameraUnit.MoveCamera( PrePoses[counter].position, PrePoses[counter].rotation, .2f );
            //     }
            // }

            if( Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.V) ){                
                cameraUnit.SingleSnap($"{filename}_{counter.ToString()}",saveDir);
                counter++;

            }

      
            
        }

        public IEnumerator TakeSnaps(){
            yield return new WaitForSeconds(0.5f);
            cameraUnit.SingleSnap($"{filename}",saveDir);
            yield return new WaitForSeconds(1.0f);
            for (int i = 0; i < 4; i++)
            {
                switch (i)
                {
                    case 0:
                        yaw.Rotate( Vector3.up, 12f );
                        pitch.Rotate( Vector3.right, 12f );
                        break;
                    
                    case 1:
                        yaw.Rotate( Vector3.up, -24f );
                        pitch.Rotate( Vector3.right, 0f );
                        break;

                    case 2:
                        yaw.Rotate( Vector3.up, 0f );
                        pitch.Rotate( Vector3.right, -24f );
                        break;

                    case 3:
                        yaw.Rotate( Vector3.up, 24f );
                        pitch.Rotate( Vector3.right, 0f );
                        break;
                    default:
                        break;
                }
                yield return new WaitForSeconds(1.5f);
                cameraUnit.SingleSnap($"{filename}_{i.ToString()}",saveDir);
            }
            
            yaw.Rotate( Vector3.up, -12f );
            pitch.Rotate( Vector3.right, 12f );

            if( !isThreadRunning ){
                // Start a new thread
                meshroomThread = new Thread( () => RunMeshroomBatch(cameraUnit.get_IMG_CAP_DIR(), saveDir) );
                meshroomThread.Start();
                isThreadRunning = true;
            }
        }

        public int RunMeshroomBatch(string _BASE_DIR, string dir)
        {
            // Escape the prefix string for safe usage in arguments
            string escapedPrefix = System.Text.RegularExpressions.Regex.Escape(dir);
            
            // Build the complete command with arguments
            // string command = $"meshroom_batch --input {BASE_DIR}/{escapedPrefix}/photos --output {BASE_DIR}/{escapedPrefix} --pipeline {BASE_DIR}/workflow.mg";
            string command = $"--input {_BASE_DIR}/{escapedPrefix}/photos --output {_BASE_DIR}/{escapedPrefix}/ --pipeline {_BASE_DIR}/workflow.mg --forceCompute";

            // Configure process start information
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                // FileName = "/bin/bash",
                FileName = $"{System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile)}/Apps/Meshroom-2023.3.0/meshroom_batch",
                // Arguments = $"-c {command}",
                Arguments = command,
                UseShellExecute = false,
                RedirectStandardError = true,
            };

            // Start the process and capture the exit code
            using (Process process = Process.Start(startInfo))
            {
                string error = process.StandardError.ReadToEnd();
                process.WaitForExit();

                // Check for errors
                if (process.ExitCode != 0)
                {
                    UnityEngine.Debug.Log($"Error running meshroom_batch: {error}");
                    System.Console.WriteLine($"Error running meshroom_batch: {error}");
                }
                
                return process.ExitCode;
            }
        }
       
    }

}

