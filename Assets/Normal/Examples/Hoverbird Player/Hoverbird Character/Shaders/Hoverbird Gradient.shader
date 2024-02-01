Shader "Normal/Examples/Hoverbird Gradient" {
	Properties {
		_MainTex("Base (RGB)", 2D) = "white" {}
		[NoScaleOffset] _ColorTex("Color Gradient", 2D) = "white" {}
		_ColorSpeed("Color Speed", Float) = 0.1
	}
	SubShader {
		Tags { "RenderType" = "Opaque" }

        CGPROGRAM
		#pragma surface surf Lambert addshadow fullforwardshadows 

		struct Input {
        	float2 uv_MainTex;
		};

        sampler2D _MainTex;
		sampler2D _ColorTex;
        
		uniform float _ColorSpeed;

		void surf(Input i , inout SurfaceOutput o) {
			half3 color = tex2D(_ColorTex, float2(_ColorSpeed * _Time.y, 0.5)).rgb;
			o.Albedo = tex2D(_MainTex, i.uv_MainTex).rgb * color;
		}

		ENDCG
	}
	Fallback "Diffuse"
}
