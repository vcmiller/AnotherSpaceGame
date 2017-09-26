Shader "Unlit/Atmosphere"
{
	Properties
	{
		_Color("Color", Color) = (0, 0, 1, 1)
		_LightDir("Light Direction", Vector) = (0, 0, 1)
	}
		SubShader
	{
		Tags{"RenderType" = "Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog

			#include "UnityCG.cginc"

			fixed4 _Color;
			float3 _LightDir;

			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
			};

			struct v2f
			{
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
				float3 vpos : TEXCOORD0;
				float3 light : TEXCOORD1;
				float3 norm : TEXCOORD2;
			};
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vpos = normalize(UnityObjectToViewPos(v.vertex));
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.light = mul(UNITY_MATRIX_V, float4(_LightDir, 0));
				o.norm = mul(UNITY_MATRIX_IT_MV, v.normal);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{

				// sample the texture
				fixed4 col = _Color;

				float3 viewAxis = normalize(i.vpos);

				float3 norm = normalize(i.norm);
				float f = abs(dot(norm, viewAxis) * 2);

				float d = dot(i.light, viewAxis);

				if (d < 1 - f) {
					discard;
				} else {
					col *= d / (1.5 - f);
				}

				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}
