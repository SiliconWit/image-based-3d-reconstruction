using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThaIntersect.V3R
{
    public class V3R : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
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
        public int Orientation;
        public int Flash;
        public int PixelXDimension;
        public int PixelYDimension;
    }
    
}
