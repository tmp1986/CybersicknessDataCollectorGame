Shader "Skybox/Colored Cartoon Procedural Skybox"
{
	Properties
	{
		_Tint ("", Color) = (.5, .5, .5, .5)
		_NightTint("", Color) = (0.5, 0.5, 0.5, 0.5)
    	[Gamma] _Exposure ("", Range(0, 8)) = 1.0
    	_Rotation ("", Range(0, 360)) = 0
		_SunDiscColor("", Color) = (1,1,1,1)
		_SunDiscMultiplier("", Float) = 100000
		_SunDiscExponent("", Float) = 100000
		_SunHaloColor("", Color) = (1,0,0,1)
		_SunHaloExponent("", Float) = 500
		_SunHaloContribution("", Range( 0 , 1)) = 1
		_HorizonLineColor("", Color) = (0,0,0,0)
		_HorizonLineExponent("", Float) = 1
		_HorizonLineContribution("", Range( 0 , 1)) = 1
		_SkyGradientTop("", Color) = (0,0,0,0)
		_SkyGradientBottom("", Color) = (0,0,0,0)
		_SkyGradientExponent("", Float) = 1
	}

	SubShader
	{
		Tags{ "RenderType"="Background"  "Queue"="Background" "PreviewType"="Skybox" }
		Cull Off ZWrite Off

		Pass 
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			half4 _Tint;
			half4 _NightTint;
			half _Exposure;
			float _Rotation;
			half _SunHaloExponent;
			half4 _SunHaloColor;
			half _SunHaloContribution;
			half4 _SkyGradientTop;
			half4 _SkyGradientBottom;
			half _SkyGradientExponent;
			half _HorizonLineExponent;
			half4 _HorizonLineColor;
			half _HorizonLineContribution;
			half4 _SunDiscColor;
			half _SunDiscMultiplier;
			half _SunDiscExponent;

			struct appdata_t {
				float4 vertex : POSITION;
			};

			struct v2f {
				float4 vertex : SV_POSITION;
				float4 worldPos : TEXCOORD0;
			};

			float3 RotateAroundYInDegrees (float3 vertex, float degrees)
			{
				float alpha = degrees * UNITY_PI / 180.0;
				float sina, cosa;
				sincos(alpha, sina, cosa);
				float2x2 m = float2x2(cosa, -sina, sina, cosa);
				return float3(mul(m, vertex.xz), vertex.y).xzy;
			}

			inline float3 CCPSWorldSpaceLightDir( in float3 worldPos )
			{
				#ifndef USING_LIGHT_MULTI_COMPILE
					float3 dir = _WorldSpaceLightPos0.xyz - worldPos * _WorldSpaceLightPos0.w;
				#else
					#ifndef USING_DIRECTIONAL_LIGHT
					float3 dir = _WorldSpaceLightPos0.xyz - worldPos;
					#else
					float3 dir = _WorldSpaceLightPos0.xyz;
					#endif
				#endif
				dir = RotateAroundYInDegrees(dir, _Rotation);
				return dir;
			}

			v2f vert (appdata_t v)
			{
				v2f o;
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
				return o;
			}

			half4 frag (v2f i) : SV_Target
			{
				float3 ase_worldPos = i.worldPos;
				half3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
				float3 n_ase_worldViewDir = normalize( ase_worldViewDir );
				#if defined(LIGHTMAP_ON) && UNITY_VERSION < 560
				float3 ase_worldlightDir = 0;
				#else
				float3 ase_worldlightDir = normalize( CCPSWorldSpaceLightDir( ase_worldPos ) );
				#endif
				float3 n_n_ase_worldlightDir = normalize( -ase_worldlightDir );
				float d_angle = dot( n_ase_worldViewDir , n_n_ase_worldlightDir );
				float mask_horizon = dot( half3(0,-1,0) , n_ase_worldViewDir );
				float4 color_sunHalo = ( saturate( ( pow( saturate( d_angle ) , ( saturate( abs( mask_horizon ) ) * _SunHaloExponent ) ) * ( 1.0 - saturate( pow( ( 1.0 - saturate( mask_horizon ) ) , 50.0 ) ) ) ) ) * _SunHaloColor * _SunHaloContribution );
				float4 color_skyGradient = lerp( _SkyGradientTop , _SkyGradientBottom , saturate( pow( ( 1.0 - saturate( mask_horizon ) ) , _SkyGradientExponent ) ));
				float3 color_horizonLine = lerp( float3( 0,0,0 ) , ( saturate( pow( ( 1.0 - abs( mask_horizon ) ) , _HorizonLineExponent ) ) * (_HorizonLineColor).rgb ) , _HorizonLineContribution);
				float mask_sunDisc = saturate( ( _SunDiscMultiplier * saturate( pow( saturate( d_angle ) , _SunDiscExponent ) ) ) );
				half4 c = lerp( saturate( ( color_sunHalo + color_skyGradient + half4( color_horizonLine , 0.0 ) ) ) , _SunDiscColor , mask_sunDisc);
				c.rgb = c.rgb * _Tint.rgb * unity_ColorSpaceDouble.rgb;
				c.rgb *= _Exposure;
				half time = (dot(ase_worldlightDir, float3(0,1,0)) + 1)*0.5;
				time = clamp(time, 0, 0.5);
				time  = 1 - time * 2;
				c.rgb = lerp(c.rgb, c.rgb * _NightTint, time);
				c.a = 1;
				return c;
			}
			ENDCG
		}
	}
	Fallback "Skybox/Procedural"
	CustomEditor "ColoredCartoonProceduralSkyboxShaderGUI"
}
