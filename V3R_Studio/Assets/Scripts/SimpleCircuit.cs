using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ThaIntersect.V3RLite;
using NaughtyAttributes;
using UnityEngine.UIElements;

public class SimpleCircuit : MonoBehaviour
{

    [SerializeField] CameraUnit cameraUnit;
    [SerializeField] int steps;
    [SerializeField] string basename = "Tree_Single_Elev_72";

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [Button]
    public IEnumerator TakePhotos(){
        for (int i = 0; i < steps; i++)
        {
            transform.Rotate(new Vector3(0,360/steps,0));
            yield return new WaitForSeconds(1);
            int _angle = i * 360/steps;
            cameraUnit.SingleSnap(_angle.ToString(), basename);
        }
    }
}
