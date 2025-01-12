Shader "Hidden/Shader/DepthTextureRenderer"
{
    HLSLINCLUDE

    #pragma target 4.5
    #pragma only_renderers d3d11 vulkan metal

    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
    #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
    #include "Packages/com.unity.render-pipelines.high-definition/Runtime/PostProcessing/Shaders/FXAA.hlsl"
    #include "Packages/com.unity.render-pipelines.high-definition/Runtime/PostProcessing/Shaders/RTUpscale.hlsl"

    struct Attributes
    {
        uint vertexID : SV_VertexID;
        // UNITY_VERTEX_INPUT_INSTANCE_ID
    };

    struct Varyings
    {
        float4 positionCS : SV_POSITION; // clip space position
        float2 texcoord   : TEXCOORD0; // Texture coordinates
    };

    Varyings Vert(Attributes input)
    {
        Varyings output;
        // UNITY_SETUP_INSTANCE_ID(input);
        output.positionCS = GetFullScreenTriangleVertexPosition(input.vertexID);
        output.texcoord = GetFullScreenTriangleTexCoord(input.vertexID);
        return output;
    }

    float4 CustomPostProcess(Varyings input) : SV_Target
    {
        float2 uv = input.texcoord;
        float depth = LoadCameraDepth(input.positionCS.xy);
        float linearDepth = LinearEyeDepth(depth, _ZBufferParams);
        
        // Normalize the depth value
        float normalizedDepth = saturate(linearDepth / _ProjectionParams.z);
        
        // Output the grayscale depth
        return float4(normalizedDepth.xxx, 1);
    }

    ENDHLSL

    SubShader
    {
        Pass
        {
            Name "DepthTextureRenderer"

            ZWrite Off
            ZTest Always
            Blend Off
            Cull Off

            HLSLPROGRAM
                #pragma vertex Vert
                #pragma fragment CustomPostProcess
            ENDHLSL
        }
    }
    Fallback Off
}
