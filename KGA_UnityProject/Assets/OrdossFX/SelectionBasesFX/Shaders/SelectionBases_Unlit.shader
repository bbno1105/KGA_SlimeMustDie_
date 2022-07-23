Shader "SelectionBases/Unlit"
{
    Properties
    {
		[HDR]_TintColor("Color", Color) = (1,1,1,1)
        _MainTex ("Texture", 2D) = "white" {}
		_ScaleSpeedMainTex ("ScaleSpeedXYMain", Vector) = (1,1,0,0)
		_MainNoiseTex ("mainNoise", 2D) = "white" {}
		_ScaleSpeedMainNoiseTex ("ScaleSpeedXYMainNoise", Vector) = (1,1,1,1)
		_AlphaMask ("AlphaMask", 2D) = "white" {}
		_SoftFade("Soft Fade", Float) = 3
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue" = "Transparent"}
		Blend SrcAlpha One
		Cull Off
		ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog
			#pragma multi_compile_particles

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv1 : TEXCOORD0;
				float2 uv2 : TEXCOORD1;
				float2 uv3 : TEXCOORD2;
				float4 color : COLOR0;
            };

            struct v2f
            {
                float2 uv1 : TEXCOORD0;
				float2 uv2 : TEXCOORD1;
				float2 uv3 : TEXCOORD2;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
				float4 screenPos : TEXCOORD4;
				float4 color : COLOR0;
            };

			float4 _TintColor;
            sampler2D _MainTex;
            float4 _MainTex_ST;
			float4 _ScaleSpeedMainTex;
			sampler2D _MainNoiseTex;
			float4 _MainNoiseTex_ST;
			float4 _ScaleSpeedMainNoiseTex;
			sampler2D _AlphaMask;
			float4 _AlphaMask_ST;
			float _SoftFade;
			sampler2D _CameraDepthTexture;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv1 = TRANSFORM_TEX(v.uv1, _MainTex);
				o.uv2 = TRANSFORM_TEX(v.uv2, _MainNoiseTex);
				o.uv3 = TRANSFORM_TEX(v.uv3, _AlphaMask);
				#ifdef SOFTPARTICLES_ON
				o.screenPos = ComputeScreenPos(o.vertex);
				COMPUTE_EYEDEPTH(o.screenPos.z);
				#endif
                UNITY_TRANSFER_FOG(o,o.vertex);
				o.color = v.color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				half fade = 1;
				#ifdef SOFTPARTICLES_ON
				float sceneZ = LinearEyeDepth(UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.screenPos))));
				float partZ = i.screenPos.z;
				fade = saturate(_SoftFade * (sceneZ - partZ));
				#endif
                fixed4 col = tex2D(_MainTex, i.uv1-fixed2(_ScaleSpeedMainTex.z,_ScaleSpeedMainTex.a)*_Time)*tex2D(_MainNoiseTex, i.uv2-fixed2(_ScaleSpeedMainNoiseTex.z,_ScaleSpeedMainNoiseTex.a)*_Time)*i.color*_TintColor;
				fixed4 alphamask = tex2D(_AlphaMask,i.uv3);
				col.a = col.a * fade * alphamask.a;
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
