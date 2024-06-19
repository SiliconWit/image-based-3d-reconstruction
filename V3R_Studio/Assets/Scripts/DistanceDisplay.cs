using UnityEngine;
using TMPro;

namespace ThaIntersect
{
    
    public class DistanceDisplay : MonoBehaviour
    {
        public Camera mainCamera;
        public TextMeshProUGUI distanceText;

        void Start()
        {
            if (mainCamera == null)
            {
                // If the main camera is not assigned, assume the main camera of the scene.
                mainCamera = Camera.main;
            }

            if (distanceText == null)
            {
                // If the TextMeshPro component is not assigned, try to find it in the current GameObject.
                distanceText = GetComponent<TextMeshProUGUI>();
            }

            if (mainCamera == null || distanceText == null)
            {
                Debug.LogError("Main Camera or TextMeshPro component not assigned.");
            }
        }

        void Update()
        {
            // Cast a ray from the camera to the world space.
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // Get the distance from the camera to the hit point.
                float distance = Vector3.Distance(mainCamera.transform.position, hit.point);

                // Display the distance in the TextMeshPro component.
                distanceText.text = distance.ToString("F2") + " units";
            }
        }
    }
}
