using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;


namespace ThaIntersect.V3RLite
{
    public class CircularPlacementTool : MonoBehaviour
    {
        
        [SerializeField] int MarkersCount = 12;
        [SerializeField] float radius = 1.5f;
        public GameObject prefabToPlace;
        
        [Button("Spawn Markers")] // Specify button text
        public void SpawnMarkers()
        {
            if (prefabToPlace == null)
            {
                Debug.LogError("Prefab not assigned!");
                return;
            }

            float angleIncrement = 360f / MarkersCount;
            Vector3 centerPosition = transform.position;

            for (int i = 0; i < MarkersCount; i++)
            {
                float angle = i * angleIncrement;
                float x = centerPosition.x + radius * Mathf.Cos(angle * Mathf.Deg2Rad);
                float z = centerPosition.z + radius * Mathf.Sin(angle * Mathf.Deg2Rad);

                // Quaternion.LookRotation(centerPosition)

                Vector3 spawnPosition = new Vector3(x, centerPosition.y, z);
                
                var rotation = UtilFunctions.LookAtTargetYAxisOnly(spawnPosition , centerPosition);

                GameObject spawnedPrefab = Instantiate(prefabToPlace, spawnPosition, rotation);
                if(spawnedPrefab.TryGetComponent<MeshRenderer>(out var m)){ 

                    // Texture2D tex = QRGenerator(spawnPosition.ToString());
                    // spawnedPrefab.GetComponent<MeshRenderer>().material.mainTexture = tex;
                    spawnedPrefab.transform.SetParent(transform);
                }

            }
        }

        

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }

    }
}
