// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

#define PIXELATED_INCLUDED
#include "UnityPBSLighting.cginc"
#include "AutoLight.cginc"

uniform float _Ka;
uniform float _Kd;
uniform float _Ks;
uniform float _fAtt;
uniform float _specN;

struct vertIn
{
    float4 vertex : POSITION;
    float4 normal : NORMAL;
    float4 uv : TEXCOORD0;
};

struct vertOut
{
    float4 vertex : SV_POSITION;
    float4 uv : TEXCOORD0;
    float4 worldVertex : TEXCOORD1;
    float3 worldNormal : TEXCOORD2;
};

uniform float _PixelSize;
uniform sampler2D _MainTex;

// Implementation of the vertex shader
vertOut vert(vertIn v)
{
    vertOut o;

    float4 worldVertex = mul(unity_ObjectToWorld, v.vertex);
    float3 worldNormal = normalize(mul(transpose((float3x3)unity_WorldToObject), v.normal.xyz));

    o.vertex = UnityObjectToClipPos(v.vertex);
    o.uv = v.uv;

    o.worldVertex = worldVertex;
    o.worldNormal = worldNormal;

    return o;
};

// Implementation of the fragment (pixel) shader
fixed4 frag(vertOut v) : SV_Target
{

    
    float2 pixelatedUV = floor(v.uv * _PixelSize) / _PixelSize;
    fixed4 unlitColor = tex2D(_MainTex, pixelatedUV);

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

    return returnColor;
};
