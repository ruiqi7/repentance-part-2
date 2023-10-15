Shader "Custom/NPC"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _DissolveColor ("Dissolve Color", Color) = (1,1,1,1)
        _DissolveTexture ("Dissolve Texture", 2D) = "white" {}
        _Amount ("Amount", Range(0,1)) = 0
        _Color ("Color", Color) = (1,1,1,1)

        _Ka("Ka", Float) = 1.0
		_Kd("Kd", Float) = 1.0
		_Ks("Ks", Float) = 1.0
		_fAtt("fAtt", Float) = 1.0
		_specN("specN", Float) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "LightMode"="ForwardBase" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma multi_compile DIRECITONAL SPOT
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #if !defined(NPC_INCLUDED)
			#define NPC_INCLUDED
			#include "UnityPBSLighting.cginc"
			#include "AutoLight.cginc"
			#endif
			
			#include "dissolve.cginc"

            ENDCG
        }

        Pass{
            Tags {"RenderType"="Opaque" "LightMode"="ForwardAdd" }
            Blend One One
            CGPROGRAM

            #pragma multi_compile DIRECITONAL SPOT
			#pragma vertex vert
			#pragma fragment frag

			#include "dissolve.cginc"

            ENDCG
        }
    }
}