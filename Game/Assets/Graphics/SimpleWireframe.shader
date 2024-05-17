// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/QuadWireframe"
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
            ZWrite Off
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
                float camDist : TEXCOORD1;
                float camDistLinear : TEXCOORD2;
            };

            struct g2f {
                float4 pos : SV_POSITION;
                float3 barycentric : TEXCOORD0;
                float camDist : TEXCOORD1;
                float camDistLinear : TEXCOORD2;
            };

            float random(float2 normal) {
                return frac(sin(dot(normal, float2(12.9898, 78.233))) * 43758.5453123);
            }

            float _Intact;

            inline float invLerp(float a,float b,float v) {
                return (v-a)/(b-a);
            }

            v2f vert(appdata v) {
                v2f o;

                float randomNum = random(v.normal)*2;
                float4 thing = v.vertex + v.normal * _Intact*0.6*randomNum;
                o.vertex = thing;
                float3 worldPos = mul(unity_ObjectToWorld, o.vertex);
                float3 diff = abs(_WorldSpaceCameraPos-worldPos);
                float camDist = (diff.x+diff.y+diff.z)*0.333f;
                o.camDist = invLerp(1400,200,camDist);
                o.camDistLinear = o.camDist;
                o.camDist = clamp(o.camDist,0,1);
                o.camDistLinear = clamp(o.camDistLinear, 0,1);
                o.camDist *= o.camDist;
                o.camDist *= o.camDist;

                o.color = v.color;
                o.uv = v.uv;

                return o;
            }

            [maxvertexcount(3)]
            void geom(triangle v2f IN[3], inout TriangleStream<g2f> triStream) {
                float edgeLengthX = length(IN[1].vertex - IN[2].vertex);
                float edgeLengthY = length(IN[0].vertex - IN[2].vertex);
                float edgeLengthZ = length(IN[0].vertex - IN[1].vertex);
                float3 modifier = float3(0.0, 0.0, 0.0);
                
                if ((edgeLengthX > edgeLengthY) && (edgeLengthX > edgeLengthZ)) {
                    modifier = float3(1.0, 0.0, 0.0);
                }
                else if ((edgeLengthY > edgeLengthX) && (edgeLengthY > edgeLengthZ)) {
                    modifier = float3(0.0, 1.0, 0.0);
                }
                else if ((edgeLengthZ > edgeLengthX) && (edgeLengthZ > edgeLengthY)) {
                    modifier = float3(0.0, 0.0, 1.0);
                }

                g2f o;
                o.pos = UnityObjectToClipPos(IN[0].vertex);
                o.barycentric = float3(1.0, 0.0, 0.0) + modifier;
                o.camDist = IN[0].camDist;
                o.camDistLinear = IN[0].camDistLinear;
                triStream.Append(o);
                o.pos = UnityObjectToClipPos(IN[1].vertex);
                o.barycentric = float3(0.0, 1.0, 0.0) + modifier;
                o.camDist = IN[1].camDist;
                o.camDistLinear = IN[1].camDistLinear;
                triStream.Append(o);
                o.pos = UnityObjectToClipPos(IN[2].vertex);
                o.barycentric = float3(0.0, 0.0, 1.0) + modifier;
                o.camDist = IN[2].camDist;
                o.camDistLinear = IN[2].camDistLinear;
                triStream.Append(o);

            }

            fixed4 _WireframeColor;
            float _WireframeAliasing;

            fixed4 frag(g2f i) : SV_Target {
                // Calculate the unit width based on triangle size.
                float3 unitWidth = fwidth(i.barycentric);
                // Alias the line a bit.
                float3 aliased = smoothstep(float3(0.0, 0.0, 0.0), unitWidth * _WireframeAliasing * i.camDistLinear, i.barycentric);
                // Use the coordinate closest to the edge.
                float alpha = 1 - min(aliased.x, min(aliased.y, aliased.z));

                return fixed4(_WireframeColor.r, _WireframeColor.g, _WireframeColor.b, _WireframeColor.a*clamp(alpha*abs(_Intact-1),0,1)* i.camDist);
            }
            ENDCG
        }
    }
}