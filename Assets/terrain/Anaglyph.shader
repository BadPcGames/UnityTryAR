Shader "Custom/Anaglyph"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Offset ("Offset", Float) = 0.01
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

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
            float _Offset;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

          fixed4 frag (v2f i) : SV_Target
            {
                float offsetX;
                #if defined(UNITY_SINGLE_PASS_STEREO)
                    offsetX = _Offset * (unity_StereoEyeIndex * 2 - 1);
                #else
                    offsetX = _Offset * (unity_StereoEyeIndex == 0 ? -1 : 1);
                #endif

                float2 uvLeft = i.uv + float2(-offsetX, 0);
                float2 uvRight = i.uv + float2(offsetX, 0);

                fixed4 colorLeft = tex2D(_MainTex, uvLeft);
                fixed4 colorRight = tex2D(_MainTex, uvRight);


                return fixed4(colorLeft.r, colorRight.g, colorRight.b, 1);
            }
            ENDCG
        }
    }
}