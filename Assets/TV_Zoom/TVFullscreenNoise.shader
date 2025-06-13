Shader "UI/TVFullscreenNoise"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _NoiseStrength("Noise Strength", Range(0,1)) = 0.4
        _ScanlineStrength("Scanline Strength", Range(0,1)) = 0.3
        _TimeSpeed("Time Speed", Float) = 1
    }

        SubShader
        {
            Tags { "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
            LOD 100

            Pass
            {
                ZWrite Off
                Blend SrcAlpha OneMinusSrcAlpha
                Cull Off

                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #pragma target 2.0
                #include \"UnityCG.cginc\"

                sampler2D _MainTex;
                float4 _MainTex_ST;
                float _NoiseStrength;
                float _ScanlineStrength;
                float _TimeSpeed;

                struct appdata_t {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                };

                struct v2f {
                    float2 uv : TEXCOORD0;
                    float4 vertex : SV_POSITION;
                };

                float rand(float2 co) {
                    return frac(sin(dot(co.xy, float2(12.9898, 78.233))) * 43758.5453);
                }

                v2f vert(appdata_t v) {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target {
                    float2 uv = i.uv;

                    float noise = rand(float2(uv.y, _Time.y * _TimeSpeed)) * _NoiseStrength;
                    float scanline = sin(uv.y * 800.0) * _ScanlineStrength;

                    fixed4 col = tex2D(_MainTex, uv);
                    col.rgb += noise - scanline;
                    col.a = 1.0; // 完全不透明

                    return col;
                }
                ENDCG
            }
        }

            FallBack "UI/Default"
}
