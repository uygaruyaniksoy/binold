﻿Shader "Hidden/Blur"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
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

			fixed4 frag (v2f i) : SV_Target
			{
			    float2 offset;
			    float intensity = 3;
			    float4 color = float4(.4, .4, .9, 1);
			    offset.x = 0.003;
				fixed4 col = tex2D(_MainTex, i.uv + float2(offset.x, offset.x)) + 
				tex2D(_MainTex, i.uv + float2(offset.x, 0)) +
				tex2D(_MainTex, i.uv + float2(offset.x, -offset.x)) +
				tex2D(_MainTex, i.uv + float2(0, -offset.x)) + 
				tex2D(_MainTex, i.uv + float2(-offset.x, -offset.x)) + 
				tex2D(_MainTex, i.uv + float2(-offset.x, 0)) +
				tex2D(_MainTex, i.uv + float2(-offset.x, offset.x)) +
				tex2D(_MainTex, i.uv + float2(0, offset.x)) + 
				tex2D(_MainTex, i.uv + float2(0, 0));
				//col.rgb = (col.rgb * 0.125 - tex2D(_MainTex, i.uv)) * intensity * color;
				col.rgb = (col.rgb * 0.11) * intensity * color;
				//return tex2D(_MainTex, i.uv);
				return col;
			}
			ENDCG
		}
	}
}
