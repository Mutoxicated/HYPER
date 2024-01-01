Shader "Hidden/Scanlines"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _LineSize ("LineSize",float) = 0.1
        _Modulo ("Modulo",int) = 2
        _Alpha ("Alpha",float) = 0.4
    }
    SubShader
    {
        Tags { "Queue" = "Overlay"}
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
                float4 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            fixed _LineSize;
            fixed _Modulo;
            fixed _Alpha;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed p = i.uv.y / i.uv.w;
                fixed4 col = tex2D(_MainTex, i.uv);
                if((int)(p*_ScreenParams.y/floor(_LineSize))%_Modulo==0) {
                    col *= _Alpha;
                }
                return col;
            }
            ENDCG
        }
    }
}
