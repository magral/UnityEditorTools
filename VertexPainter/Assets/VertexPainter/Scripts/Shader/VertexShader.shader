Shader "Unlit/VertexShader"
{
	Properties{
		_Color("Main Color", Color) = (1,1,1,1)
		_SpecColor("Spec Color", Color) = (1,1,1,0)
		_Emission("Emmisive Color", Color) = (0,0,0,0)
		_Shininess("Shininess", Range(0.01, 1)) = 0.7
		_MainTex("Base (RGB) Trans (A)", 2D) = "white" {}
		_Cutoff("Alpha cutoff", Range(0,1)) = 0.5
	}

		SubShader{
			ZWrite on
			Alphatest Greater[_Cutoff]
			Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
			Blend SrcAlpha OneMinusSrcAlpha
			ColorMask RGB
			Pass{
				Material{
				Shininess[_Shininess]
				Specular[_SpecColor]
				Emission[_Emission]
			}

			ColorMaterial Emission
			Lighting on
			SeparateSpecular on

			SetTexture[_MainTex]{
				Combine texture * primary, texture * primary
			}
			SetTexture[_MainTex]{
				constantColor[_Color]
				Combine previous * constant DOUBLE, previous * constant
			}
		}
	}
}
