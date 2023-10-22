Shader "Custom/PostProcess" {
  Properties {
    _MainTex("Texture", 2D) = "white" {}
    _Active("Grayscale on", float) = 0
    _BloodTex("Texture", 2D) = "white" {}
    _Color("color", Color) = (1,1,1,1)
  }

  SubShader {
    Pass {
      CGPROGRAM
      #pragma vertex vert_img
      #pragma fragment frag
      #include "UnityCG.cginc" // required for v2f_img

      // Properties
      sampler2D _MainTex;
      sampler2D _BloodTex;
      float _Active;
      fixed4 _Color;

      float4 frag(v2f_img input) : COLOR {
        float3 color = tex2D(_MainTex,input.uv.xy).xyz;

    
        if(_Active == 1.0f) {
            half blood = tex2D(_BloodTex, input.uv).r;

            if(blood < 0.5) {
                return (1.25*float4(color.r, color.g, color.b, 1.0)) * _Color;
            }
            float grayscale = dot(color, float3(0.2126, 0.7152, 0.0722));
            return float4(grayscale,grayscale,grayscale,1.0);
        } else {
            return float4(color.r, color.g, color.b, 1.0);
        }
      }
      ENDCG
}}}