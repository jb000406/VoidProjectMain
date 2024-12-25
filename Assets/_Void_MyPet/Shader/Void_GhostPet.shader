Shader "Void/GhostShader"
{
    Properties
    {
        _BaseColor ("Base Color", Color) = (0.2, 0.8, 1.0, 1.0)
        _EmissionColor ("Emission Color", Color) = (0.2, 0.8, 1.0, 1.0)
        _DissolveThreshold ("Dissolve Threshold", Range(0, 1)) = 0.5
        _NoiseScale ("Noise Scale", Float) = 5.0
        _MainTex ("Main Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 200

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _BaseColor;
            float4 _EmissionColor;
            float _DissolveThreshold;
            float _NoiseScale;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                float4 texColor = tex2D(_MainTex, i.uv);

                // Noise calculation for dissolve
                float noise = frac(sin(dot(i.uv * _NoiseScale, float2(12.9898, 78.233))) * 43758.5453);
                if (noise < _DissolveThreshold)
                {
                    discard;
                }

                // Base Color with Transparency
                float4 finalColor = _BaseColor * texColor;
                finalColor.a *= texColor.a;

                // Add Emission
                finalColor.rgb += _EmissionColor.rgb * 2.0;

                return finalColor;
            }
            ENDCG
        }
    }
    FallBack "Transparent/Diffuse"
}
