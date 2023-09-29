//UNITY_SHADER_NO_UPGRADE

Shader "Custom/PixelateShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_PixelSize ("Pixel Size", Range(1,1000)) = 10
	}
	SubShader
	{
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct vertIn
			{
				float4 vertex : POSITION;
				float4 uv : TEXCOORD0;
			};

			struct vertOut
			{
				float4 vertex : SV_POSITION;
				float4 uv : TEXCOORD0;
			};

			uniform float _PixelSize;
			uniform sampler2D _MainTex;

			// Implementation of the vertex shader
			vertOut vert(vertIn v)
			{
				vertOut o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			// Implementation of the fragment (pixel) shader
			fixed4 frag(vertOut v) : SV_Target
			{
				float2 pixelatedUV = floor(v.uv * _PixelSize) / _PixelSize;
				fixed4 texColor = tex2D(_MainTex, pixelatedUV);
				return texColor;
			}
			ENDCG
		}
	}
}
