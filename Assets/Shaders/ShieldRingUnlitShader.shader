Shader "Unlit/ShieldRingUnlitShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
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
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct vertIn
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct vertOut
            {
                float4 worldVertex : TEXCOORD1;
                float2 uv : TEXCOORD0;
                //UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;

                float3 normalDir : TEXCOORD2;
                fixed4 col : TEXCOORD3;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            vertOut vert (vertIn v)
            {
                vertOut o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                //UNITY_TRANSFER_FOG(o,o.vertex);

                o.worldVertex = v.vertex;

                // https://pastebin.com/iGuv7AvX
                o.normalDir = normalize(mul(float4(v.normal, 0.0), unity_WorldToObject).xyz);

                // Pulsing dimmer
                float dimmer = 0.5 * (sin(_Time.y * 5) + 1);

                // Normalized dot product for pseudo-fresnel effect
                float dotNorm = (dot(_WorldSpaceCameraPos - o.worldVertex, o.normalDir) + 1) / 2;

                o.col = fixed4( dimmer * dotNorm, dimmer * dotNorm, .75 + .25 * dimmer, 0);



                return o;
            }

            fixed4 frag (vertOut i) : SV_Target
            {
                // sample the texture
                //fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                //UNITY_APPLY_FOG(i.fogCoord, col);

                // https://forum.unity.com/threads/normal-information-for-the-unlit-shader.559288/
                // https://forum.unity.com/threads/angle-between-object-and-camera.214382/
                // dot(camera viewdir, world normal of object)

                // _WorldSpaceCameraPos - i.worldVertex

                //col = mul(col, dot(_WorldSpaceCameraPos - i.worldVertex, i.normalDir));

                /*
                // Pulsing dimmer
                float dimmer = 0.5 * (sin(_Time.y * 5) + 1);

                // Normalized dot product for pseudo-fresnel effect
                float dotNorm = (dot(_WorldSpaceCameraPos - i.worldVertex, i.normalDir) + 1) / 2;

                fixed4 col = fixed4( dimmer * dotNorm, .75 + .25 * dimmer, dimmer * dotNorm, 0);
                */

                return i.col;
            }
            ENDCG
        }
    }
}
