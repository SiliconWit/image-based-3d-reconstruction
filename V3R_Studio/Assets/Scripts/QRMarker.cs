using System.IO;
using NaughtyAttributes;
using QRManager;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEditor.Build;

namespace ThaIntersect.V3RLite
{
    public class QRMarker : MonoBehaviour
    {
        [SerializeField] QRClient qRClient; 
        [SerializeField] string decodedText;

        [Button]
        async void GenerateQR(){
            Vector3 pos = GetComponent<RectTransform>().position;
            string qrString = $"{ Math.Round(pos.x, 2).ToString() },{ Math.Round(pos.y, 2).ToString() },{ Math.Round(pos.z, 2).ToString() }";
            QRText qrText = new QRManager.QRText();
            qrText.Qrtext = qrString;
            byte [] qrimage = await qRClient.GenerateQRCode( qrText );

            var basePath = $"{Application.dataPath}/Resources/QRCodes/";
            var filename = qrString.Replace(",", "_").Replace("(", "").Replace(")", "")+".png";

            try
            {
                File.WriteAllBytes( basePath+filename, qrimage );
                Debug.Log("QRMarker: "+filename+" created Succesfully.");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"QRMarker failed: {e}");
                throw;
            }
            decodedText = qrString;
            // Create a new Texture2D and load the image data
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(qrimage); // This assumes the image is in PNG or JPG format
        }

        [Button("Assign Sprite")]
        void AssignSprite(){
            if(decodedText.Trim().Length >0 ){
                var txt = decodedText.Replace(",", "_").Replace("(", "").Replace(")", "");
                Sprite sprite = Resources.Load<Sprite>("QRCodes/"+txt);
                transform.GetComponentInChildren<Image>().sprite = sprite;      
            }
        }



        // Start is called before the first frame update
        void Start()
        {
            AssignSprite();
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }    
}
