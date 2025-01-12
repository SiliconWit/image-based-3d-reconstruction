Shader "Hidden/Shader/EuclideanDepthRenderer"
{
    HLSLINCLUDE

    #pragma target 4.5
    #pragma only_renderers d3d11 vulkan metal

    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
    #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"

    struct Attributes
    {
        uint vertexID : SV_VertexID;
        UNITY_VERTEX_INPUT_INSTANCE_ID
    }; 

    struct Varyings
    {
        float4 positionCS : SV_POSITION;
        float2 texcoord   : TEXCOORD0;
    };

    Varyings Vert(Attributes input)
    {
        Varyings output;
        UNITY_SETUP_INSTANCE_ID(input);
        output.positionCS = GetFullScreenTriangleVertexPosition(input.vertexID);
        output.texcoord = GetFullScreenTriangleTexCoord(input.vertexID);
        return output;
    }

    float4 CustomPostProcess(Varyings input) : SV_Target
    {
        float2 uv = input.texcoord;
        float depth = LoadCameraDepth(input.positionCS.xy);
        
        // Convert depth to world space position
        float3 worldPos = ComputeWorldSpacePosition(uv, depth, UNITY_MATRIX_I_VP);
        
        // Calculate distance from camera to world position
        float3 cameraPos = GetCurrentViewPosition();
        float distance = length(worldPos - cameraPos);
        
        // Normalize the distance (you may need to adjust this based on your scene scale)
        float normalizedDistance = saturate(distance / _ProjectionParams.z);
        
        // Output the Euclidean distance
        return float4(normalizedDistance, 0, 0, 1);
    }

    ENDHLSL

    SubShader
    {
        Pass
        {
            Name "EuclideanDepthRenderer"

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
