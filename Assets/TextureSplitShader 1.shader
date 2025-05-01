// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/TextureSplitShader"
{
    Properties
    {
        _MainTex ("Texture 1", 2D) = "white" { }
        _SecondTex ("Texture 2", 2D) = "white" { }
    }

    SubShader
    {
        Tags { "Queue" = "Overlay" }

        Pass
        {
            // Enable alpha testing with a custom threshold
            AlphaTest Greater 0.1 // Set a threshold to remove faint transparency at edges

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            // Texture samplers for the two textures
            sampler2D _MainTex; // First texture (left side)
            sampler2D _SecondTex; // Second texture (right side)

            // Vertex shader (simple pass-through)
            struct appdata
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.texcoord = v.texcoord;
                return o;
            }

            // Fragment shader
            half4 frag(v2f i) : SV_Target
            {
                half4 color;

                // Check if we're on the left or right half
                if (i.texcoord.x < 0.5)
                {
                    // Sample the first texture
                    color = tex2D(_MainTex, i.texcoord);
                }
                else
                {
                    // Sample the second texture
                    color = tex2D(_SecondTex, i.texcoord);
                }

                // If the alpha value is too low (faint transparency), discard the fragment
                if (color.a < 0.8)
                {
                    discard;
                }

                return color;
            }
            ENDCG
        }
    }

    Fallback "Diffuse"
}