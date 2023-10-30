Shader "Custom/Letter"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _DissolveTexture ("Dissolve Texture", 2D) = "white" {}
        _Amount ("Amount", Range(0,1)) = 0
        _Color ("Color", Color) = (1,1,1,1)

        _Ka("Ka", Float) = 1.0
		_Kd("Kd", Float) = 1.0
		_Ks("Ks", Float) = 1.0
		_fAtt("fAtt", Float) = 1.0
		_specN("specN", Float) = 1.0

        _BurnSize("Burn Size", range(0.0, 1.0)) = 0.15
        _BurnMap("Burn Map (RGB)", 2D) = "white" {}
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #if !defined(DISSOLVE_INCLUDED)
			#define DISSOLVE_INCLUDED
			#include "UnityPBSLighting.cginc"
			#include "AutoLight.cginc"
			#endif
			
			#include "Dissolve.cginc"

            ENDCG
        }
    }
}
