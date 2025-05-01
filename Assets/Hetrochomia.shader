Shader "Custom/TextureUVMapping" {
    Properties {
        _MainTex("Main Texture", 2D) = "white" {}
        _SectorTex("Sector Texture", 2D) = "white" {}
        _Angle("Angle", Range(0, 180)) = 90
        _Offset("Offset", Range(0, 360)) = 0
        _SectorialEnabled("SectorialEnabled", int) = 0
        _BlurAmount("Blur Amount", Range(0.0, 1.0)) = 0.9 // Range from 0 to 1, control the blur amount
    }

    SubShader {
        Tags { "Queue" = "Overlay" }
        LOD 100

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float4 pos : POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            sampler2D _SectorTex;
            float _Angle;
            float _Offset;
            int _SectorialEnabled;
            float _BlurAmount;

            v2f vert(appdata v) {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            // Function to calculate the distance from the angle to the sector edge
            float getEdgeBlendFactor(float angleUV, float startAngle, float endAngle, float blurWidth) {
                float blendFactor = 0.0;

                // If angle is within blur range, blend it
                if (angleUV >= startAngle - blurWidth && angleUV <= startAngle) {
                    blendFactor = 1.0 - (startAngle - angleUV) / blurWidth;
                } else if (angleUV >= endAngle && angleUV <= endAngle + blurWidth) {
                    blendFactor = 1.0 - (angleUV - endAngle) / blurWidth;
                }

                return blendFactor;
            }

            fixed4 frag(v2f i) : SV_Target {
                float2 uv = i.uv;
                float2 center = float2(0.5, 0.5);

                // Shift UV to the center
                uv = uv - center;

                // Calculate the angle of the UV point relative to the center
                float angleUV = -atan2(uv.y, uv.x);

                // Normalize angle to [0, 2*PI]
                if (angleUV < 0.0) {
                    angleUV += 2.0 * 3.14159265359;
                }

                // Convert _Offset and _Angle to radians
                float angleOffset = radians(_Offset);
                float sectorAngle = radians(_Angle);

                // Define sector boundaries (clockwise)
                float startAngle = angleOffset;
                float endAngle = angleOffset + sectorAngle;

                // Handle wrapping around the 2*PI boundary
                float sectorMask = 0.0;
                if (startAngle < endAngle) {
                    // Sector does not wrap around 2*PI
                    sectorMask = (angleUV >= startAngle && angleUV <= endAngle) ? 1.0 : 0.0;
                } else {
                    // Sector wraps around 2*PI
                    sectorMask = (angleUV >= startAngle || angleUV <= endAngle) ? 1.0 : 0.0;
                }

                // Apply the smooth transition based on the distance to the sector edge
                float blurWidth = _BlurAmount * (sectorAngle * 0.5);  // Adjust the blur width
                float edgeBlendFactor = getEdgeBlendFactor(angleUV, startAngle, endAngle, blurWidth);

                // Sample textures
                fixed4 mainTexColor = tex2D(_MainTex, i.uv);
                fixed4 sectorTexColor = tex2D(_SectorTex, i.uv);

                // If sector is enabled, blend the textures based on the edge factor
                fixed4 color;
                if (_SectorialEnabled == 0) {
                    color = mainTexColor;
                } else {
                    // Use edgeBlendFactor to smoothly blend the textures
                    color = lerp(mainTexColor, sectorTexColor, sectorMask * (1.0 - edgeBlendFactor));
                }

                // If the alpha value is too low (faint transparency), discard the fragment
                if (color.a < 0.8) {
                    discard;
                }

                return color;
            }
            ENDCG
        }
    }

    FallBack "Diffuse"
}