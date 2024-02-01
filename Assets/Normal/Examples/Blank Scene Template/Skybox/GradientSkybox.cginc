#ifndef NORMAL_GRADIENT_SKYBOX
#define NORMAL_GRADIENT_SKYBOX

#include "HLSLSupport.cginc"

struct GradientSkyboxSettings {
	fixed3 ColorT;
	fixed3 ColorM;
	fixed3 ColorB;
	
	half ExponentT;
	half ExponentB;
	half Intensity;
};

fixed3 ScreenSpaceDither(float2 screenpos)
{
	float3 dither = dot(float2(171.0, 231.0), screenpos + _Time.yy).xxx;
	dither.rgb = frac(dither / float3(103.0, 71.0, 97.0)) - float3(0.5, 0.5, 0.5);
	return (dither / 255.0);
}

fixed3 GradientSkybox(GradientSkyboxSettings settings, float3 dir) {
	float3 n = normalize(dir);
	
	float factorT = 1.0 - pow(min(1.0, 1.0 - n.y), settings.ExponentT);
	float factorB = 1.0 - pow(min(1.0, 1.0 + n.y), settings.ExponentB);
	float factorM = 1.0 - factorT - factorB;
	
	return (settings.ColorT * factorT + settings.ColorM * factorM + settings.ColorB * factorB) * settings.Intensity;
}

#endif
