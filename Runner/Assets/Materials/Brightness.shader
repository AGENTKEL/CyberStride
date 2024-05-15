Shader "Custom/Brightness"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Brightness("Brightness", Range(-1.0, 1.0)) = 0.0
        _Contrast("Contrast", Range(0.0, 2.0)) = 1.0
        _Glow("Glow", Range(0.0, 1.0)) = 0.0
    }
        SubShader
        {
            Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
            LOD 100

            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off

            Pass
            {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"

                struct appdata_t
                {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                };

                struct v2f
                {
                    float2 uv : TEXCOORD0;
                    float4 vertex : SV_POSITION;
                };

                sampler2D _MainTex;
                float _Brightness;
                float _Contrast;
                float _Glow;

                v2f vert(appdata_t v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = v.uv;
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    fixed4 col = tex2D(_MainTex, i.uv);

                // Apply brightness
                col.rgb += _Brightness;

                // Apply contrast
                col.rgb = ((col.rgb - 0.5) * max(_Contrast, 0)) + 0.5;

                // Apply glow
                col.rgb += col.rgb * _Glow;

                return col;
            }
            ENDCG
        }
        }
            FallBack "Diffuse"
}
