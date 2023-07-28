Shader "Custom/PixelArtFilter" {
    Properties{
        _MainTex("Main", 2D) = "white" {}
        _TexComp("DepthCompare", 2D) = "white" {}
        _PixelSize("Pixel Size", Float) = 10
    }

        SubShader{

            Pass {
                Blend SrcAlpha OneMinusSrcAlpha
                
                CGPROGRAM
                #pragma vertex vp
                #pragma fragment fp

                #include "UnityCG.cginc"

                struct VertexData {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                };

                struct v2f {
                    float2 uv : TEXCOORD0;
                    float4 vertex : SV_POSITION;
                };

                v2f vp(VertexData v) {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = v.uv;
                    return o;
                }

                sampler2D _MainTex;
                sampler2D _TexComp;
                float _PixelSize;

                fixed4 fp(v2f i) : COLOR{
                    fixed4 col1 = tex2D(_MainTex, i.uv);
                    fixed4 col2 = tex2D(_TexComp, i.uv);
                    float comp = col2.rgb - col1.rgb;
                    if (col1.r != col2.r || col1.g != col2.g|| col1.b != col2.b) {

                        float2 steppedUV = i.uv.xy;
                        steppedUV /= _PixelSize / _ScreenParams.xy;
                        steppedUV = round(steppedUV);
                        steppedUV *= _PixelSize / _ScreenParams.xy;
                        return tex2D(_MainTex, steppedUV);
                    }
                    return fixed4(0, 0, 0, 1);
                }
                ENDCG
            }
    }
}