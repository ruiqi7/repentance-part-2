Shader "Custom/Tree"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Color("Color", Color) = (1, 1, 1, 1)
		_Cutoff("Alpha cutoff", Range(0,1)) = 0.5
        _WaveSpeed("Wave Speed", float) = 1.0
        _WaveAmp("Wave Amp", float) = 1.0
        _HeightFactor("Height Factor", float) = 1.0
		_HeightCutoff("Height Cutoff", float) = 1.2
        _WindTex("Wind Texture", 2D) = "white" {}
        _WorldSize("World Size", vector) = (1, 1, 1, 1)
        _WindSpeed("Wind Speed", vector) = (1, 1, 1, 1)
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
                "DisableBatching" = "True"
				"Queue"="AlphaTest" 
				"IgnoreProjector"="True" 
				"RenderType"="TransparentCutout"
            }

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fwdbase // shadows
			#pragma multi_compile_fog
            #include "UnityCG.cginc"
			#include "UnityPBSLighting.cginc"
			#include "AutoLight.cginc"
			
			// Properties
			sampler2D _MainTex;
            sampler2D _WindTex;
            float4 _WindTex_ST;
			float4 _Color;
			//float4 _LightColor0; // provided by Unity
            float4 _WorldSize;
			float _Cutoff;
            float _WaveSpeed;
            float _WaveAmp;
            float _HeightFactor;
			float _HeightCutoff;
            float4 _WindSpeed;
			uniform float _Ka;
			uniform float _Kd;
			uniform float _Ks;
			uniform float _fAtt;
			uniform float _specN;

			struct vertexInput
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 uv : TEXCOORD0;
			};

			struct vertexOutput
			{
				float4 pos : SV_POSITION;
				float3 normal : NORMAL;
				float4 uv : TEXCOORD0;
				float4 worldVertex : TEXCOORD2;
    			float3 worldNormal : TEXCOORD3;
				UNITY_FOG_COORDS(1)
                //float2 sp : TEXCOORD0; // test sample position
			};

			vertexOutput vert(vertexInput input)
			{
				vertexOutput output;

				// convert input to clip & world space
				output.pos = UnityObjectToClipPos(input.vertex);
				float4 normal4 = float4(input.normal, 0.0);
				output.normal = normalize(mul(normal4, unity_WorldToObject).xyz);
				float4 worldVertex = mul(unity_ObjectToWorld, input.vertex);
    			float3 worldNormal = normalize(mul(transpose((float3x3)unity_WorldToObject), input.normal.xyz));
				
				output.uv = input.uv;

				output.worldVertex = worldVertex;
    			output.worldNormal = worldNormal;
				UNITY_TRANSFER_FOG(output, output.pos);

                // normalize position based on world size
                float2 samplePos = worldVertex.xz/_WorldSize.xz;
                // scroll sample position based on time
                samplePos += _Time.x * _WindSpeed.xy;
                // sample wind texture
                float windSample = tex2Dlod(_WindTex, float4(samplePos, 0, 0));

                // 0 animation below _HeightCutoff
                float heightFactor = input.vertex.y > _HeightCutoff;
				// make animation stronger with height
				heightFactor = heightFactor * pow(input.vertex.y, _HeightFactor);

                // apply wave animation
                //output.pos.z += sin(_WaveSpeed*windSample)*_WaveAmp * heightFactor;
                output.pos.x += cos(_WaveSpeed*windSample)*_WaveAmp * heightFactor;

				return output;
			}

			float4 frag(vertexOutput v) : SV_Target
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

	}
}