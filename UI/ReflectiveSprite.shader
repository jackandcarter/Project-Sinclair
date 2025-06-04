Shader "UI/ReflectiveSprite"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Main Color", Color) = (1,1,1,1)

        // How far below the original icon the reflection is drawn.
        _ReflectionOffset ("Reflection Offset", Float) = -10.0

        // How quickly the reflection fades out from top (fully opaque) to bottom (transparent).
        _ReflectionFade ("Reflection Fade", Range(0,2)) = 1.0
    }
    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
        }
        LOD 100

        //-----------------------------------------------------------------
        // PASS 1: Normal Icon
        //-----------------------------------------------------------------
        Pass
        {
            Name "BASE"
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            Lighting Off
            ZWrite Off
            ZTest Always
            Fog { Mode Off }
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                half2 texcoord  : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color;

            v2f vert(appdata_t IN)
            {
                v2f OUT;
                OUT.vertex = UnityObjectToClipPos(IN.vertex);
                OUT.texcoord = TRANSFORM_TEX(IN.texcoord, _MainTex);
                OUT.color = IN.color * _Color;
                return OUT;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, IN.texcoord) * IN.color;
                return col;
            }
            ENDCG
        }

        //-----------------------------------------------------------------
        // PASS 2: Reflection
        //-----------------------------------------------------------------
        Pass
        {
            Name "REFLECTION"
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            Lighting Off
            ZWrite Off
            ZTest Always
            Fog { Mode Off }
            CGPROGRAM
            #pragma vertex vert_reflection
            #pragma fragment frag_reflection
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f_ref
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                half2 texcoord  : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color;

            float _ReflectionOffset;
            float _ReflectionFade;

            v2f_ref vert_reflection(appdata_t IN)
            {
                v2f_ref OUT;

                // Move the geometry down for the reflection
                float4 modVertex = IN.vertex;
                modVertex.y += _ReflectionOffset;
                OUT.vertex = UnityObjectToClipPos(modVertex);

                // Flip the UV vertically
                float2 uv = IN.texcoord;
                uv.y = 1.0 - uv.y;
                OUT.texcoord = TRANSFORM_TEX(uv, _MainTex);

                OUT.color = IN.color * _Color;
                return OUT;
            }

            fixed4 frag_reflection(v2f_ref IN) : SV_Target
            {
                // Sample the flipped texture
                fixed4 col = tex2D(_MainTex, IN.texcoord) * IN.color;

                // Because we've flipped UV.y,
                //   uv.y=0 is now the TOP of the reflection (where it meets the icon)
                //   uv.y=1 is the BOTTOM (fully transparent).
                // This line makes alpha go from 1 at uv.y=0 to 0 at uv.y=1:
                col.a *= saturate(1.0 - IN.texcoord.y * _ReflectionFade);

                return col;
            }
            ENDCG
        }
    }
}
