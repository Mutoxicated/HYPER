Shader "Hidden/ChimeraEffect"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Saturation("Saturation", Range(0, 2)) = 1

        _NoiseTex1 ("NoiseTex1", 2D) = "white" {}
        _NoiseTex2("NoiseTex2", 2D) = "white" {}

        _EffectT ("EffectT", Range(0, 1)) = 1
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

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
                float2 uv2 : TEXCOORD1;
                float2 uv3 : TEXCOORD2;
                float4 vertex : SV_POSITION;
            };

            float _Saturation;
            sampler2D _MainTex;
            float4 _MainTex_ST;

            sampler2D _NoiseTex1;
            float4 _NoiseTex1_ST;

            sampler2D _NoiseTex2;
            float4 _NoiseTex2_ST;

            float _EffectT;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv.xy, _MainTex);
                o.uv2 = TRANSFORM_TEX(v.uv.xy, _NoiseTex1);
                o.uv3 = TRANSFORM_TEX(v.uv.xy, _NoiseTex2);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 noiseCol1 = tex2D(_NoiseTex1,i.uv2);
                noiseCol1.rgb = clamp(noiseCol1.rgb+float3(_EffectT, _EffectT, _EffectT), 0, 1);
                fixed4 noiseCol2 = tex2D(_NoiseTex2, i.uv3);
                noiseCol2.rgb = clamp(noiseCol2.rgb + float3(_EffectT, _EffectT, _EffectT), 0, 1);
                fixed4 col = tex2D(_MainTex, i.uv);
                float3 intensity = dot(col.rgb, float3(0.299, 0.587, 0.114));
                col.rgb = lerp(intensity, col.rgb, _Saturation);
                col *= noiseCol1;
                col *= noiseCol2;
                return col;
            }
            ENDCG
        }
    }
}
