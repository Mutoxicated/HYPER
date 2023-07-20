Shader "Hidden/TextureColor"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ColorRampTex ("Color Ramp Texture",2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType" = "Background"}

        Cull Off
        ZWrite On

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
                return o;
            }

            sampler2D _MainTex;
            sampler2D _ColorRampTex;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                fixed4 finalcol = tex2D(_ColorRampTex, clamp(lerp(0, 7, col.g),0.1,0.95));
                return finalcol;
            }
            ENDCG
        }
    }
}
