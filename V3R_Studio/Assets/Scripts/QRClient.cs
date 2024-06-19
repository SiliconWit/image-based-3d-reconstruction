using UnityEngine;
using Grpc.Core;
using Google.Protobuf;
using QRManager;
using System.Threading.Tasks;
using System;

namespace ThaIntersect.V3RLite{
    public class QRClient : MonoBehaviour
    {
        [SerializeField] string serverurl;
        Channel channel;
        [SerializeField] CameraUnit cameraUnit;

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        async void Update()
        {
            if( Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.C) && !Input.GetKey(KeyCode.LeftShift) ){
                byte[] img = cameraUnit.TakeSnap();
                QRManager.ImgByteArr ImgByteArr = new QRManager.ImgByteArr { ByteArr = ByteString.CopyFrom(img) }; // Replace with your image bytes
                await DecodeQR(ImgByteArr);
            }

        }
        
        private void OnDestroy()
        { 
            if (channel != null) channel.ShutdownAsync().Wait();
        }

        public async Task<byte []> GenerateQRCode(QRText qRText){
            try
            {
                var channel = new Channel(serverurl, ChannelCredentials.Insecure);
                var client = new QRManage.QRManageClient(channel);
                
                var response = await client.GenerateQRCodeAsync(qRText);
                byte [] imgbyteArr = response.ByteArr.ToByteArray();
                return imgbyteArr;
            }
            catch (System.Exception)
            {
                throw;
             
            }

        }

        async Task DecodeQR(QRManager.ImgByteArr imgByteArr){
            
            int maxRetryCount = 3; // Maximum number of retries
            int currentRetry = 0;

            while (currentRetry < maxRetryCount)
            {
                try
                {
                    Debug.Log("Establishing Connection: ");
                    channel = new Channel(serverurl, ChannelCredentials.Insecure);
                    var client = new QRManage.QRManageClient(channel);

                    var response = await client.DecodeQRCodeAsync(imgByteArr);
                    Debug.Log($"{response.QRTexts_.Count} QR Codes Decoded");

                    
                    foreach (var qr in response.QRTexts_)
                    {
                        // Debug.Log(qr.Qrtext);
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
}
