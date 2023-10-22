Shader "Unlit/ShieldUnlitShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }

        // May have to set blend mode

        LOD 100

        Pass
        {

            Blend SrcAlpha OneMinusSrcAlpha // Traditional transparency

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            uniform int _ShieldCapacity;

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
                o.normalDir = normalize(mul(float4(v.normal, 0.0), unity_WorldToObject).xyz);

                return o;
            }

            fixed4 frag (vertOut i) : SV_Target
            {
                // sample the texture
                //fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                //UNITY_APPLY_FOG(i.fogCoord, col);
                //return col;


                // Custom stuff

                float transparency = 0.5 * _ShieldCapacity / 3 + (sin(_Time.y * 25 / (_ShieldCapacity)) + 1) / 5;

                // Use i.vertex, _Time.y to mess w/ green amount, should make shield ripple

                float greenAmount = (sin((i.vertex.x - i.vertex.y + (_Time.y * 200)) / 50) * 50 + 200) / 250;

                float dotNorm = (dot(_WorldSpaceCameraPos - i.worldVertex, i.normalDir) + 1) / 3;

                fixed4 result = fixed4(0.2, greenAmount, 0.8, transparency / dotNorm);

                return result;
            }
            ENDCG
        }
    }
}
