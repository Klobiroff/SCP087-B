Shader "Hidden/MotionBlur"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _AccumulationTex ("Accumulation", 2D) = "black" {}
        _BlurAmount ("Blur Amount", Float) = 0.8
        _Separation ("Double Vision Separation", Float) = 0.02
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always
        ZTest Always Cull Off ZWrite Off
        Fog { Mode off }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma fragmentoption ARB_precision_hint_fastest
            
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
            
            sampler2D _MainTex;
            sampler2D _AccumulationTex;
            float _BlurAmount;
            float _Separation;

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;
                
                #if UNITY_UV_STARTS_AT_TOP
                if (_ProjectionParams.x > 0)
                    uv.y = 1 - uv.y;
                #endif

                // Calculate the offset for double vision
                float2 offset = float2(_Separation * 0.5, 0);
                
                // Sample with bilinear filtering
                fixed4 mainRight = tex2D(_MainTex, uv + offset);
                fixed4 mainLeft = tex2D(_MainTex, uv - offset);
                fixed4 currentFrame = lerp(mainLeft, mainRight, 0.5);

                fixed4 accumRight = tex2D(_AccumulationTex, uv + offset);
                fixed4 accumLeft = tex2D(_AccumulationTex, uv - offset);
                fixed4 previousFrames = lerp(accumLeft, accumRight, 0.5);
                
                return lerp(currentFrame, previousFrames, _BlurAmount);
            }
            ENDCG
        }
    }
    FallBack Off
}
