Shader "Hidden/PostProcessing"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_SecondTex ("Texture", 2D) = "white" {}
		_ThirdTex ("Texture", 2D) = "white" {}
		_FourthTex ("Texture", 2D) = "white" {}
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
			sampler2D _SecondTex;
			sampler2D _ThirdTex;
			sampler2D _FourthTex;

			fixed4 frag (v2f i) : SV_Target
			{
//			    fixed4 outline = tex2D(_SecondTex, i.uv) -
//				    tex2D(_ThirdTex, i.uv) * 100;
//				outline = fixed4(max(0, outline.r),
//				    max(0, outline.g),
//				    max(0, outline.b),
//				    1);
//				    
    			fixed4 col;
//				if (outline.a > 0) {
                col = tex2D(_MainTex, i.uv);				        
//				} else {
//				    col = outline;
//				}
				    
				return col;
			}
			ENDCG
		}
	}
}
