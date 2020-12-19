// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/EnemyShader" {
    Properties {
        _MainTex ("Ghost Texture", 2D) = "white" {}
		_ambientFactor ("ambient",Float) = 1
		_diffuseFactor ("diffuse",Float) = 1
		_specularFactor ("specular", Float) = 1
		_Shininess ("Shininess", Float) = 2 // power used in specular
        _Transparency ("Transparency", Float) = 0.8
    }
    SubShader {
     Tags {"Queue"="Transparent" "RenderType"="Transparent"}
		LOD 200

		Blend SrcAlpha OneMinusSrcAlpha

		Pass {
			CGPROGRAM
			#pragma vertex vert 
			#pragma fragment frag

			#include "UnityCG.cginc" 
			#include "UnityLightingCommon.cginc"

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _ambientFactor;
			float _diffuseFactor;
			float _specularFactor;
			float _Shininess;
			float _Transparency;
			
			struct vertIn {
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float2 uv : TEXCOORD0;
			};

			// model -> world -> view -> projection
			struct vertOut {
				float4 vertexWorld : TEXCOORD1;
				float4 vertexProjection : POSITION;
				float3 normalWorld : NORMAL;
				float2 uv : TEXCOORD0;
			};
			
			vertOut vert(vertIn v) {

                vertOut o;

                float4 worldVertex = mul(unity_ObjectToWorld, v.vertex);
				float3 worldNormal = normalize(mul(transpose((float3x3)unity_WorldToObject), v.normal.xyz));

                o.vertexProjection = UnityObjectToClipPos(v.vertex);
				// o.color = v.color;

                o.vertexWorld = worldVertex;
				o.normalWorld = worldNormal;

				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}

			fixed4 frag(vertOut v) : SV_TARGET {
				float4 tex = tex2D(_MainTex, v.uv);
				// for ambient light
				float3 ambient = UNITY_LIGHTMODEL_AMBIENT.rgb*tex.rgb;
				float3 normal = normalize(v.normalWorld);

				// for diffuse, we first need to calculate vector from vertex to light source
				// _WorldSpaceLightPos0 : if w component is 1, then it is a light source with position xyz
				// else w component is 0, and xzy is a vector of directional light without a position 
				// _LightColor0 : color of the first directional light
				float3 L = normalize(_WorldSpaceLightPos0.xyz);
				float LdotN = dot(L, normal.xyz);
				float3 diffuse = _LightColor0.rgb * tex.rgb * saturate(LdotN);
				
				// for specular, we need to calculate vector from vertex to camera, 
				// and vector of directional light reflected off surface
				float3 V = normalize(_WorldSpaceCameraPos - v.vertexWorld.xyz);
				
				// vector of directional light reflected off surface can be calculated with Cg function
				// 'reflect' which takes the incident vector and the surface normal
				float3 R = 2*LdotN*normal.xyz-L;
				float VdotR = saturate(dot(V,R));
				float3 specular =_LightColor0.rgb*pow(VdotR,_Shininess);

				float3 color = _ambientFactor*ambient + _diffuseFactor*diffuse + _specularFactor*specular;

				return float4(color,_Transparency);
			}

			ENDCG
		}
    }
}
