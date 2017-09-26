// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "Toon/Basic" {
	Properties{
		_Color("Main Color", Color) = (.5,.5,.5,1)
		_MainTex("Base (RGB)", 2D) = "white" {}
	_NormalMap("Normal Map", 2D) = "bump" {}
	_BumpDepth("Bump Depth", Float) = 1.0
		_ToonShade("ToonShader Cubemap(RGB)", CUBE) = "" { }
	_BumpDepth("Bump Depth", Float) = 1.0
	}


		SubShader{
		Tags{ "RenderType" = "Opaque" }
		Pass{
		Name "BASE"
		Cull Off

		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma multi_compile_fog

#include "UnityCG.cginc"

		sampler2D _MainTex;
	sampler2D _NormalMap;
	float _BumpDepth;
	samplerCUBE _ToonShade;
	float4 _MainTex_ST;
	float4 _NormalMap_ST;
	float4 _Color;

	struct appdata {
		float4 vertex : POSITION;
		float2 texcoord : TEXCOORD0;
		float3 normal : NORMAL;
		float4 tangent: TANGENT;
	};

	struct v2f {
		float4 pos : SV_POSITION;
		float2 texcoord : TEXCOORD0;

		float3 normalWorld: TEXCOORD2;
		float3 tangentWorld: TEXCOORD3;
		float3 binormalWorld: TEXCOORD4;

		UNITY_FOG_COORDS(2)
	};

	v2f vert(appdata v) {
		v2f o;
		o.pos = UnityObjectToClipPos(v.vertex);
		o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);

		o.normalWorld = normalize(mul(UNITY_MATRIX_MV, float4(v.normal, 0)).xyz);
		o.tangentWorld = normalize(mul(UNITY_MATRIX_MV, v.tangent).xyz);
		o.binormalWorld = normalize(cross(o.normalWorld, o.tangentWorld));

		UNITY_TRANSFER_FOG(o,o.pos);
		return o;
	}

	fixed4 frag(v2f i) : SV_Target
	{

		float4 texN = tex2D(_NormalMap, i.texcoord.xy * _NormalMap_ST.xy + _NormalMap_ST.zw);

		// unpackNormal Function
		float3 localCoords = float3(2.0 * texN.ag - float2(1.0,1.0), 0.0);
		//localCoords.z = 1.0 - 0.5 * dot(localCoords, localCoords);
		//localCoords.z = 1.0;
		localCoords.z = _BumpDepth;

		// Normal Transpose Matrix
		float3x3 localToView = float3x3(
			i.tangentWorld,
			i.binormalWorld,
			i.normalWorld
		);

		// Calculate Normal Direction
		float3 normalDirection = normalize(mul(localCoords, localToView));

		fixed4 col = _Color * tex2D(_MainTex, i.texcoord);
		fixed4 cube = texCUBE(_ToonShade, normalDirection);
		fixed4 c = fixed4(2.0f * cube.rgb * col.rgb, col.a);
		UNITY_APPLY_FOG(i.fogCoord, c);
		return c;
	}
		ENDCG
	}
	}

		Fallback "VertexLit"
}
