// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/SurfaceEffect"
{
    Properties
    {
        _MainTex("MainTexture", 2D) = "black" {}
        [HDR] _MainColor("MainColor",Color) = (1,1,1,1)
        
        _NoiseTexture1("NoiseTexture1",2D) = "white" {}
        _NoiseTexture2("NoiseTexture2",2D) = "white" {}

        _Hue("Hue", Range(-360, 360)) = 0.
        _Brightness("Brightness", Range(-1, 1)) = 0.
        _Contrast("Contrast", Range(0, 2)) = 1
        _Saturation("Saturation", Range(0, 2)) = 1

        _ClampAlphaValue("ClampAlpha", Range(0,1)) = 1
        _Texture1AlphaMod("Texture1AlphaMod",Range(0,2)) = 1
        _Texture2AlphaMod("Texture2AlphaMod",Range(0,6)) = 1

        _Extension("Extension", Range(1,2)) = 1.2
    }
        SubShader
        {
            Tags { "RenderType" = "Transparent" "Queue" = "Transparent+30" }
            LOD 100
            Pass
            {
                Blend SrcAlpha OneMinusSrcAlpha
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"
                struct appdata
                {
                    float3 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                    float3 normal : NORMAL;
                    float3 smoothNormal : TEXCOORD3;
                };
                struct v2f
                {
                    float2 uv : TEXCOORD0;
                    float2 uv2 : TEXCOORD1;
                    float2 uv3 : TEXCOORD2;
                    float4 vertex : SV_POSITION;
                };
                sampler2D _MainTex;
                float4 _MainTex_ST;
                sampler2D _NoiseTexture1;
                float4 _NoiseTexture1_ST;
                sampler2D _NoiseTexture2;
                float4 _NoiseTexture2_ST;
                float4 _MainColor;
                float _Hue;
                float _Brightness;
                float _Contrast;
                float _Saturation;
                float _ClampAlphaValue;
                float _Extension;
                float _Texture1AlphaMod;
                float _Texture2AlphaMod;

                v2f vert(appdata v)
                {
                    v2f o;
                    float4 center = float4(0,0,0,0);
                    
                    float3 vert = v.vertex - center.xyz;

                    vert *= _Extension;

                    v.vertex = vert + center.xyz;

                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                    o.uv2 = TRANSFORM_TEX(v.uv,_NoiseTexture1);          
                    o.uv3 = TRANSFORM_TEX(v.uv,_NoiseTexture2);
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
                    startColor.rgb = _MainColor.rgb;
                    float4 hsbColor = applyHSBEffect(startColor);
                    float alpha = tex2D(_NoiseTexture1,i.uv2);
                    alpha = abs(1-alpha);
                    alpha *= _Texture1AlphaMod;
                    alpha = clamp(alpha,0,1);
                    alpha = abs(1-alpha);
                    hsbColor.a = alpha*_ClampAlphaValue;
                    float nAlpha = tex2D(_NoiseTexture2,i.uv3);
                    nAlpha *= _Texture2AlphaMod;
                    nAlpha = clamp(nAlpha,0,1);
                    nAlpha = abs(1-nAlpha);
                    hsbColor.a *= nAlpha;
                    return hsbColor;
                }
                ENDCG
            }
        }
}