Shader "Unlit/ColorAdjust"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Hue("Hue", Range(-360, 360)) = 0.
        _Brightness("Brightness", Range(-1, 1)) = 0.
        _Contrast("Contrast", Range(0, 2)) = 1
        _Saturation("Saturation", Range(0, 2)) = 1
    }
        SubShader
        {
            Tags { "RenderType" = "Opaque" }
            LOD 100
            Pass
            {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"
                struct appdata
                {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                };
                struct v2f
                {
                    float2 uv : TEXCOORD0;
                    float4 vertex : SV_POSITION;
                };
                sampler2D _MainTex;
                float4 _MainTex_ST;
                float _Hue;
                float _Brightness;
                float _Contrast;
                float _Saturation;
                v2f vert(appdata v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                    return o;
                }
                inline float3 applyHue(float3 aColor, float aHue)
                {
                    float angle = radians(aHue);
                    float3 k = float3(0.57735, 0.57735, 0.57735);
                    float cosAngle = cos(angle);
                    //Rodrigues' rotation formula
                    return aColor * cosAngle + cross(k, aColor) * sin(angle) + k * dot(k, aColor) * (1 - cosAngle);
                }
                inline float4 applyHSBEffect(float4 startColor)
                {
                    float4 outputColor = startColor;
                    outputColor.rgb = applyHue(outputColor.rgb, _Hue);
                    outputColor.rgb = (outputColor.rgb - 0.5f) * (_Contrast)+0.5f;
                    outputColor.rgb = outputColor.rgb + _Brightness;
                    float3 intensity = dot(outputColor.rgb, float3(0.299, 0.587, 0.114));
                    outputColor.rgb = lerp(intensity, outputColor.rgb, _Saturation);
                    return outputColor;
                }
                fixed4 frag(v2f i) : SV_Target
                {
                    float4 startColor = tex2D(_MainTex, i.uv);
                    float4 hsbColor = applyHSBEffect(startColor);
                    return hsbColor;
                }
                ENDCG
            }
        }
}