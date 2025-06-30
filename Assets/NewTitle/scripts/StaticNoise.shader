Shader "Custom/StaticNoise"
{
    Properties
    {
        _MainTex ("Noise Texture", 2D) = "white" {}
        _ScrollSpeed ("Scroll Speed", Float) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc" // これが必須！！

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _ScrollSpeed;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex); // tiling offset に対応
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;

                // _Time.x (time) を使ってUVをスクロール
                uv.x += frac(_Time.x * _ScrollSpeed);
                uv.y += frac(_Time.x * _ScrollSpeed * 1.2);

                fixed4 col = tex2D(_MainTex, uv);
                return col;
            }
            ENDCG
        }
    }
}
