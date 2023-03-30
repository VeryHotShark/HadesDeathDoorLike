Shader "Hidden/Broccoli/Colored Branch Preview Mode"
{
	Properties {
		_SelectedLevel ("Selected Level", Float) = -1

		_Tiling ("Tiling", Range(1, 500)) = 22
		_Direction ("Direction", Range(0, 1)) = 0.75
		_Color ("Color", Color) = (0.9, 0.9, 0.9, 0.5)
		_TunedColor ("Tuned Color", Color) = (0.8, 0.8, 0.8, 0.5)
		_SelectedColor ("Selected Color", Color) = (0.63, 0.75, 0.88, 0.5)
		_SelectedTunedColor ("Selected Tuned Color", Color) = (0.43, 0.64, 0.88, 0.5)
		_WarpScale ("Warp Scale", Range(0, 1)) = 0.05
		_WarpTiling ("Warp Tiling", Range(1, 10)) = 3.75
	}
	SubShader {

		CGINCLUDE
		struct Input
			{
				float4 position : POSITION;
				float2 uv : TEXCOORD0;
				float4 uv5 : TEXCOORD4;
			};
	
			struct Varying
			{
				float4 position : SV_POSITION;
				float2 uv : TEXCOORD0;
				float4 uv5 : TEXCOORD4;
			};
		ENDCG

		Tags { "RenderType"="Opaque" }
		Pass {
			//Blend One Zero
            //ZTest LEqual
            //Cull Off
            //ZWrite Off
			Blend One Zero
            ZTest LEqual
            Cull Off
            ZWrite On

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#include "UnityCG.cginc"
			
			float _SelectedLevel;
			int _Tiling;
			float _Direction;
			float4 _Color;
			float4 _TunedColor;
			float4 _SelectedColor;
			float4 _SelectedTunedColor;
			float _WarpScale;
			float _WarpTiling;

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _DoClip;
			fixed _Cutoff;
			
			Varying vert (Input v) {
				Varying o;
				o.position = UnityObjectToClipPos(v.position );
				o.uv = v.uv;
				o.uv5 = v.uv5;

				return o;
			}

			half4 frag( Varying i ) : SV_Target {
				/// UV5 information of the mesh.
				/// x: id of the branch.
				/// y: if of the branch skin.
				/// z: id of the struct.
				/// w: tuned.

				// TUNED: use stripped pattern.
				if (i.uv5.w == 1) {
					const float PI = 3.14159;

					//float3 viewPos = UnityObjectToViewPos(i.position.xyz) / 100;
					float2 viewPos = ComputeScreenPos (i.position) / 100;
					float2 pos;
					/*
					pos.x = lerp(i.uv.x, i.uv.y, _Direction);
					pos.y = lerp(i.uv.y, 1 - i.uv.x, _Direction);
					*/
					pos.x = lerp(viewPos.x, viewPos.y, _Direction);
					pos.y = lerp(viewPos.y, 1 - viewPos.x, _Direction);

					pos.x += sin(pos.y * _WarpTiling * PI * 2) * _WarpScale;
					pos.x *= _Tiling;

					fixed value = floor(frac(pos.x) + 0.5);

					// SELECTED.
					if (i.uv5.z == _SelectedLevel) {
						return lerp(_SelectedColor, _SelectedTunedColor, value);
					}
					// NOT SELECTED.
					return lerp (_Color, _TunedColor, value);
				}
				// NOT TUNED: use plain color.
				// SELECTED.
				if (i.uv5.z == _SelectedLevel) {
					return _SelectedColor;
				}
				// NOT SELECTED.
				return _Color;
			}
			ENDCG
		}
		
	}
	FallBack "Diffuse"
}