Shader "Custom/QuadWireframe"
{
    Properties
    {
        [MainTexture] _BaseMap("Base Map (RGB) Smoothness / Alpha (A)", 2D) = "white" {}

        [HDR] [MainColor] _WireframeBackColour("Wireframe back colour", color) = (1.0, 1.0, 1.0, 1.0)
        _WireframeAliasing("Wireframe aliasing", float) = 1.5
        _Intact("Intact Range", float) = 0
    }

        SubShader
    {
        Tags {"RenderType" = "Transparent" "Queue" = "AlphaTest"}
        LOD 100


        Pass {
            Cull Back
            ZWrite On
            Lighting Off
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma geometry geom

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 smoothNormal : TEXCOORD3;
                float4 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            // We add our barycentric variables to the geometry struct.
            struct g2f {
                float4 pos : SV_POSITION;
                float3 barycentric : TEXCOORD0;
            };

            float random(float2 normal) {
                return frac(sin(dot(normal, float2(12.9898, 78.233))) * 43758.5453123);
            }

            float _Intact;
            sampler2D _BaseMap;
            float4 _BaseMap_ST;

            v2f vert(appdata v)
            {
                v2f o;
                // We push the conversion to ClipPos into the geom function as we need 
                // the mesh vertex values for the edge culling.
                //o.vertex = UnityObjectToClipPos(v.vertex);
                float randomNum = random(v.normal)*2;
                float4 thing = v.vertex + v.normal * _Intact*0.6*randomNum;
                o.vertex = thing;

                o.uv = TRANSFORM_TEX(v.uv, _BaseMap);
                UNITY_TRANSFER_FOG(o, o.vertex);
                return o;
            }

            // This applies the barycentric coordinates to each vertex in a triangle.
            [maxvertexcount(3)]
            void geom(triangle v2f IN[3], inout TriangleStream<g2f> triStream) {
                float edgeLengthX = length(IN[1].vertex - IN[2].vertex);
                float edgeLengthY = length(IN[0].vertex - IN[2].vertex);
                float edgeLengthZ = length(IN[0].vertex - IN[1].vertex);
                float3 modifier = float3(0.0, 0.0, 0.0);
                // We're fine using if statments it's a trivial function.
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
                triStream.Append(o);
                o.pos = UnityObjectToClipPos(IN[1].vertex);
                o.barycentric = float3(0.0, 1.0, 0.0) + modifier;
                triStream.Append(o);
                o.pos = UnityObjectToClipPos(IN[2].vertex);
                o.barycentric = float3(0.0, 0.0, 1.0) + modifier;
                triStream.Append(o);
            }

            fixed4 _WireframeBackColour;
            float _WireframeAliasing;

            fixed4 frag(g2f i) : SV_Target
            {
                // Calculate the unit width based on triangle size.
                float3 unitWidth = fwidth(i.barycentric);
                // Alias the line a bit.
                float3 aliased = smoothstep(float3(0.0, 0.0, 0.0), unitWidth * _WireframeAliasing, i.barycentric);
                // Use the coordinate closest to the edge.
                float alpha = 1 - min(aliased.x, min(aliased.y, aliased.z));
                clip(alpha-0.1);
                // Set to our backwards facing wireframe colour.
                return fixed4(_WireframeBackColour.r, _WireframeBackColour.g, _WireframeBackColour.b, clamp(alpha*abs(_Intact-1),0,1));
            }
            ENDCG
            }
    }
    Fallback "Diffuse"
}