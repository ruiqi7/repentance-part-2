Shader "Custom/Outline"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _OutlineThickness ("Thickness", Float) = 1.0
        _OutlineColor ("Color", Color) = (1,1,1,1)
        _Cutoff("Alpha cutoff", Range(0,1)) = 0.5
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
        Tags { "RenderType"="Opaque" }
        LOD 100
            Stencil
			{
				Ref 4
				Comp always
				Pass replace
				ZFail keep
			}

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"
            #include "UnityPBSLighting.cginc"
            #include "AutoLight.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float4 worldVertex : TEXCOORD2;
                float3 worldNormal : TEXCOORD3;
            };

            sampler2D _MainTex;
            float _Cutoff;
            float _OutlineThickness;
            float4 _MainTex_ST;
            uniform float _Ka;
            uniform float _Kd;
            uniform float _Ks;
            uniform float _fAtt;
            uniform float _specN;

            v2f vert (appdata v)
            {
                v2f o;

                float4 worldVertex = mul(unity_ObjectToWorld, v.vertex);
                float3 worldNormal = normalize(mul(transpose((float3x3)unity_WorldToObject), v.normal.xyz));


                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f v) : SV_Target
            {
                fixed4 unlitColor = tex2D(_MainTex, v.uv);
				clip(unlitColor.a - _Cutoff);
				float3 interpNormal = normalize(v.worldNormal);

				// Calculate ambient RGB intensities
				float Ka = _Ka;
				#if defined(SPOT)
					Ka = 0;
				#endif
				float3 amb = unlitColor.rgb * UNITY_LIGHTMODEL_AMBIENT.rgb * Ka;

				#if defined(SPOT) 
					float3 V = normalize(_WorldSpaceCameraPos - v.worldVertex.xyz);
				#else
					float3 V = normalize(_WorldSpaceCameraPos - v.worldVertex.xyz);
				#endif

				// Calculate diffuse RBG reflections, we save the results of L.N because we will use it again
				// (when calculating the reflected ray in our specular component)
				#if defined(SPOT)
					UNITY_LIGHT_ATTENUATION(fAtt, 0, v.worldVertex.xyz);
				#else
					float fAtt = _fAtt;
				#endif
				float Kd = _Kd;

				#if defined(SPOT)
					float3 L = normalize(_WorldSpaceLightPos0 - v.worldVertex.xyz);
				#else
					float3 L = _WorldSpaceLightPos0;
				#endif
				float LdotN = dot(L, interpNormal);
				float3 dif = fAtt * _LightColor0 * Kd * unlitColor.rgb * saturate(LdotN);

				// Calculate specular reflections
				float Ks = _Ks;
				//float specN = _specN; // Values>>1 give tighter highlights
				

				// Using Blinn-Phong approximation:
				float specN = _specN; // We usually need a higher specular power when using Blinn-Phong
				float3 H = normalize(V + L);
				float3 spe = fAtt * _LightColor0 * Ks * pow(saturate(dot(interpNormal, H)), specN);

				// Combine Phong illumination model components
				float4 returnColor = float4(0.0f, 0.0f, 0.0f, 0.0f);
				returnColor.rgb = amb.rgb + dif.rgb + spe.rgb;
				returnColor.a = unlitColor.a;
				
				UNITY_APPLY_FOG(v.fogCoord, returnColor);
				
				return returnColor;
            }
            ENDCG
            }
            Pass
		{
			// Won't draw where it sees ref value 4
			Cull OFF
			ZWrite OFF
			ZTest ON
			Stencil
			{
				Ref 4
				Comp notequal
				Fail keep
				Pass replace
			}

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			// Properties
			uniform float4 _OutlineColor;
			uniform float _OutlineThickness;

			struct vertexInput
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
			};

			struct vertexOutput
			{
				float4 pos : SV_POSITION;
				float4 color : COLOR;
			};

			vertexOutput vert(vertexInput input)
			{
				vertexOutput output;

				float4 newPos = input.vertex;

				// Setup Ouline Vertices multiply current vertex by a scale outline thickness by mesh normal
				float3 normal = normalize(input.normal);
				newPos += float4(normal, 0.0) * _OutlineThickness;

				// convert to world space
				output.pos = UnityObjectToClipPos(newPos);

				output.color = _OutlineColor;
				return output;
			}

			fixed4 frag (vertexOutput v) : SV_Target
			{
				// checker value will be negative for 4x4 blocks of pixels
				// in a checkerboard pattern
				//input.pos.xy = floor(input.pos.xy * _OutlineDot) * 0.5;
				//float checker = -frac(input.pos.r + input.pos.g);

				// clip HLSL instruction stops rendering a pixel if value is negative
				//clip(checker);

				return v.color;
			}

			ENDCG
		}
    }
}

