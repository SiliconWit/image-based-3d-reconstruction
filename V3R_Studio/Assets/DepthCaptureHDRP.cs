using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using System.IO;

public class DepthCaptureHDRP : MonoBehaviour
{
    public Camera targetCamera;
    public int captureWidth = 1920;
    public int captureHeight = 1080;
    public string fileName = "DepthCapture.exr";

    private RenderTexture depthRT;
    private Material depthMaterial;

    void Start()
    {
        // Create a RenderTexture for the depth
        depthRT = new RenderTexture(Screen.width, Screen.height, 0, RenderTextureFormat.RFloat);
        depthRT.Create();

        // Load the depth shader
        Shader depthShader = Shader.Find("Hidden/Shader/EuclideanDepthRenderer");
        if (depthShader == null)
        {
            Debug.LogError("Depth shader not found!");
            return;
        }

        // Create a material with the depth shader
        depthMaterial = new Material(depthShader);
    }

    void Update()
    {
        // Press Space to capture the depth
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CaptureDepth();
        }
    }

    void CaptureDepth()
    {
        // Render the depth to the RenderTexture
        Graphics.Blit(null, depthRT, depthMaterial);

        // Read the RenderTexture into a Texture2D
        Texture2D tex = new Texture2D(depthRT.width, depthRT.height, TextureFormat.RFloat, false);
        RenderTexture.active = depthRT;
        tex.ReadPixels(new Rect(0, 0, depthRT.width, depthRT.height), 0, 0);
        tex.Apply();

        // Encode to EXR and save
        byte[] bytes = tex.EncodeToEXR(Texture2D.EXRFlags.CompressZIP);
        File.WriteAllBytes(Path.Combine(Application.dataPath, fileName), bytes);

        Debug.Log("Depth image saved to: " + Path.Combine(Application.dataPath, fileName));

        // Clean up
        RenderTexture.active = null;
        Destroy(tex);
    }

    void OnDestroy()
    {
        if (depthRT != null)
        {
            depthRT.Release();
        }
        if (depthMaterial != null)
        {
            Destroy(depthMaterial);
        }
    }
}
