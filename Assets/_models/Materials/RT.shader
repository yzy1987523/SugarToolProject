Shader "ImageEffect/RT"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" { }
		_Color("Tint", Color) = (1,1,1,1)
		_StencilComp("Stencil Comparison", Float) = 8
		_Stencil("Stencil ID", Float) = 0
		_StencilOp("Stencil Operation", Float) = 0
		_StencilWriteMask("Stencil Write Mask", Float) = 255
		_StencilReadMask("Stencil Read Mask", Float) = 255
		_ColorMask("Color Mask", Float) = 15//这样这个组件就支持mask的使用了
	}
	SubShader
		   {
				  Pass
		   {
				  Tags{ "Queue" = "Transparent"
				  "IgnoreProjector" = "True"
						"RenderType" = "Transparent"
						"PreviewType" = "Plane"
						"CanUseSpriteAtlas" = "True"}
						Stencil
				  {
						Ref[_Stencil]
						Comp[_StencilComp]
						Pass[_StencilOp]
						ReadMask[_StencilReadMask]
						WriteMask[_StencilWriteMask]
				  }
				  ZTest Always Cull Off ZWrite Off
				  Blend One OneMinusSrcAlpha
				  ColorMask[_ColorMask]
				  CGPROGRAM
	#pragma vertex vert
	#pragma fragment frag
	#pragma target 3.0
	#include "UnityCG.cginc"
				  struct appdata
		   {
				  float4 vertex : POSITION;
				  float4 color : COLOR;
				  float2 uv : TEXCOORD0;
		   };
		   struct v2f
		   {
				  float4 uv : TEXCOORD0;
				  float4 Color : COLOR;
				  float4 vertex : SV_POSITION;
		   };
		   sampler2D _MainTex;
		   float4 _MainTex_ST;
		   fixed4 _Color;
		   v2f vert(appdata v)
		   {
				  v2f o;
				  o.uv.xy = v.uv.xy;
				  o.vertex = UnityObjectToClipPos(v.vertex);
				  o.Color = v.color * _Color;
				  return o;
		   }
		   half4 frag(v2f i) : SV_Target
		   {
				  float4 color = tex2D(_MainTex, i.uv.xy);
				  color.w = step(0.05, color.w);
				  return color * i.Color;
		   }
				  ENDCG
		   }
		   }
			   FallBack "Diffuse"
}