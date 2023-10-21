#define DISOLVE_INCLUDED
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
    float4 vertex : SV_POSITION;
    float4 worldVertex : TEXCOORD1;
    float3 worldNormal : TEXCOORD2;
};

sampler2D _MainTex;
float4 _MainTex_ST;
fixed4 _DissolveColor;
sampler2D _DissolveTexture;
half _Amount;
fixed4 _Color;
uniform float _PixelSize;

uniform float _Ka;
uniform float _Kd;
uniform float _Ks;
uniform float _fAtt;
uniform float _specN;

uniform sampler2D _BurnMap;
uniform float _BurnSize;

v2f vert (appdata v)
{
    v2f o;

    float4 worldVertex = mul(unity_ObjectToWorld, v.vertex);
    float3 worldNormal = normalize(mul(transpose((float3x3)unity_WorldToObject), v.normal.xyz));

    o.vertex = UnityObjectToClipPos(v.vertex);
    o.uv = TRANSFORM_TEX(v.uv, _MainTex);

    o.worldVertex = worldVertex;
    o.worldNormal = worldNormal;

    return o;
}

fixed4 frag (v2f i) : SV_Target
{
    // sample the texture
    half dissolve_value = tex2D(_DissolveTexture, i.uv).r;
    clip(dissolve_value - _Amount);

    half burn = tex2D(_DissolveTexture, i.uv).r - _Amount;

    fixed4 unlitColor;

    if(burn < _BurnSize  && _Amount > 0 && _Amount < 1) {
        unlitColor = tex2D(_BurnMap, float2(burn*(1/_BurnSize), 0));
    } else {
        float2 pixelatedUV = floor(i.uv * _PixelSize) / _PixelSize;
        unlitColor = tex2D(_MainTex, pixelatedUV);
    }

    float3 interpNormal = normalize(i.worldNormal);

    // Calculate ambient RGB intensities
    float Ka = _Ka;
    #if defined(SPOT)
        Ka = 0;
    #endif
    float3 amb = unlitColor.rgb * UNITY_LIGHTMODEL_AMBIENT.rgb * Ka;

    #if defined(SPOT) 
        float3 V = normalize(_WorldSpaceCameraPos - i.worldVertex.xyz);
    #else
        float3 V = normalize(_WorldSpaceCameraPos - i.worldVertex.xyz);
    #endif

    // Calculate diffuse RBG reflections, we save the results of L.N because we will use it again
    // (when calculating the reflected ray in our specular component)
    #if defined(SPOT)
        UNITY_LIGHT_ATTENUATION(fAtt, 0, i.worldVertex.xyz);
    #else
        float fAtt = _fAtt;
    #endif
    float Kd = _Kd;

    #if defined(SPOT)
        float3 L = normalize(_WorldSpaceLightPos0 - i.worldVertex.xyz);
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
    returnColor.a = 0.5;
    return returnColor * _Color;
}

