using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements.Experimental;

namespace ThaIntersect.V3RLite{
    [RequireComponent(typeof(CameraUnit))]
    public class SingleSnap : MonoBehaviour
    {
        [SerializeField] float baseline = 0.06f;
        [SerializeField] CameraUnit cameraUnit;
        [SerializeField] string saveDir;
        [SerializeField] string filename;
        int counter = 0;
        enum PosePatterns{  Radial2, Quad5}
        [SerializeField] PosePatterns pattern;
        [SerializeField] List<PoseSteps> radial2 = new List<PoseSteps>();
        [SerializeField] List<PoseSteps> quad5 = new List<PoseSteps>();
        Vector3 origin_pos;
        Quaternion origin_rot;
        GameObject refFrame;


        // Start is called before the first frame update
        void Start()
        {
            cameraUnit = GetComponent<CameraUnit>();            
            origin_pos = transform.position;
            origin_rot = transform.rotation;
            refFrame = new GameObject("RefFrame");
            refFrame.transform.SetParent(transform.parent);
            refFrame.transform.position = new Vector3(0,0,0);
            refFrame.transform.rotation = transform.rotation;     
            
        }

        void GenPoses(){
            transform.position = origin_pos;
            transform.rotation = origin_rot;

            if( pattern == PosePatterns.Radial2 && counter < radial2.Count ){
                var axis = ( radial2[counter].axis == Vector3.up  ) ? refFrame.transform.up : refFrame.transform.right;
                transform.RotateAround(transform.parent.position, axis, radial2[counter].angle);
                counter+=1;           }
            
            if( pattern == PosePatterns.Quad5 && counter < quad5.Count ){
                var axis = ( quad5[counter].axis == Vector3.up  ) ? refFrame.transform.up : refFrame.transform.right;
                transform.RotateAround(transform.parent.position, axis, quad5[counter].angle);
                counter+=1;
            }
        }
        
        void RadialLinear3Move(){
            transform.RotateAround(transform.parent.position,Vector3.up, -15f);
            
        }

        // Update is called once per frame
        void Update()
        {
            if( Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.N)){
                GenPoses();
            }

            if( Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.V)){
                cameraUnit.SingleSnap($"{filename}_{counter.ToString()}",saveDir);
            }
        }

        void SnapStereoPhoto(){
            
        }
    }

    [Serializable]
    public class PoseSteps{
        public Vector3 axis;
        public float angle;
    }
}
