//
//  OutlineFill.shader
//  QuickOutline
//
//  Created by Chris Nolet on 2/21/18.
//  Copyright © 2018 Chris Nolet. All rights reserved.
//

Shader "Custom/Outline Fill" {
  Properties {
    _MainTex("Texture",2D) = "white"{}
    [IntRange] _StencilRef ("Stencil Ref",Range(1,255)) = 0

    [HDR] [MainColor] _OutlineColor("Outline Color", Color) = (1, 1, 1, 1)
    _OutlineWidth("Outline Width", Range(0, 20)) = 2
  }

  SubShader {
    Tags {
      "Queue" = "AlphaTest"
      "RenderType" = "Transparent"
    }
     Pass {

      Name "Mask"
        Cull Off
        ZTest Always
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        Colormask 0

        Stencil {
          Ref [_StencilRef]
          Pass Replace
        }
    }
    Pass {
      Name "Fill"
      Cull Off
      ZWrite On
      ZTest Less
      Blend SrcAlpha OneMinusSrcAlpha

        Stencil{
            Ref[_StencilRef]
            Comp Greater
        }

      CGPROGRAM
      #include "UnityCG.cginc"

      #pragma vertex vert
      #pragma fragment frag

      struct appdata {
        float4 vertex : POSITION;
        float3 normal : NORMAL;
        float3 smoothNormal : TEXCOORD3;
      };

      struct v2f {
        float4 position : SV_POSITION;
        fixed4 color : COLOR;
      };

      inline float invLerp(float a,float b,float v) {
        return (v-a)/(b-a);
      }

      uniform fixed4 _OutlineColor;
      uniform float _OutlineWidth;

      v2f vert(appdata input) {
        v2f output;


        float3 normal = any(input.smoothNormal) ? input.smoothNormal : input.normal;
        float3 viewPosition = UnityObjectToViewPos(input.vertex);
        float3 viewNormal = normalize(mul((float3x3)UNITY_MATRIX_IT_MV, normal));

        float3 worldPos = mul(unity_ObjectToWorld, input.vertex);
        float3 diff = abs(_WorldSpaceCameraPos-worldPos);
        float camDist = (diff.x+diff.y+diff.z)*0.333f;
        camDist = invLerp(1400,200,camDist);
        camDist = clamp(camDist,0.5,1);

        output.position = UnityViewToClipPos(viewPosition + viewNormal * -clamp(viewPosition.z,-7,1) * _OutlineWidth * camDist / 100.0);
        output.color = _OutlineColor;

        return output;
      }

      fixed4 frag(v2f input) : SV_Target {
        return input.color ;
      }
      ENDCG
    }
  }
}
