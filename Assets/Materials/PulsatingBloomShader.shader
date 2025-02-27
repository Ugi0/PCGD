Shader "Custom/PulsatingBloomShader"
{
    Properties
    {
        _Color("Color", Color) = (1,1,0,1)
        _EmissionStrength("Emission Strength", Range(1, 5)) = 2
        _PulseSpeed("Pulse Speed", Range(0.1, 5)) = 1
    }
        SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha

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

            fixed4 _Color;
            float _EmissionStrength;
            float _PulseSpeed;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float pulse = abs(sin(_Time.y * _PulseSpeed)) * _EmissionStrength;
                fixed4 col = _Color * pulse;
                return col;
            }
            ENDCG
        }
    }
}