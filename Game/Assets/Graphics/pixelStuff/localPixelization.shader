Shader "Custom/localPixelization"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        [HDR] _Color("Color", color) = (1,1,1,1)
        _PixelSize("Pixel Size", Float) = 10
    }

        SubShader
    {
        Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" }
        Blend Off
        Lighting Off
        Fog{ Mode Off }
        ZWrite Off
        LOD 200
        Cull Off

        GrabPass{ "_GrabTexture" }

        Pass
        {
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata {
                float4 pos : POSITION;
                float3 normal : NORMAL;
                float3 smoothNormal : TEXCOORD3;
                float4 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float4 uv : TEXCOORD0;
            };

            float _PixelSize;

            v2f vert(appdata v)
            {
                v2f o;

                float3 normal = any(v.smoothNormal) ? v.smoothNormal : v.normal;
                float3 viewPosition = UnityObjectToViewPos(v.pos);
                float3 viewNormal = normalize(mul((float3x3)UNITY_MATRIX_IT_MV, normal));

                o.pos = UnityViewToClipPos(viewPosition + viewNormal * _PixelSize*0.75 / 100.0);
                o.pos = UnityPixelSnap(o.pos);
                o.uv = ComputeScreenPos(o.pos);
                return o;
            }

            sampler2D _GrabTexture;
            fixed4 _Color;

            float4 frag(v2f IN) : SV_Target
            {
                float2 steppedUV = IN.uv.xy / IN.uv.w;
                float thing = _PixelSize / _ScreenParams.xy;
                steppedUV /= thing;
                steppedUV = round(steppedUV);
                steppedUV *= thing;
                fixed4 col = tex2D(_GrabTexture, steppedUV);
                fixed check = _Color.rgb - col.rgb;
                if (check < 0.9)
                {
                    col = _Color;
                    return col;
                }
                return col;
            }

            ENDCG
        }
    }
}