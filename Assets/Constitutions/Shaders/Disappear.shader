// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Disappear"
{
	Properties{
		_Diffuse("Diffuse", Color) = (1,1,1,1)
		_DissolveColor("Dissolve Color", Color) = (0,0,0,0)
		_DissolveEdgeColor("Dissolve Edge Color", Color) = (1,1,1,1)
		_MainTex("Base 2D", 2D) = "white"{}
		_DissolveMap("DissolveMap", 2D) = "white"{}
		_DissolveThreshold("DissolveThreshold", Range(0,1)) = 0
		_ColorFactor("ColorFactor", Range(0,1)) = 0.7
		_DissolveEdge("DissolveEdge", Range(0,1)) = 0.8
	}

	CGINCLUDE
#include "Lighting.cginc"  
		uniform fixed4 _Diffuse;
		uniform fixed4 _DissolveColor;
		uniform fixed4 _DissolveEdgeColor;
		uniform sampler2D _MainTex;
		uniform float4 _MainTex_ST;
		uniform sampler2D _DissolveMap;
		uniform float _DissolveThreshold;
		uniform float _ColorFactor;
		uniform float _DissolveEdge;

		struct v2f
		{
			float4 pos : SV_POSITION;
			float3 worldNormal : TEXCOORD0;
			float2 uv : TEXCOORD1;
		};

		v2f vert(appdata_base v)
		{
			v2f o;
			o.pos = UnityObjectToClipPos(v.vertex);
			o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
			o.worldNormal = mul(v.normal, (float3x3)unity_WorldToObject);
			return o;
		}

		fixed4 frag(v2f i) : SV_Target
		{
			//采样Map  
			fixed4 dissolveValue = tex2D(_DissolveMap, i.uv);
			//小于阈值的部分discard  
			if (dissolveValue.r < _DissolveThreshold)
			{
				discard;
			}
			//Diffuse + Ambient光照计算  
			fixed3 worldNormal = normalize(i.worldNormal);
			fixed3 worldLightDir = normalize(_WorldSpaceLightPos0.xyz);
			fixed3 lambert = saturate(dot(worldNormal, worldLightDir));
			fixed3 albedo = lambert * _Diffuse.xyz * _LightColor0.xyz + UNITY_LIGHTMODEL_AMBIENT.xyz;
			fixed3 color = tex2D(_MainTex, i.uv).rgb * albedo;


			float percentage = _DissolveThreshold / dissolveValue.r;
			float lerpEdge = sign(percentage - _ColorFactor - _DissolveEdge);
			fixed3 edgeColor = lerp(_DissolveEdgeColor.rgb, _DissolveColor.rgb, saturate(lerpEdge));
			//最终输出颜色的lerp值  
			float lerpOut = sign(percentage - _ColorFactor);
			//最终颜色在原颜色和上一步计算的颜色之间差值
			fixed3 colorOut = lerp(color, edgeColor, saturate(lerpOut));
			return fixed4(colorOut, 1);
			}
		ENDCG
			SubShader
			{
				Tags{ "RenderType" = "Opaque" }
				Pass
				{
					CGPROGRAM
				#pragma vertex vert  
				#pragma fragment frag     
		ENDCG
			}
		}
	FallBack "Diffuse"
}