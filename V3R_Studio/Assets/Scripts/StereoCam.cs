using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThaIntersect.V3RLite{
    [RequireComponent(typeof(CameraUnit))]
    public class StereoCam : MonoBehaviour
    {
        [SerializeField] float baseline = 0.06f;
        [SerializeField] CameraUnit cameraUnit;
        // Start is called before the first frame update
        void Start()
        {
            cameraUnit = GetComponent<CameraUnit>();
            
        }

        // Update is called once per frame
        void Update()
        {
            if(Input.GetKeyDown(KeyCode.V)){
                StartCoroutine(cameraUnit.StereoSnap(baseline));
            }
        }

        void SnapStereoPhoto(){
            
        }
    }
}
