Shader "LibraryGame/UIBackgroundBlur"
{
    Properties
    {
        _BlurRadius ("Blur Radius (render pixels)", Range(0, 16)) = 4
        _Intensity ("Blur Intensity", Range(0, 1)) = 1
    }
    SubShader
    {
        Tags { "RenderPipeline" = "UniversalPipeline" }
        Pass
        {
            Name "UIBackgroundBlur"
            ZWrite Off
            ZTest Always
            Cull Off
            Blend Off

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"

            CBUFFER_START(UnityPerMaterial)
                float _BlurRadius;
                float _Intensity;
            CBUFFER_END

            half4 Frag(Varyings input) : SV_Target
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
                float2 uv = input.texcoord;
                half4 original = SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_LinearClamp, uv);
                if (_BlurRadius <= 0.001 || _Intensity <= 0.001)
                    return original;

                float2 stepUV = _BlitTexture_TexelSize.xy * (_BlurRadius / 4.0);
                float3 sum = 0;
                float totalWeight = 0;
                [loop]
                for (int y = -4; y <= 4; y++)
                {
                    [loop]
                    for (int x = -4; x <= 4; x++)
                    {
                        float2 offset = float2(x, y);
                        float weight = exp(-dot(offset, offset) / 8.0);
                        sum += SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_LinearClamp,
                            uv + offset * stepUV).rgb * weight;
                        totalWeight += weight;
                    }
                }
                return half4(lerp(original.rgb, sum / totalWeight, saturate(_Intensity)), original.a);
            }
            ENDHLSL
        }
    }
}
