Shader "Custom/ConflateShader"
{
    Properties
    {
        _Tex ("Texture", 2D) = "white" {}
        
    }
    SubShader
    {
        // No culling or depth
        Cull Back
        ZWrite On
        ZTest LEqual
        Blend SrcAlpha OneMinusSrcAlpha

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

            sampler2D _Tex;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_Tex, i.uv);
                if (col.r == 0 && col.g == 0 && col.b == 0){
                    col.a = 0;
                }
                return col;
            }
            ENDCG
        }
    }
}
