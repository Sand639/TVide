Shader "UI/TransparentNoiseOnly"
{
    Properties
    {
        _NoiseStrength("Noise Strength", Range(0,1)) = 0.5
        _TimeSpeed("Time Speed", Float) = 1.0
    }

        SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            float _NoiseStrength;
            float _TimeSpeed;

            struct appdata_t {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float rand(float2 co)
            {
                return frac(sin(dot(co.xy, float2(12.9898, 78.233))) * 43758.5453);
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 uv = i.uv;
                float noise = rand(uv * _Time.y * _TimeSpeed);

                float intensity = step(1.0 - _NoiseStrength, noise); // ノイズ点の発生率

                float brightness = intensity * 1.0; // 白い点
                return fixed4(brightness, brightness, brightness, intensity);
            }
            ENDCG
        }
    }
}
