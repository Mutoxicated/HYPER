Shader "Hidden/AustralianShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
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
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                #if UNITY_UV_STARTS_AT_TOP
                    o.uv.y = 1-o.uv.y;
                #endif
                return o;
            }

            sampler2D _MainTex;

            fixed4 frag (v2f i) : SV_Target
            {
                float2 modifiedUV = i.uv;
                modifiedUV.y = abs(1-modifiedUV.y);
                fixed4 col = tex2D(_MainTex, modifiedUV);
                
                return col;
            }
            ENDCG
        }
    }
}
