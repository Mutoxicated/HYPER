Shader "Custom/particlePixelation"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        [HDR] _Color("Color", color) = (1,1,1,1)
        _PixelSize("Pixel Size", Float) = 10
    }

        SubShader
        {
            Tags{ "Queue" = "Transparent+1000" }
            LOD 200
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Back

            Pass{
                CGPROGRAM

                #include "UnityCG.cginc"

                #pragma vertex vert
                #pragma fragment frag

                sampler2D _MainTex;

                struct appdata {
                    float4 vertex : POSITION;
                    float4 uv : TEXCOORD1;
                    fixed4 custom1 : TEXCOORD0;
                };

                struct v2f {
                    float4 position : SV_POSITION;
                    float4 uv : TEXCOORD0;
                    fixed4 color : TEXCOORD1;
                };

                v2f vert(appdata v) {
                    v2f o;
                    o.position = UnityObjectToClipPos(v.vertex);
                    o.uv = ComputeScreenPos(v.vertex);
                    o.color = v.custom1;
                    return o;
                }

                fixed4 frag(v2f i) : SV_TARGET{
                    return i.color;
                }

                ENDCG
            }
            GrabPass{ "_GrabTransparentTexture" }
            ZWrite Off
            Cull Back
            Blend Off
            Pass
            {
                CGPROGRAM

                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"

                struct appdata {
                    float3 pos : POSITION;
                    float4 uv : TEXCOORD1;
                    fixed4 custom1 : TEXCOORD0;
                    float4 center : TEXCOORD2;
                };

                struct v2f
                {
                    float4 pos : SV_POSITION;
                    float4 uv : TEXCOORD1;
                    fixed4 custom1 : TEXCOORD0;
                    float4 center : TEXCOORD2;
                    float viewZ : TEXCOORD3;
                };

                v2f vert(appdata v)
                {
                    v2f o;
                    o.center = v.center;
                    
                    float3 vert = v.pos - v.center.xyz;

                    vert *= 2;

                    v.pos = vert + v.center.xyz;

                    o.viewZ = UnityObjectToViewPos(v.pos).z;

                    o.pos = UnityObjectToClipPos(v.pos);
                    o.uv = ComputeScreenPos(o.pos);
                    o.custom1 = v.custom1;
                    return o;
                }

                inline bool compareColors(float4 color1, float4 color2, float tolerance){
                    float4 endcolor;
                    endcolor.r = abs(color1.r-color2.r);
                    endcolor.g = abs(color1.g-color2.g);
                    endcolor.b = abs(color1.b-color2.b);

                    float average = (endcolor.r+endcolor.g+endcolor.b)/3;
                    return (average < tolerance) ? true : false;
                }

                sampler2D _MainTex;
                Texture2D _GrabTransparentTexture;
                SamplerState point_clamp_sampler;

                float4 frag(v2f IN) : SV_Target
                {
                    float2 steppedUV = IN.uv.xy/IN.uv.w;
                    fixed4 beforeColor = _GrabTransparentTexture.Sample(point_clamp_sampler, steppedUV);
                    float thing = (IN.center.w) / _ScreenParams.xy / _ScreenParams.w;// + clamp(IN.viewZ,-1,1);
                    steppedUV /= thing;
                    steppedUV = round(steppedUV);
                    steppedUV *= thing;
                    fixed4 col = _GrabTransparentTexture.Sample(point_clamp_sampler, steppedUV);
                    fixed4 finalColor;// = (distance(_Color, col) < 1.3) ? _Color : col;
                    if (compareColors(IN.custom1, col, 0.01)) {
                        finalColor = IN.custom1;
                    }
                    else {
                        finalColor = (compareColors(IN.custom1, beforeColor, 0.01)) ? col : beforeColor;
                    }
                    //finalColor = (distance(beforeColor, col) > 0.7) ? col : beforeColor;
                    return finalColor;
                }

                ENDCG
            }
        }
}