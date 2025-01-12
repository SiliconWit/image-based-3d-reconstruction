using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

[System.Serializable, VolumeComponentMenu("Post-processing/Custom/DepthTextureRenderer")]
public sealed class DepthTextureRenderer : CustomPostProcessVolumeComponent, IPostProcessComponent
{
    public BoolParameter enableEffect = new BoolParameter(false);

    Material m_Material;

    public bool IsActive() => m_Material != null && enableEffect.value;

    public override CustomPostProcessInjectionPoint injectionPoint => CustomPostProcessInjectionPoint.AfterPostProcess;

    public override void Setup()
    {
        if (Shader.Find("Hidden/Shader/DepthTextureRenderer") != null)
            m_Material = new Material(Shader.Find("Hidden/Shader/DepthTextureRenderer"));
    }

    public override void Render(CommandBuffer cmd, HDCamera camera, RTHandle source, RTHandle destination)
    {
        if (m_Material == null)
            return;

        m_Material.SetTexture("_InputTexture", source);
        HDUtils.DrawFullScreen(cmd, m_Material, destination);
    }

    public override void Cleanup() => CoreUtils.Destroy(m_Material);
}
