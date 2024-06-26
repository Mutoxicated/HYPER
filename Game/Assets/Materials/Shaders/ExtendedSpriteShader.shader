Shader "Custom/SpriteExtended"{
	Properties{
		[HDR] [MainColor] _Color("Tint", Color) = (0, 0, 0, 1)
		_MainTex("Texture", 2D) = "white" {}
		_Extension("Extension", Float) = 1.2
	}

		SubShader{
			Tags{
				"RenderType" = "Transparent"
				"Queue" = "AlphaTest"
			}
		    LOD 100
			
			Pass{
				Blend SrcAlpha OneMinusSrcAlpha
				ZWrite On
				Cull Back

				CGPROGRAM

				#include "UnityCG.cginc"

				#pragma vertex vert
				#pragma fragment frag

				sampler2D _MainTex;
				float4 _MainTex_ST;

				fixed4 _Color;
				float _Extension;

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
					v.vertex *= _Extension*0.9f;
					o.position = UnityObjectToClipPos(v.vertex);
					o.uv = TRANSFORM_TEX(v.uv, _MainTex);
					o.color = v.color;
					return o;
				}

				fixed4 frag(v2f i) : SV_TARGET{
					fixed4 col = tex2D(_MainTex, i.uv);
					col *= i.color;
					col *= _Color;
					return col;
				}

				ENDCG
			}
	}
}