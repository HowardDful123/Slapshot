Shader "Unlit/451ShaderWithTexture"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 200
		Cull Off

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float3 normal : NORMAL;
				float3 vertexWC : TEXCOORD3;
			};

			sampler2D _MainTex;
			float4 LightPosition;
			float4 _MainTex_ST;
            float4x4 MyXformMat;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(MyXformMat, v.vertex);
                o.vertex = mul(UNITY_MATRIX_VP, o.vertex);
				o.vertexWC = mul(UNITY_MATRIX_M, v.vertex);
				float3 p = v.vertex + v.normal;
				p = mul(UNITY_MATRIX_M, float4(p, 1));
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.normal = normalize(p - o.vertexWC);
				return o;
			}

			fixed4 ComputeDiffuse(v2f i)
			{
				float3 l = normalize(LightPosition - i.vertexWC);
				return clamp(dot(i.normal, l), 0, 1);
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				float diff = ComputeDiffuse(i);
				return col * diff;
			}
			ENDCG
		}
	}
}
