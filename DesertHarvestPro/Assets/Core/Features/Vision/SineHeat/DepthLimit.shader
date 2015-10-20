Shader "Custom/DepthLimit" {

Properties {
   _MainTex ("", 2D) = "white" {} //this texture will have the rendered image before post-processing
   _CloseDepth("Close Depth", Range(0.0, 1.0)) = 0.3 
}

SubShader {
Tags { "RenderType"="Opaque" }

Pass{
CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"

sampler2D _CameraDepthTexture;

struct appdata_t {
	  float4 vertex : POSITION;
   half2 texcoord : TEXCOORD0;
   float4 scrPos:TEXCOORD1;
};

struct v2f {
   float4 pos : SV_POSITION;
   half2 texcoord : TEXCOORD0;
   float4 scrPos:TEXCOORD1;
};



//Vertex Shader
v2f vert (appdata_t v){
   v2f o;

   o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
   o.scrPos=ComputeScreenPos(o.pos);
   o.texcoord = v.texcoord;
   //for some reason, the y position of the depth texture comes out inverted
  
   
   	// texcoord.xy stores the distortion texture coordinates.
	//o.texcoord.xy = TRANSFORM_TEX(v.texcoord, _MainTex); // Apply texture tiling and offset.
	//o.texcoord.xy += _Time.gg; //* _IntensityAndScrolling.zw; // Apply texture scrolling.
   
   return o;
}

sampler2D _MainTex; //Reference in Pass is necessary to let us use this variable in shaders
float _CloseDepth;
//Fragment Shader
half4 frag (v2f i) : COLOR{
   // i.scrPos.y = 1 - i.scrPos.y;
   float depthValue = Linear01Depth (tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.scrPos)).r);
   half4 depth;
   float far = 0.9;
   float close = _CloseDepth;
   
   fixed4 orgColor = tex2Dproj(_MainTex, i.scrPos); //Get the orginal rendered color
   //half2 distort = tex2D(_MainTex, i.texcoord.xy).xy;
   
    float2 aux = i.texcoord;
    
    float factor = ( 3.14) / 0.5; 
   
    aux.x = aux.x + (sin(aux.y * factor + _Time.gg * 7) * 0.002);
    aux.y = aux.y + (sin(aux.x * factor + _Time.gg * 7) * 0.001);
   if (depthValue < far && depthValue > close)
   {
    
	   depth.r = depthValue;
	   depth.g = depthValue;
	   depth.b = depthValue;
		depth.a = 1;
		//return float4(aux.x,aux.y, 0, 0);
   		//return depth;
   		return tex2D(_MainTex, aux);
   		//return tex2D(_MainTex, i.texcoord.xy);
   }
	else
	{
	 return orgColor;
	}
	
  
}
ENDCG
}
}
FallBack "Diffuse"
}