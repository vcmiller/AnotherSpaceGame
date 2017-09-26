// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "Toon/BasicAlpha" {
	Properties{
		_Color("Main Color", Color) = (.5,.5,.5,1)
		_MainTex("Base (RGB)", 2D) = "white" {}
		_ToonShade("ToonShader Cubemap(RGB)", CUBE) = "" { }
	}


	SubShader{
		Tags{ "RenderType" = "Transparent" "Queue" = "Transparent"}
		Pass{
			Name "BASE"
			ZWrite off
			Blend srcAlpha oneMinusSrcAlpha
			Cull Off

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog

			#include "UnityCG.cginc"

			sampler2D _MainTex;
			samplerCUBE _ToonShade;
			float4 _MainTex_ST;
			float4 _Color;

			struct appdata {
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
				float3 normal : NORMAL;
			};

			struct v2f {
				float4 pos : SV_POSITION;
				float2 texcoord : TEXCOORD0;
				float3 normal : TEXCOORD1;
				float3 vpos : TEXCOORD3;

				UNITY_FOG_COORDS(2)
			};

			v2f vert(appdata v) {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.vpos = UnityObjectToViewPos(v.vertex);
				o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);

				o.normal = mul(UNITY_MATRIX_IT_MV, v.normal);

				UNITY_TRANSFER_FOG(o,o.pos);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				float3 fw = normalize(i.vpos);
				float3 rt = normalize(cross(fw, float3(0, 1, 0)));
				float3 up = cross(rt, fw);

				fw *= -1;

				float4x4 viewToDir = float4x4(
					float4(rt, 0),
					float4(up, 0),
					float4(fw, 0),
					float4(0, 0, 0, 1)
				);

				float3 normalView = normalize(mul(viewToDir, float4(i.normal, 0)).xyz);

				fixed4 col = _Color * tex2D(_MainTex, i.texcoord);
				fixed4 cube = texCUBE(_ToonShade, normalView);
				fixed4 c = fixed4(2.0f * cube.rgb * col.rgb, col.a * cube.a);
				UNITY_APPLY_FOG(i.fogCoord, c);
				return c;
			}
			ENDCG
		}
	}

	Fallback "VertexLit"
}
