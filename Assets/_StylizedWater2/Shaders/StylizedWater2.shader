//Stylized Water 2
//Staggart Creations (http://staggart.xyz)
//Copyright protected under Unity Asset Store EULA

Shader "Universal Render Pipeline/FX/Stylized Water 2"
{
	Properties
	{
		//[Header(Rendering)]
		[MaterialEnum(Off,0,On,1)]_ZWrite("Depth writing", Float) = 0
		[Enum(UnityEngine.Rendering.CullMode)] _Cull("Render faces", Float) = 2
		[MaterialEnum(Simple,0,Advanced,1)] _ShadingMode("Shading mode", Float) = 1

		//[Header(Feature switches)]
		_DisableDepthTexture("Disable depth texture", Float) = 0
		_AnimationParams("XY=Direction, Z=Speed", Vector) = (1,1,1,0)
		_SlopeParams("Slope (X=Stretch) (Y=Speed)", Vector) = (0.5, 4, 0, 0)
		[MaterialEnum(Mesh UV,0,World XZ projected ,1)]_WorldSpaceUV("UV Source", Float) = 1

		//[Header(Color)]
		[HDR]_BaseColor("Deep", Color) = (0, 0.44, 0.62, 1)
		[HDR]_ShallowColor("Shallow", Color) = (0.1, 0.9, 0.89, 0.02)
		[HDR]_HorizonColor("Horizon", Color) = (0.84, 1, 1, 0.15)
		_HorizonDistance("Horizon Distance", Range(0.01 , 32)) = 8
		_Depth("Color Depth", Range(0.01 , 8)) = 1
		_DepthExp("Exponential Blend", Range(0 , 1)) = 1
		_WaveTint("Wave tint", Range( -0.1 , 0.1)) = 0
		_TranslucencyParams("Translucency", Vector) = (1,4,0,0)
		//X: Strength
		//Y: Exponent
		_EdgeFade("Edge Fade", Float) = 0.1
		_ShadowStrength("Shadow Strength", Range(0 , 1)) = 1

		//[Header(Underwater)]
		[Toggle(_CAUSTICS)] _CausticsOn("Caustics ON", Float) = 1
		_CausticsBrightness("Brightness", Float) = 2
		_CausticsTiling("Tiling", Float) = 0.5
		_CausticsSpeed("Speed multiplier", Float) = 0.1
		[NoScaleOffset][SingleLineTexture]_CausticsTex("Caustics Mask", 2D) = "black" {}
		
		[Toggle(_REFRACTION)] _RefractionOn("_REFRACTION", Float) = 1
		_RefractionStrength("_RefractionStrength", Range(0 , 1)) = 0.1

		//[Header(Intersection)]
		[MaterialEnum(Camera Depth,0,Vertex Color,1,Both,2)] _IntersectionSource("Intersection source", Float) = 0
		[MaterialEnum(None,0,Sharp,1,Smooth,2)] _IntersectionStyle("Intersection style", Float) = 1

		[NoScaleOffset][SingleLineTexture]_IntersectionNoise("Intersection noise", 2D) = "white" {}
		_IntersectionColor("Color", Color) = (1,1,1,1)
		_IntersectionLength("Distance", Range(0.01 , 5)) = 2
		_IntersectionClipping("Cutoff", Range(0.01, 1)) = 0.5
		_IntersectionFalloff("Falloff", Range(0.01 , 1)) = 0.5
		_IntersectionTiling("Noise Tiling", float) = 0.2
		_IntersectionSpeed("Speed multiplier", float) = 0.1
		_IntersectionRippleDist("Ripple distance", float) = 32
		_IntersectionRippleStrength("Ripple Strength", Range(0 , 1)) = 0.5

		//[Header(Foam)]
		[NoScaleOffset][SingleLineTexture]_FoamTex("Foam Mask", 2D) = "black" {}
		_FoamColor("Color", Color) = (1,1,1,1)
		_FoamSize("Cutoff", Range(0.01 , 0.999)) = 0.01
		_FoamSpeed("Speed multiplier", float) = 0.1
		_FoamWaveMask("Wave mask", Range(0 , 1)) = 0
		_FoamWaveMaskExp("Wave mask exponent", Range(1 , 8)) = 1
		_FoamTiling("Tiling", float) = 0.1

		//[Header(Normals)]
		[Toggle(_NORMALMAP)] _NormalMapOn("_NORMALMAP", Float) = 1
		[NoScaleOffset][Normal][SingleLineTexture]_BumpMap("Normals", 2D) = "bump" {}
		_NormalTiling("Tiling", Float) = 1
		_NormalStrength("Strength", Range(0 , 1)) = 0.5
		_NormalSpeed("Speed multiplier", Float) = 0.2
		//X: Start
		//Y: End
		//Z: Tiling multiplier
		_DistanceNormalParams("Distance normals", vector) = (100, 300, 0.25, 0)
		[NoScaleOffset][Normal][SingleLineTexture]_BumpMapLarge("Normals (Distance)", 2D) = "bump" {}

		_SparkleIntensity("Sparkle Intensity", Range(0 , 10)) = 00
		_SparkleSize("Sparkle Size", Range( 0 , 1)) = 0.280

		//[Header(Sun Reflection)]
		_SunReflectionSize("Size", Range(0 , 1)) = 0.5
		_SunReflectionStrength("Strength", Float) = 10
		_SunReflectionDistortion("Distortion", Range( 0 , 1)) = 0.49

		//[Header(World Reflection)]
		_ReflectionStrength("Strength", Range( 0 , 1)) = 0
		_ReflectionDistortion("Distortion", Range( 0 , 1)) = 0.05
		_ReflectionBlur("Blur", Range( 0 , 1)) = 0	
		_ReflectionFresnel("Curvature mask", Range( 0.01 , 20)) = 5	
		_PlanarReflectionLeft("Planar Reflections", 2D) = "" {} //Instanced
		//_PlanarReflectionRight("Planar Reflections", 2D) = "" {} //Instanced
		_PlanarReflectionsEnabled("Planar Enabled", float) = 0 //Instanced
		_PlanarReflectionsParams("Planar angle mask", Range(0 , 2)) = 0
		//X: Angle mask
		
		//[Header(Waves)]
		[Toggle(_WAVES)] _WavesOn("_WAVES", Float) = 0

		_WaveSpeed("Speed", Float) = 2
		_WaveHeight("Height", Range(0 , 10)) = 0.25
		_WaveNormalStr("Normal Strength", Range(0 , 6)) = 0.5
		_WaveDistance("Distance", Range(0 , 1)) = 0.8
		_WaveFadeDistance("Fade Distance", vector) = (150, 300, 0, 0)
		//_ShoreLineWaveStr("Shore line strength", Range(0 , 1)) = 0.5
		//_ShoreLineWaveDistance("Shore wave distance", Range(1 , 256)) = 64
		//_ShoreLineLength("Shore Length", Range(0.0 , 30)) = 2

		//[Header(Prototype)]
		_WaveSteepness("Steepness", Range(0 , 1)) = 0.1
		_WaveCount("Count", Range(1 , 5)) = 1
		_WaveDirection("Direction", vector) = (1,1,1,1)

		/* start Tesselation */
		//_TessValue("Max Tessellation", Range(1, 32)) = 16
		//_TessMin("Start Distance", Float) = 5
		//_TessMax("End Distance", Float) = 25
 		/* end Tesselation */

		//Instanced properties
		_VertexColorMask ("Vertex color mask", vector) = (0,0,0,0)
		
		/* start CurvedWorld */
		//[CurvedWorldBendSettings] _CurvedWorldBendSettings("0|1|1", Vector) = (0, 0, 0, 0)
		/* end CurvedWorld */
	}

	SubShader
	{
		LOD 0
		
		Tags { "RenderPipeline" = "UniversalPipeline" "RenderType" = "Transparent" "Queue" = "Transparent" }
		//Tags { "RenderPipeline"="UniversalPipeline" "RenderType"="Opaque" "Queue"="Geometry+1" }
				
		Pass
		{	
			Name "ForwardLit"
			Tags { "LightMode"="UniversalForward" }
			
			Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
			ZWrite [_ZWrite]
			Cull [_Cull]
			ZTest LEqual
			ColorMask RGBA
			
			HLSLPROGRAM

			#pragma multi_compile_instancing
			/* start UnityFog */
			#pragma multi_compile_fog
			/* end UnityFog */

			#pragma prefer_hlslcc gles
			/* if Tesselation */
			//#pragma target 4.6
			/* endif Tesselation */
			/* ifn Tesselation */
			#pragma target 3.0
			/* endifn Tesselation */
			#pragma exclude_renderers d3d11_9x ps3 psp2 xbox360 gles n3ds wiiu
			
			// Material Keywords
			//Note: _fragment suffix fails to work on GLES. Keywords would always be stripped
			#pragma shader_feature_local _NORMALMAP
			#pragma shader_feature_local _DISTANCE_NORMALS
			#pragma shader_feature_local _WAVES
			#pragma shader_feature_local _FOAM
			#pragma shader_feature_local _UNLIT
			#pragma shader_feature_local _TRANSLUCENCY
			#pragma shader_feature_local _CAUSTICS
			#pragma shader_feature_local _REFRACTION
			#pragma shader_feature_local _ADVANCED_SHADING
			#pragma shader_feature_local _FLAT_SHADING
			#pragma shader_feature_local _SPECULARHIGHLIGHTS_OFF
			#pragma shader_feature_local _ENVIRONMENTREFLECTIONS_OFF
			#pragma shader_feature_local _RECEIVE_SHADOWS_OFF
			#pragma shader_feature_local _DISABLE_DEPTH_TEX
			#pragma shader_feature_local _SHARP_INERSECTION
			#pragma shader_feature_local _SMOOTH_INTERSECTION
			#pragma shader_feature_local _RIVER
			
			#if _RIVER
			#undef _WAVES
			#endif
			
			#if _DISABLE_DEPTH_TEX
			#undef _CAUSTICS //Caustics require depth texture
			#endif
			
			#if !_NORMALMAP && !_WAVES
			#undef _REFRACTION
			#endif
			
			//Unity global keywords
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
			#pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
			#pragma multi_compile _ _SHADOWS_SOFT
			//Tiny usecase, disabled to reduce variants (each adds about 200)
			//#pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
			//#pragma multi_compile _ _MIXED_LIGHTING_SUBTRACTIVE			
			//#pragma multi_compile _ LIGHTMAP_ON

			/* start AtmosphericHeightFog */
//			#pragma multi_compile AHF_NOISEMODE_OFF AHF_NOISEMODE_PROCEDURAL3D
			/* end AtmosphericHeightFog */

			//Defines
			#define SHADERPASS_FORWARD
			#if !defined(_DISABLE_DEPTH_TEX) || defined(_REFRACTION) || defined(_CAUSTICS)
			#define SCREEN_POS
			#endif
			#if !_ADVANCED_SHADING
			#define _SIMPLE_SHADING
			#endif

			#pragma vertex Vertex
			#pragma fragment ForwardPassFragment

			#include "Libraries/Pipeline.hlsl"
			/* start Tesselation */
//			#define TESSELLATION_ON
			/* end Tesselation */
			#include "Libraries/Input.hlsl"

			/* start CurvedWorld */
			//#define CURVEDWORLD_BEND_TYPE_CLASSICRUNNER_X_POSITIVE
			//#define CURVEDWORLD_BEND_ID_1
			//#pragma shader_feature_local CURVEDWORLD_DISABLED_ON
			//#pragma shader_feature_local CURVEDWORLD_NORMAL_TRANSFORMATION_ON
			//#include "Assets/Amazing Assets/Curved World/Shaders/Core/CurvedWorldTransform.cginc"
			/* end CurvedWorld */
			
			#include "Libraries/Common.hlsl"
			#include "Libraries/Fog.hlsl"
			#include "Libraries/Waves.hlsl"
			//Fragment only
			//#pragma multi_compile_local _ _UNDERWATER_ENABLED
			#include "Libraries/UnderwaterShading.hlsl"
			#include "Libraries/Lighting.hlsl"
			#include "Libraries/Features.hlsl"
			#define VERTEX_PASS
			#include "Libraries/Vertex.hlsl"
			#undef VERTEX_PASS
			#include "Libraries/ForwardPass.hlsl"
			
			/* start Tesselation */
//			#include "Libraries/Tesselation.hlsl"
//			#pragma require tessellation tessHW
//			#pragma hull HullFunction
//			#pragma domain DomainFunction
			/* end Tesselation */
			

			Varyings Vertex(Attributes v)
			{
				return LitPassVertex(v);
			}

			ENDHLSL
		}
		
		//Used solely in underwater rendering (prototype)
		Pass
        {
            Name "DepthOnly"
            //Tags{"LightMode" = "DepthOnly"}
            Tags { "LightMode"="UniversalForward" }
            
            ZWrite On
            //ColorMask RG
            Cull Off

            HLSLPROGRAM
            // Required to compile gles 2.0 with standard srp library
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0
            #pragma multi_compile_instancing

            #pragma shader_feature_local _WAVES

            #pragma vertex Vertex
            #pragma fragment DepthOnlyFragment

            #define SHADERPASS_DEPTHONLY

            #include "Libraries/Pipeline.hlsl"
            /* start Tesselation */
//			#define TESSELLATION_ON
            /* end Tesselation */
            #include "Libraries/Input.hlsl"

            /* start CurvedWorld */
//#define CURVEDWORLD_BEND_TYPE_CLASSICRUNNER_X_POSITIVE
//#define CURVEDWORLD_BEND_ID_1
//#pragma shader_feature_local CURVEDWORLD_DISABLED_ON
//#pragma shader_feature_local CURVEDWORLD_NORMAL_TRANSFORMATION_ON
//#include "Assets/Amazing Assets/Curved World/Shaders/Core/CurvedWorldTransform.cginc"
            /* end CurvedWorld */

            //#pragma multi_compile_local _UNDERWATER_ENABLED
			#if defined(_UNDERWATER_ENABLED)
            #define SCREEN_POS
			#include "Libraries/UnderwaterShading.hlsl"
			#endif

            #include "Libraries/Common.hlsl"
            #include "Libraries/Fog.hlsl"
            #include "Libraries/Waves.hlsl"
            #define VERTEX_PASS
            #include "Libraries/Vertex.hlsl"
            #undef VERTEX_PASS

            Varyings Vertex(Attributes v)
            {
                return LitPassVertex(v);
            }

            half4 DepthOnlyFragment(Varyings input, FRONT_FACE_TYPE facing : FRONT_FACE_SEMANTIC) : SV_TARGET
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

            	float depth = input.positionCS.z;

            	#if UNDERWATER_ENABLED
				ClipSurface(input.screenPos / input.screenPos.w, input.positionCS.z, facing);
				#endif

                return float4(depth, facing, 0, 0);
            }

            ENDHLSL

        }
	}

	CustomEditor "StylizedWater2.MaterialUI"
	Fallback "Hidden/InternalErrorShader"	
}