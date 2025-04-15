Shader "UI/Blur"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BlurSize ("Blur Size", Range(0, 10)) = 1
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

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

            sampler2D _MainTex;
            float _BlurSize;
            static const int kernelSize = 9;
            static const float2 offsets[kernelSize] = {
                float2(-1, 1), float2(0, 1), float2(1, 1),
                float2(-1, 0), float2(0, 0), float2(1, 0),
                float2(-1, -1), float2(0, -1), float2(1, -1)
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = fixed4(0, 0, 0, 0);
                for (int j = 0; j < kernelSize; j++)
                {
                    col += tex2D(_MainTex, i.uv + offsets[j] * _BlurSize * 0.001);
                }
                col /= kernelSize;
                return col;
            }
            ENDCG
        }
    }
}