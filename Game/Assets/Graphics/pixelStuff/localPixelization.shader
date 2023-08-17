// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

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
        Tags{ "Queue" = "AlphaTest" "IgnoreProjector" = "True" }
        LOD 200
        Lighting Off
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite On
        Cull Back

        Pass{
            ZTest Less
            CGPROGRAM

            #include "UnityCG.cginc"

            #pragma vertex vert
            #pragma fragment frag

            sampler2D _MainTex;
            float4 _MainTex_ST;

            fixed4 _Color;

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                fixed4 color : COLOR;
            };

            struct v2f {
                float4 position : SV_POSITION;
                float2 uv : TEXCOORD0;
                fixed4 color : COLOR;
            };

            v2f vert(appdata v) {
                v2f o;
                o.position = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color;
                return o;
            }

            fixed4 frag(v2f i) : SV_TARGET{
                fixed4 col = tex2D(_MainTex, i.uv);
                col *= _Color;
                return _Color;
            }

            ENDCG
        }
        ZWrite Off
        Cull Off
        Blend Off
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
                fixed4 color : COLOR;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float4 uv : TEXCOORD0;
                fixed4 color : COLOR;
            };

            float _PixelSize;

            v2f vert(appdata v)
            {
                v2f o;

                v.pos *= 1.1;

                o.pos = UnityObjectToClipPos(v.pos);
                o.pos = UnityPixelSnap(o.pos);
                o.uv = ComputeScreenPos(o.pos);
                o.color = v.color;
                return o;
            }

            sampler2D _MainTex;
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
                if (distance(_Color, col) < 0.1) {
                    finalColor = _Color;
                }
                else {
                    finalColor = (distance(_Color,beforeColor) < 0.1) ? col : beforeColor;
                }
                //finalColor = (distance(beforeColor, col) > 0.7) ? col : beforeColor;
                return finalColor;
            }

            ENDCG
        }
    }
}