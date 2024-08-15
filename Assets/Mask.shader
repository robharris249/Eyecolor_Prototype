Shader "Custom/Mask" {
    Properties {
        _Angle("Angle", Range(0, 180)) = 90
        _Offset("Offset", Range(0, 360)) = 0
        _MainColor("Main Color", Color) = (1, 0, 0, 1) // Default is red
        _SectorColor("Sector Color", Color) = (0, 1, 0, 1) // Default is green
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

            float _Angle;
            float _Offset;
            fixed4 _MainColor;
            fixed4 _SectorColor;

            v2f vert(appdata v) {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float2 Rotate(float2 uv, float angle) {
                float s = sin(angle);
                float c = cos(angle);
                return float2(uv.x * c - uv.y * s, uv.x * s + uv.y * c);
            }

            float Circle(float2 uv, float radius) {
                return saturate(1.0 - length(uv) / radius);
            }

            fixed4 frag(v2f i) : SV_Target {
                float2 uv = i.uv;
                float2 center = float2(0.5, 0.5);
                float radius = 0.5;

                // Shift UV to center
                uv = uv - center;

                // Calculate the angle of the UV point relative to the center, with a clockwise rotation adjustment
                float angleUV = atan2(uv.y, uv.x); 

                // Normalize to [0, 2*PI] with clockwise adjustment
                angleUV = -angleUV + 3.14159265359 / 2.0;
                if (angleUV < 0.0) {
                    angleUV += 2.0 * 3.14159265359;
                }

                // Convert _Offset and _Angle to radians
                float angleOffset = radians(_Offset);
                float sectorAngle = radians(_Angle);

                // Apply rotation offset to the angle
                float startAngle = angleOffset;
                float endAngle = angleOffset + sectorAngle;

                // Wrap end angle around 2*PI
                if (endAngle > 2.0 * 3.14159265359) {
                    endAngle -= 2.0 * 3.14159265359;
                }

                // Determine if the current UV is within the sector
                float sectorMask;
                if (startAngle < endAngle) {
                    sectorMask = step(startAngle, angleUV) * step(angleUV, endAngle);
                } else { 
                    // This handles the wrap-around case where the sector crosses the 0 angle
                    sectorMask = step(startAngle, angleUV) + step(angleUV, endAngle);
                }

                // Circle mask
                float circleMask = Circle(uv, radius);

                // Apply the sector mask and the circle mask
                float mainColor = 1.0 - circleMask;
                float sectorColor = sectorMask * circleMask;
                return lerp(_MainColor, _SectorColor, sectorColor) * mainColor;
            }
            ENDCG
        }
    }

    FallBack "Diffuse"
}