// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/RadialBlur"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_BlurStrength("", Float) = 0.5
		_BlurWidth("", Float) = 0.5
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			// Upgrade NOTE: excluded shader from DX11, Xbox360, OpenGL ES 2.0 because it uses unsized arrays
			//#pragma exclude_renderers d3d11 xbox360 gles
			#pragma vertex vert
			#pragma fragment frag
			#pragma debug
			
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
			
			uniform sampler2D _MainTex;
			uniform half _BlurStrength;
			uniform half _BlurWidth;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				// just invert the colors
//				col = 1 - col;

//				fixed samples[] = fixed[](-0.05, -0.03, -0.02, -0.01, 0.01, 0.02, 0.03, 0.05);
				
				// from center(0.5, 0.5)
				//fixed2 dir = half(0.5, 0.5);
//				dir -= i.uv;
				fixed2 dir = 0.5 - i.uv;
				//half2 dir = i.uv;
				fixed dist = sqrt(dir.x * dir.x + dir.y * dir.y);

				// normalize direction
				dir = dir / dist;

				half4 sum = col;
				sum += tex2D(_MainTex, i.uv + dir * -0.05 * _BlurWidth);
				sum += tex2D(_MainTex, i.uv + dir * -0.03 * _BlurWidth);
				sum += tex2D(_MainTex, i.uv + dir * -0.02 * _BlurWidth);
				sum += tex2D(_MainTex, i.uv + dir * -0.01 * _BlurWidth);
				sum += tex2D(_MainTex, i.uv + dir * 0.01 * _BlurWidth);
				sum += tex2D(_MainTex, i.uv + dir * 0.02 * _BlurWidth);
				sum += tex2D(_MainTex, i.uv + dir * 0.03 * _BlurWidth);
				sum += tex2D(_MainTex, i.uv + dir * 0.05 * _BlurWidth);

				sum *= 1.0 / 8.0;

				fixed t = clamp(dist * _BlurStrength, 0.0, 1.0);
				//half t = 0.5;
				return lerp(col, sum, t);
				//return col;
			}
			ENDCG
		}
	}
}
