Shader "Custom/WireframeTest"
{
    Properties
    {
        [HDR] [MainColor] _WireframeColor("Wireframe color", color) = (1.0, 1.0, 1.0, 1.0)
        _WireframeAliasing("Wireframe aliasing", float) = 1.5
        _Intact("Intact Range", float) = 0

       [Enum(UnityEngine.Rendering.CullMode)] _Cull ("Cull", Integer) = 2
    }

        SubShader
    {
        Tags {"RenderType" = "Transparent" "Queue" = "Transparent"}
        LOD 100


        Pass {
            Cull [_Cull]
            ZWrite On
            Lighting Off
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma geometry geom

            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 smoothNormal : TEXCOORD3;
                float4 normal : NORMAL;
                float4 color : COLOR;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
            };

            struct g2f {
                float4 pos : SV_POSITION;
                float3 barycentric : TEXCOORD0;
                float4 color : COLOR;
            };

            float random(float2 normal) {
                return frac(sin(dot(normal, float2(12.9898, 78.233))) * 43758.5453123);
            }

            float _Intact;

            v2f vert(appdata v) {
                v2f o;

                float randomNum = random(v.normal)*2;
                float4 thing = v.vertex + v.normal * _Intact*0.6*randomNum;
                o.vertex = thing;
                o.color = v.color;
                o.uv = v.uv;

                return o;
            }

            [maxvertexcount(3)]
            void geom(triangle v2f IN[3], inout TriangleStream<g2f> triStream) {

                g2f o;
                o.pos = UnityObjectToClipPos(IN[0].vertex);
                o.barycentric = IN[0].color;
                o.color = IN[0].color;
                triStream.Append(o);
                o.pos = UnityObjectToClipPos(IN[1].vertex);
                o.color = IN[1].color;
                o.barycentric = IN[1].color;
                triStream.Append(o);
                o.pos = UnityObjectToClipPos(IN[2].vertex);
                o.color = IN[2].color;
                o.barycentric = IN[2].color;
                triStream.Append(o);                
            }

            fixed4 _WireframeColor;
            float _WireframeAliasing;

            fixed4 frag(g2f i) : SV_Target {
                // Calculate the unit width based on triangle size.
                float3 unitWidth = fwidth(i.barycentric);
                // Alias the line a bit.
                float3 aliased = smoothstep(float3(0.0, 0.0, 0.0), unitWidth * _WireframeAliasing, i.barycentric);
                // Use the coordinate closest to the edge.
                float alpha = 1 - min(aliased.x, min(aliased.y, aliased.z));

                return fixed4(_WireframeColor.r, _WireframeColor.g, _WireframeColor.b, _WireframeColor.a*clamp(alpha*abs(_Intact-1),0,1));
            }
            ENDCG
            }
    }
}