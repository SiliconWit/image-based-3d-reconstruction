using UnityEngine;

[RequireComponent(typeof(Camera))]
public class EnableDepthTexture : MonoBehaviour
{
    void Start()
    {
        // Get the Camera component attached to this GameObject
        Camera camera = GetComponent<Camera>();

        // Enable depth texture mode
        camera.depthTextureMode = DepthTextureMode.Depth;
    }
}
