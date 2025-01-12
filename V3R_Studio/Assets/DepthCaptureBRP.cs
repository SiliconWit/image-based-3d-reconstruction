using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class DepthCaptureBRP : MonoBehaviour
{

    public Camera _camera;
    public Material _mat;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    
        
    }

    // Update is called once per frame
    void Update()
    {
        _camera = GetComponent<Camera>();
        _camera.depthTextureMode = DepthTextureMode.DepthNormals;



        if( _mat == null){
            _mat = new Material(Shader.Find("Unlit/DepthCaptureBRPShader"));
        }
    }

    void OnPreRender(){
        Shader.SetGlobalMatrix(Shader.PropertyToID("UNITY_MATRIX_IV"), _camera.cameraToWorldMatrix);
    }
    private void OnRenderImage(RenderTexture source, RenderTexture destination){
        // Graphics.Blit(source, destination);
        _mat.SetTexture("_CamDepthNorm", Shader.GetGlobalTexture("_CameraDepthNormalsTexture"));
        Graphics.Blit(source, destination,_mat);
        Debug.Log("RENDER");
        
    }
}
