using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThaIntersect.V3R{
    public class SquareWayPointGenerator : CameraPoseControl
    {
        // Start is called before the first frame update
        new void Start()
        {
            base.Start();   
            CreatePosePoints();
        }

        private void CreatePosePoints()
        {
            float r = Mathf.Sqrt( 
                (boundingBox.x * boundingBox.x) + (boundingBox.z * boundingBox.z)
            );
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }

}
