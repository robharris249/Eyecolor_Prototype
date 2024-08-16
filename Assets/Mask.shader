Shader "Custom/SharpColorBlockClockwise" {
    Properties {
        _Angle("Angle", Range(0, 180)) = 90
        _Offset("Offset", Range(0, 360)) = 90
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

            float CircleMask(float2 uv, float radius) {
                return saturate(1.0 - length(uv) / radius);
            }

            fixed4 frag(v2f i) : SV_Target {
                float2 uv = i.uv;
                float2 center = float2(0.5, 0.5);
                float radius = 0.5;

                // Shift UV to center
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
                float sectorMask;
                if (startAngle < endAngle) {
                    // Sector does not wrap around 2*PI
                    sectorMask = (angleUV >= startAngle && angleUV <= endAngle) ? 1.0 : 0.0;
                } else {
                    // Sector wraps around 2*PI
                    sectorMask = (angleUV >= startAngle || angleUV <= endAngle) ? 1.0 : 0.0;
                }

                // Circle mask
                float circleMask = CircleMask(uv, radius);

                // Define edge threshold for sharp transition
                float edgeThreshold = 0.01;
                float sectorBlend = smoothstep(0.5 - edgeThreshold, 0.5 + edgeThreshold, sectorMask);

                // Final color calculation
                fixed4 color = lerp(_MainColor, _SectorColor, sectorBlend) * circleMask;

                return color;
            }
            ENDCG
        }
    }

    FallBack "Diffuse"
}