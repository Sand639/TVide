Shader "UI/TransparentDotNoiseEffect"
{
    Properties
    {
        _NoiseStrength("Noise Strength", Range(0,1)) = 0.5
        _DotSize("Dot Size", Float) = 0.02
        _TimeSpeed("Time Speed", Float) = 1.0
        _Alpha("Alpha", Range(0,1)) = 1.0 // 透明度プロパティ追加
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
            float _DotSize;
            float _TimeSpeed;
            float _Alpha; // 透明度の変数

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

                // ドット模様を生成
                float noise = rand(floor(uv / _DotSize) * _Time.y * _TimeSpeed);

                // ノイズ強さと透明度を加味
                float brightness = noise * _NoiseStrength;

                // アルファ値が0の場合は完全に透明にする
                if (_Alpha == 0)
                    return fixed4(0, 0, 0, 0); // 完全透明

                return fixed4(brightness, brightness, brightness, _Alpha); // 透明度が調整可能
            }
            ENDCG
        }
    }
}
