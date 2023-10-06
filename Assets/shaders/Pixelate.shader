//UNITY_SHADER_NO_UPGRADE..

Shader "Custom/PixelateShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_PixelSize ("Pixel Size", Range(1,200)) = 10
		_Ka("Ka", Float) = 1.0
		_Kd("Kd", Float) = 1.0
		_Ks("Ks", Float) = 1.0
		_fAtt("fAtt", Float) = 1.0
		_specN("specN", Float) = 1.0
	}
	SubShader
	{
		Pass
		{
			Tags 
			{
				"LightMode" = "ForwardBase"
			}
			CGPROGRAM

			#pragma multi_compile DIRECITONAL SPOT
			#pragma vertex vert
			#pragma fragment frag

			#if !defined(PIXELATED_INCLUDED)
			#define PIXELATED_INCLUDED
			#include "UnityPBSLighting.cginc"
			#include "AutoLight.cginc"
			#endif
			
			#include "PixelateBase.cginc"
			
			ENDCG
		}

		Pass 
		{
			Tags 
			{
				"LightMode"="ForwardAdd"
			}
			Blend One One

			CGPROGRAM

			#pragma multi_compile DIRECITONAL SPOT
			#pragma vertex vert
			#pragma fragment frag

			#include "PixelateBase.cginc"

			ENDCG
		}

	}
}