// [[unity-patterns#up13]] — URP Unlit shader with _FlashAmount property for hit flash.
// Drop into Assets/_Project/Shaders/, create a material from it, assign to enemy renderer.
// Drive _FlashAmount from C# via MaterialPropertyBlock (no material instance, no draw-call cost).
//
// Usage (C#):
//   _mpb.SetFloat("_FlashAmount", 1f);       // full white
//   _renderer.SetPropertyBlock(_mpb);
//   // After 70ms (Vlambeer P19):
//   _mpb.SetFloat("_FlashAmount", 0f);
//   _renderer.SetPropertyBlock(_mpb);

Shader "VibeGame/HitFlash"
{
    Properties
    {
        _BaseMap ("Base Texture", 2D) = "white" {}
        _BaseColor ("Base Color", Color) = (1,1,1,1)
        _FlashColor ("Flash Color", Color) = (1,1,1,1)
        _FlashAmount ("Flash Amount", Range(0,1)) = 0
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline"="UniversalPipeline" "Queue"="Geometry" }

        Pass
        {
            Name "Forward"
            Tags { "LightMode"="UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseMap_ST;
                float4 _BaseColor;
                float4 _FlashColor;
                float  _FlashAmount;
            CBUFFER_END

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv         : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv          : TEXCOORD0;
            };

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = TRANSFORM_TEX(IN.uv, _BaseMap);
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                half4 baseTex = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.uv);
                half4 base = baseTex * _BaseColor;
                // Vlambeer P19 — full-surface override on hit
                return lerp(base, _FlashColor, _FlashAmount);
            }
            ENDHLSL
        }
    }

    FallBack "Universal Render Pipeline/Unlit"
}
