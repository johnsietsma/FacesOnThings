Shader "Unlit/FlipBook"
{
	Properties
	{
		_FlipBook ("Texture", 2DArray) = "white" {}
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
			#pragma require 2darray
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float3 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			UNITY_DECLARE_TEX2DARRAY( _MainTex );
			float flipBookIndex = 1;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = float3(v.uv, flipBookIndex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				return UNITY_SAMPLE_TEX2DARRAY(_MainTex, i.uv);
			}
			ENDCG
		}
	}
}
