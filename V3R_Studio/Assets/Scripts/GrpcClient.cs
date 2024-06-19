using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Google.Protobuf;
using Grpc.Core;
using NaughtyAttributes;
using TMPro;
using UnityEditor.PackageManager;
using UnityEngine;
using V3R;


namespace ThaIntersect.V3RLite
{
    public class GrpcClient : MonoBehaviour
    {
        Channel channel;
        // V3Rrelay.V3RrelayClient client;
        [SerializeField] CameraUnit cameraUnit;
        [SerializeField] int calib_images = 5;
        List<NBVPose> capturePoses;
        [SerializeField] float min;
        public string host = "http://localhost:49990";
        public string saveDir = "sources";
        public string targetDir = "sources";
        int counter = 0;
        [SerializeField] GameObject arrowGizmo;
        Transform refPose;
        [SerializeField] bool pose_gizmo, capture_photos;

        [Button]
        public void Set_SFM(){
    
            channel = new Channel("127.0.0.1:49990",ChannelCredentials.Insecure);
            var client = new V3Rrelay.V3RrelayClient(channel);
            var input = new SFM_Args{ Sourcedir=targetDir };
            var response = client.ComputeSFM(input);           
        }

        [Button]
        public void TriggerNBV(){
            GetNBVPoses();
        }

        // Start is called before the first frame update
        void Start()
        {
            cameraUnit.SetupCapture(saveDir);
        }        

        private void OnDestroy()
        { 
            if (channel != null) channel.ShutdownAsync().Wait();

            cameraUnit.CompleteCapture(saveDir);
        }

        // Update is called once per frame
        void Update()
        {
            if(Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.C)){
                
            }
        }

        private void LateUpdate() {
            // print(depthData.Mean.ToString());
        }

        IEnumerator TakePhotos(){           
            foreach (var pose in capturePoses)
            {
                counter++;
                cameraUnit.MoveCamera(pose.pos, pose.eul);
                yield return new WaitForSeconds(1f);  
                if( capture_photos )
                    cameraUnit.SingleSnap(counter.ToString(), saveDir);
            }            
        }

        public async void GetNBVPoses(){
            refPose = cameraUnit.transform;
            var _pos = new Vec3 { X = refPose.position.x, Y = refPose.position.y, Z = refPose.position.z };
            var _rot = new Quat{ 
                            W=refPose.rotation.w,
                            X=refPose.rotation.x,
                            Y=refPose.rotation.y,
                            Z=refPose.rotation.z
                        };
            // Read image bytes from file (replace with your image handling logic)
            byte[] imageBytes = cameraUnit.TakeSnap();

            // Prepare input data for DerivePoses
            NBVInput input = new NBVInput
            {
                RefPose = new CamPose{ Location=_pos, Orientation=_rot },                    
                Image = new ImgByteArr { ByteArr = ByteString.CopyFrom(imageBytes) } // Replace with your image bytes
            };
            // await CallDerivePoses(input);   
            await CallDerivePosesWithRetry(input);
            StartCoroutine(TakePhotos());
        }

        async Task CallDerivePosesWithRetry(NBVInput input)
        {
            int maxRetryCount = 3; // Maximum number of retries
            int currentRetry = 0;

            while (currentRetry < maxRetryCount)
            {
                try
                {
                    channel = new Channel("127.0.0.1:49990",ChannelCredentials.Insecure);
                    var client = new V3Rrelay.V3RrelayClient(channel);

                    var response = await client.DerivePosesAsync(input);
                    Debug.Log($"{response.Poses.Count} Poses determined");
                    
                    capturePoses = new List<NBVPose>();
                    
                    foreach (var pose in response.Poses)
                    {
                        var _pos = new Vector3(
                            pose.Location.X,
                            pose.Location.Y,
                            pose.Location.Z
                        );
                        var _rot = new Quaternion(
                            pose.Orientation.X,
                            pose.Orientation.Y,
                            pose.Orientation.Z,
                            pose.Orientation.W
                        );

                        var _eul = Quaternion.Euler( pose.Eulers.X, pose.Eulers.Y, pose.Eulers.Z );


                        var new_eul = refPose.transform.rotation.eulerAngles - new Vector3( pose.Eulers.X, -pose.Eulers.Y, refPose.transform.rotation.eulerAngles.z );
                        // var new_pose = refPose.transform.rotation.eulerAngles - new Vector3( pose.Eulers.X, pose.Eulers.Y, pose.Eulers.Z );
                        if( pose_gizmo )
                            Instantiate( arrowGizmo, _pos+refPose.position, Quaternion.Euler(new_eul));
                        
                        capturePoses.Add( new NBVPose(){pos=_pos+refPose.position, rot=_rot, eul=Quaternion.Euler(new_eul)});
                        // arrow.transform.localScale = new Vector3( 0.05f, 0.05f, 0.05f );

                    }
                    // Close the Channel
                    channel.ShutdownAsync().Wait();
                    // If successful, break out of the retry loop
                    break;
                }
                catch (RpcException e)
                {
                    Debug.LogError($"RPC failed: {e}");

                    currentRetry++;
                    if (currentRetry < maxRetryCount)
                    {
                        // Add a delay before retrying (you can adjust the delay time)
                        await Task.Delay(1000); // Wait for 1 second before retrying
                    }
                    else
                    {
                        Debug.LogError("Maximum retry count reached. Operation failed.");
                    }
                }
            }
        }
        
    }

    class NBVPose{
        public Vector3 pos;
        public Quaternion rot;
        public Quaternion eul;
    }
}

