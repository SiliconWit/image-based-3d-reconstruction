using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

namespace ThaIntersect.V3R{
    public class VRWayPointGenerator : CameraPoseControl
    {
        
        private void Update() {
            if( Input.GetKeyDown(KeyCode.C) ){
                waypointTransforms.Add( new FakeTransform{
                    position = cameraUnit.transform.position,
                    rotation = cameraUnit.transform.rotation
                });
            }

            if( Input.GetKeyDown(KeyCode.V) ){
                InitialisePhotoShoot();
            }
        }
    }

}
