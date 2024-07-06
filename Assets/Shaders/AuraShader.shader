Shader "Custom/AuraShader"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _AuraColor ("Aura Color", Color) = (0.5, 0.5, 1, 1)
        _AuraSize ("Aura Size", Float) = 0.1
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100

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
            float4 _MainTex_ST;
            float4 _Color;
            float4 _AuraColor;
            float _AuraSize;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                half4 texColor = tex2D(_MainTex, i.uv) * _Color;
                half2 uvCenter = i.uv - 0.5;
                float distance = length(uvCenter);
                half auraFactor = smoothstep(_AuraSize, _AuraSize + 0.1, distance);
                half4 auraColor = lerp(texColor, _AuraColor, auraFactor);
                return auraColor;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
