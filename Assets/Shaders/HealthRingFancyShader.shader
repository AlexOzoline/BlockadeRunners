Shader "Unlit/HealthRingFancyShader"
{

    // Using workshop 8 Phong illumination model as base for shader

    Properties
    {
        //_MainTex ("Texture", 2D) = "white" {}

        _LightPosition("Light Position", Vector) = (0.0, 3.0, 0.0)
		_Ka("Ka", Float) = 1.0
		_Kd("Kd", Float) = 1.0
		_Ks("Ks", Float) = 1.0
		_fAtt("fAtt", Float) = 1.0
		_specN("specN", Float) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            uniform float3 _LightPosition;
			uniform float _Ka;
			uniform float _Kd;
			uniform float _Ks;
			uniform float _fAtt;
			uniform float _specN;

            struct vertIn
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 normal : NORMAL;
            };

            struct vertOut
            {
                //float2 uv : TEXCOORD0;
                //UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;

                float4 color : COLOR;
				float4 worldVertex : TEXCOORD1;
				float3 worldNormal : TEXCOORD2;
            };


            vertOut vert (vertIn v)
            {
                vertOut o;

                // Convert to world coords
				float4 worldVertex = mul(unity_ObjectToWorld, v.vertex);
				float3 worldNormal = normalize(mul(transpose((float3x3)unity_WorldToObject), v.normal.xyz));

				// Transform vertex in world coordinates to camera coordinates
				o.vertex = UnityObjectToClipPos(v.vertex);

				// Pass out the world vertex position and world normal to be interpolated
				// in the fragment shader (and utilised)
				o.worldVertex = worldVertex;

                // Messes with how the world vertex is interpreted in fragment shader
                // Gives wobble effect to final object
                o.worldVertex.z = 10 * sin(_Time.y * 10);

				o.worldNormal = worldNormal;

                // Hardcode base color (green)
                o.color = float4(0.1, 1.0, 0.1, 0);

                return o;
            }

            fixed4 frag (vertOut v) : SV_Target
            {

                // Normalize interpreted normal
                float3 interpNormal = normalize(v.worldNormal);

                // Calculate ambient RGB intensities
                float Ka = _Ka;

                // Pulse ambient lighting
				float3 amb = v.color.rgb * UNITY_LIGHTMODEL_AMBIENT.rgb * Ka * ((sin(_Time.y * 5) + 7) / 8);


                // Diffuse RGB reflections
                
                float fAtt = _fAtt;
				float Kd = _Kd;
				float3 L = normalize(_LightPosition - v.worldVertex.xyz);

                // Remap dot product to make ring appear brighter
				//float LdotN = (3 * dot(L, interpNormal) / 4) + 0.25;
                float LdotN = (dot(L, interpNormal) + 1) / 2;
                // Avoiding diffuse shadows

				float3 dif = fAtt * Kd * v.color.rgb * saturate(LdotN);
                

                //  Specular reflections
                float Ks = _Ks;
				float specN = _specN; // Values>>1 give tighter highlights
				float3 V = normalize(_WorldSpaceCameraPos - v.worldVertex.xyz);
				
				float3 H = normalize(V + L);
				float3 spe = fAtt * float3(1,1,1) * Ks * pow(saturate(dot(interpNormal, H)), specN);

                // Combine (pseudo) Phong illumination model components
                float4 returnColor = float4(0.0f, 0.0f, 0.0f, 0.0f);
				returnColor.rgb = amb.rgb + dif.rgb + spe.rgb;
				returnColor.a = v.color.a;

				return returnColor;

            }
            ENDCG
        }
    }
}