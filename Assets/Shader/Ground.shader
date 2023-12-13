Shader "URP/Unlit/Ground"
{
    Properties
    { 
        _BaseMap1("Texture 1 Use Normal", 2D) = "black" {}
        _BaseMap2("Texture 2", 2D) = "black" {}
        _BaseMap3("Texture 3", 2D) = "black" {}
        _BaseMap4("Texture 4", 2D) = "black" {}
        _BumpMap1 ("Normal Texture 1", 2D) = "bump"{}
        _NoiseMap("Noise Texture", 2D) = "white" {}
        _BaseColor("BaseColor", Color) = (1,1,1,1)
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" "Queue"="Geometry" "RenderPipeline" = "UniversalPipeline" }
        LOD 100

        Pass
        {
            Name  "Forward"
            Tags {"LightMode" = "UniversalForward"}

            HLSLPROGRAM
            #pragma target 4.5
            #pragma exclude_renderers gles d3d9
            
            #pragma vertex vert
            #pragma fragment frag

            //#pragma shader_feature_local _NORMALMAP
            //#pragma shader_feature_local _PARALLAXMAP
            #pragma shader_feature_local _RECEIVE_SHADOWS_OFF
            //#pragma shader_feature_local _ _DETAIL_MULX2 _DETAIL_SCALED
            //#pragma shader_feature_local_fragment _SURFACE_TYPE_TRANSPARENT
            #pragma shader_feature_local_fragment _ALPHATEST_ON
            #pragma shader_feature_local_fragment _ _ALPHAPREMULTIPLY_ON _ALPHAMODULATE_ON
            #pragma shader_feature_local_fragment _EMISSION
            //#pragma shader_feature_local_fragment _METALLICSPECGLOSSMAP
            //#pragma shader_feature_local_fragment _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
            //#pragma shader_feature_local_fragment _OCCLUSIONMAP
            //#pragma shader_feature_local_fragment _SPECULARHIGHLIGHTS_OFF
            //#pragma shader_feature_local_fragment _ENVIRONMENTREFLECTIONS_OFF
            //#pragma shader_feature_local_fragment _SPECULAR_SETUP

            // -------------------------------------
            // Universal Pipeline keywords
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
            #pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
            //#pragma multi_compile _ EVALUATE_SH_MIXED EVALUATE_SH_VERTEX
            #pragma multi_compile_fragment _ _ADDITIONAL_LIGHT_SHADOWS
            //#pragma multi_compile_fragment _ _REFLECTION_PROBE_BLENDING
            //#pragma multi_compile_fragment _ _REFLECTION_PROBE_BOX_PROJECTION
            #pragma multi_compile_fragment _ _SHADOWS_SOFT
            //#pragma multi_compile_fragment _ _SCREEN_SPACE_OCCLUSION
            //#pragma multi_compile_fragment _ _DBUFFER_MRT1 _DBUFFER_MRT2 _DBUFFER_MRT3
            #pragma multi_compile_fragment _ _LIGHT_LAYERS
            //#pragma multi_compile_fragment _ _LIGHT_COOKIES
            //#pragma multi_compile _ _FORWARD_PLUS
            //#include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/RenderingLayers.hlsl"


            // -------------------------------------
            // Unity defined keywords
            //#pragma multi_compile _ LIGHTMAP_SHADOW_MIXING
            #pragma multi_compile _ SHADOWS_SHADOWMASK
            //#pragma multi_compile _ DIRLIGHTMAP_COMBINED
            //#pragma multi_compile _ LIGHTMAP_ON
            //#pragma multi_compile _ DYNAMICLIGHTMAP_ON
            #pragma multi_compile_fragment _ LOD_FADE_CROSSFADE
            #pragma multi_compile_fog
            #pragma multi_compile_fragment _ DEBUG_DISPLAY

            //--------------------------------------
            // GPU Instancing
            #pragma multi_compile_instancing
            //#include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DOTS.hlsl"


            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl" 
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DBuffer.hlsl"

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseMap1_ST;
                float4 _BaseMap2_ST;
                float4 _BaseMap3_ST;
                float4 _BaseMap4_ST;
                float4 _BumpMap1_ST;
                float4 _NoiseMap_ST;
                half4 _BaseColor;
            CBUFFER_END

            TEXTURE2D(_BaseMap1);        SAMPLER(sampler_BaseMap1);
            TEXTURE2D(_BaseMap2);        SAMPLER(sampler_BaseMap2);
            TEXTURE2D(_BaseMap3);        SAMPLER(sampler_BaseMap3);
            TEXTURE2D(_BaseMap4);        SAMPLER(sampler_BaseMap4);
            TEXTURE2D(_BumpMap1);        SAMPLER(sampler_BumpMap1);
            TEXTURE2D(_NoiseMap);        SAMPLER(sampler_NoiseMap);

            UNITY_INSTANCING_BUFFER_START(Props)
            UNITY_INSTANCING_BUFFER_END(Props)

            struct Attributes
            {
                float4 positionOS   : POSITION;
                float4 color        : COLOR;
                float2 uv           : TEXCOORD0;
                float3 normalOS     : NORMAL;
                float4 tangentOS      : TANGENT;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float4 positionCS   : SV_POSITION;
                float4 color        : COLOR;
                float2 uv           : TEXCOORD0;
                
                float3 positionWS   : TEXCOORD1;
                float fogCoord      : TEXCOORD2;
                
                float3 T : TEXCOORD3;
                float3 B : TEXCOORD4;
                float3 N     : TEXCOORD5;
                //float4 shadowCoord   : TEXCOORD2;

                UNITY_VERTEX_OUTPUT_STEREO
            }; 
            
            Varyings vert(Attributes input)
            {
                Varyings output;
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);
                VertexNormalInputs normal = GetVertexNormalInputs(input.normalOS.xyz, input.tangentOS);

                output.positionWS = TransformObjectToWorld(input.positionOS.xyz);
                output.positionCS = TransformWorldToHClip(output.positionWS.xyz);

                output.uv = input.uv;
                output.color = input.color;
                output.N = normalize(normal.normalWS);
                output.T = normalize(normal.tangentWS);
                output.B = normalize(normal.bitangentWS);

                output.fogCoord = ComputeFogFactor(output.positionCS.z);
                return output;
            }

            void RenderDecal(float4 positionCS, inout float3 baseColor, inout half3 normalWS)
            {
                half3 spec = 0;
                half metalic = 0;
                half occlusion = 0;
                half smooth = 0;
                ApplyDecal(positionCS, baseColor.rgb, spec, normalWS, metalic, occlusion, smooth);
            }

            half4 frag(Varyings input) : SV_Target
            {
                float4 shadowCoord = TransformWorldToShadowCoord(input.positionWS);
                Light light = GetMainLight(shadowCoord);

                float3x3 TBN = CreateTangentToWorld(input.N.xyz, input.T.xyz, input.B.xyz);
                float3 tangentNormal = UnpackNormal(SAMPLE_TEXTURE2D(_BumpMap1, sampler_BumpMap1, TRANSFORM_TEX(input.uv, _BumpMap1)));
                float3 worldNormal = normalize(TransformTangentToWorld(tangentNormal, TBN));

                float NdotL = dot(worldNormal, light.direction) * light.shadowAttenuation;

                half4 tex1 = SAMPLE_TEXTURE2D(_BaseMap1, sampler_BaseMap1, TRANSFORM_TEX(input.uv, _BaseMap1));
                half4 tex2 = SAMPLE_TEXTURE2D(_BaseMap2, sampler_BaseMap2, TRANSFORM_TEX(input.uv, _BaseMap2));
                half4 tex3 = SAMPLE_TEXTURE2D(_BaseMap3, sampler_BaseMap3, TRANSFORM_TEX(input.uv, _BaseMap3));
                half Noise = SAMPLE_TEXTURE2D(_NoiseMap, sampler_NoiseMap, input.uv.xy * _NoiseMap_ST.xy + (_BaseMap4_ST.zw + (-_Time.y * 0.01))).g;
                float2 WaterUV = input.uv.xy * (_BaseMap4_ST.xy + Noise) + (_BaseMap4_ST.zw + (_Time.y * 0.02));
                half4 tex4 = SAMPLE_TEXTURE2D(_BaseMap4, sampler_BaseMap4, WaterUV);
                
                tex1 *= NdotL;

                half4 finalColor = lerp(tex1, tex2, input.color.r); //R 마스킹
                finalColor = lerp(finalColor, tex3, input.color.g); //G 마스킹
                finalColor = lerp(finalColor, tex4, input.color.b); //B 마스킹
                finalColor.rgb *= _BaseColor.rgb * light.color.rgb;
                finalColor.rgb = MixFog(finalColor.rgb, input.fogCoord);
                RenderDecal(input.positionCS, finalColor.rgb, input.N.xyz);
                return finalColor;
            }
            
            ENDHLSL
        }

         Pass
        {
            Name "ShadowCaster"
            Tags
            {
                "LightMode" = "ShadowCaster"
            }

            // -------------------------------------
            // Render State Commands
            ZWrite On
            ZTest LEqual
            ColorMask 0
            Cull[_Cull]

            HLSLPROGRAM
            #pragma target 2.0

            // -------------------------------------
            // Shader Stages
            #pragma vertex ShadowPassVertex
            #pragma fragment ShadowPassFragment

            // -------------------------------------
            // Material Keywords
            #pragma shader_feature_local_fragment _ALPHATEST_ON
            #pragma shader_feature_local_fragment _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A

            //--------------------------------------
            // GPU Instancing
            #pragma multi_compile_instancing
            //#include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DOTS.hlsl"

            // -------------------------------------
            // Universal Pipeline keywords

            // -------------------------------------
            // Unity defined keywords
            #pragma multi_compile_fragment _ LOD_FADE_CROSSFADE

            // This is used during shadow map generation to differentiate between directional and punctual light shadows, as they use different formulas to apply Normal Bias
            #pragma multi_compile_vertex _ _CASTING_PUNCTUAL_LIGHT_SHADOW

            // -------------------------------------
            // Includes
            #include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/ShadowCasterPass.hlsl"
            ENDHLSL
        }

        Pass
        {
            Name "DepthOnly"
            Tags
            {
                "LightMode" = "DepthOnly"
            }

            // -------------------------------------
            // Render State Commands
            ZWrite On
            ColorMask R
            Cull[_Cull]

            HLSLPROGRAM
            #pragma target 2.0

            // -------------------------------------
            // Shader Stages
            #pragma vertex DepthOnlyVertex
            #pragma fragment DepthOnlyFragment

            // -------------------------------------
            // Material Keywords
            #pragma shader_feature_local_fragment _ALPHATEST_ON
            #pragma shader_feature_local_fragment _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A

            // -------------------------------------
            // Unity defined keywords
            #pragma multi_compile_fragment _ LOD_FADE_CROSSFADE

            //--------------------------------------
            // GPU Instancing
            #pragma multi_compile_instancing
            //#include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DOTS.hlsl"

            // -------------------------------------
            // Includes
            #include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/DepthOnlyPass.hlsl"
            ENDHLSL
        }

        Pass
        {
            Name "DepthNormals"
            Tags
            {
                "LightMode" = "DepthNormals"
            }

            // -------------------------------------
            // Render State Commands
            ZWrite On
            ColorMask R
            Cull[_Cull]

            HLSLPROGRAM
            #pragma target 2.0

            // -------------------------------------
            // Shader Stages
            #pragma vertex DepthNormalsVertex
            #pragma fragment DepthNormalsFragment

            // -------------------------------------
            // Includes
            #include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/LitDepthNormalsPass.hlsl"
            ENDHLSL
        }

    }
}

