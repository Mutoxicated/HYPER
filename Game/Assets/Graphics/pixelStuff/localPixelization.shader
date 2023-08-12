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

                o.pos = UnityViewToClipPos(viewPosition + viewNormal * _PixelSize / 100.0);
                o.pos = UnityPixelSnap(o.pos);
                o.uv = ComputeScreenPos(o.pos);
                return o;
            }

            Texture2D _GrabTexture;
            SamplerState point_clamp_sampler;
            fixed4 _Color;

            float4 frag(v2f IN) : SV_Target
            {
                float2 steppedUV = IN.uv.xy / IN.uv.w;
                fixed4 beforeColor = _GrabTexture.Sample(point_clamp_sampler, steppedUV);
                float thing = _PixelSize / _ScreenParams.xy / _ScreenParams.w;
                steppedUV /= thing;
                steppedUV = round(steppedUV);
                steppedUV *= thing;
                fixed4 col = _GrabTexture.Sample(point_clamp_sampler, steppedUV);
                fixed4 finalColor;// = (distance(_Color, col) < 1.3) ? _Color : col;
                if (distance(_Color, col) < 1.1) {
                    finalColor = _Color;
                }
                else {
                    finalColor = (distance(beforeColor, _Color) < 1.5) ? col : beforeColor;
                }
                //finalColor = (distance(beforeColor, col) > 0.7) ? col : beforeColor;
                return finalColor;
            }

            ENDCG
        }
    }
}