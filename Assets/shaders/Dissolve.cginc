/*
    Adapted from and inspired by the following sources:
        - Code Avarice: Burning Edges Dissolve Shader in Unity (http://www.codeavarice.com/dev-blog/tutorial-burning-edges-dissolve-shader-in-unity)
        - febucci//gamedev: Dissolve Shader (https://www.febucci.com/2018/09/dissolve-shader/)
        - Linden Reig: Dissolve Shader in Unity (https://lindenreidblog.com/2017/12/16/dissolve-shader-in-unity/)
        - COMP30019: Workshop-9-Solution PhongShader.shader (https://github.com/COMP30019/Workshop-9-Solution/blob/main/Assets/PhongShader.shader)
*/

#define DISSOLVE_INCLUDED
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
sampler2D _DissolveTexture;
half _Amount;
fixed4 _Color;

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

    // dissolve part of the texture
    half dissolve_value = tex2D(_DissolveTexture, i.uv).r;
    clip(dissolve_value - _Amount);

    // sample burn colour
    half burn = tex2D(_DissolveTexture, i.uv).r - _Amount;

    fixed4 unlitColor;

    // determine unlit colour - either burning or normal
    if(burn < _BurnSize  && _Amount > 0 && _Amount < 1) {
        unlitColor = tex2D(_BurnMap, float2(burn*(1/_BurnSize), 0));
    } else {
        unlitColor = tex2D(_MainTex, i.uv);
    }

    float3 interpNormal = normalize(i.worldNormal);


    // Determine ambient reflection
    float Ka = _Ka;
    #if defined(SPOT) // no reflection if the light source is the flashlight
        Ka = 0;
    #endif


    float3 amb = unlitColor.rgb * UNITY_LIGHTMODEL_AMBIENT.rgb * Ka;

    float3 V = normalize(_WorldSpaceCameraPos - i.worldVertex.xyz);

    // determine light attenuation
    #if defined(SPOT)
        UNITY_LIGHT_ATTENUATION(fAtt, 0, i.worldVertex.xyz); // unity preset for flashlight
    #else
        float fAtt = _fAtt;
    #endif

    float Kd = _Kd;

    // determine direction to light source
    #if defined(SPOT)
        float3 L = normalize(_WorldSpaceLightPos0 - i.worldVertex.xyz);
    #else
        float3 L = _WorldSpaceLightPos0;
    #endif


    float LdotN = dot(L, interpNormal);

    // determine diffuse RGB reflections
    float3 dif = fAtt * _LightColor0 * Kd * unlitColor.rgb * saturate(LdotN);

    float Ks = _Ks;
    
    // Using Blinn-Phong approximation:
    float specN = _specN;
    float3 H = normalize(V + L);
    float3 spe = fAtt * _LightColor0 * Ks * pow(saturate(dot(interpNormal, H)), specN);

    // Combine Phong illumination model components
    float4 returnColor = float4(0.0f, 0.0f, 0.0f, 0.0f);
    returnColor.rgb = amb.rgb + dif.rgb + spe.rgb;
    returnColor.a = 0.5;

    return returnColor * _Color;
}

