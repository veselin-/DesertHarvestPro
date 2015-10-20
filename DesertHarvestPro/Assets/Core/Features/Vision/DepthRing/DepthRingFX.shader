Shader "Custom/DepthRingFX" {

Properties {
   _MainTex ("", 2D) = "white" {} //this texture will have the rendered image before post-processing
   _RingWidth("ring width", Float) = 0.01
   _RingPassTimeLength("ring pass time", Float) = 2.0
}

SubShader {
Tags { "RenderType"="Opaque" }
Pass{
CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"

sampler2D _CameraDepthTexture;
float _StartingTime;
uniform float _RingPassTimeLength; //the length of time it takes the ring to traverse all depth values
uniform float _RingWidth; //width of the ring
float _RunRingPass = 0; //use this as a boolean value, to trigger the ring pass. It is called from the script attached to the camera.

struct v2f {
   float4 pos : SV_POSITION;
   float4 scrPos:TEXCOORD1;
};

//Our Vertex Shader
v2f vert (appdata_base v){
   v2f o;
   o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
   o.scrPos=ComputeScreenPos(o.pos);
  
   return o;
}

sampler2D _MainTex; //Reference in Pass is necessary to let us use this variable in shaders

//Our Fragment Shader
half4 frag (v2f i) : COLOR{

   //extract the value of depth for each screen position from _CameraDepthExture
   float depthValue = Linear01Depth (tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.scrPos)).r);

	
	 i.scrPos.y =  1 - i.scrPos.y;
	

  
   fixed4 orgColor = tex2Dproj(_MainTex, i.scrPos); //Get the orginal rendered color
   float4 newColor; //the color after the ring has passed
   half4 lightRing; //the ring of light that will pass through the dpeth

   float t =  ((_Time.y - _StartingTime)/_RingPassTimeLength );
   float t2 = 1 - ((_Time.y - _StartingTime)/(_RingPassTimeLength) );
   float far = 0.1;
  // float close = 0.4;
   
   //the script attached to the camera will set _RunRingPass to 1 and then will start the ring pass
   if (_RunRingPass == 1){
   	  if(depthValue < far)
	  {
	      //this part draws the light ring
	      if (depthValue < t && depthValue > t - _RingWidth){
	         lightRing.r = (orgColor.r + 0.5)*0.4;
	         lightRing.g = (orgColor.g + 0.2)*0.4;
	         lightRing.b = (orgColor.b)*0.5;
	         lightRing.a = 0.5;
	         return lightRing;
	      } 
	      else 
	      {
      	  
	          if (depthValue > t) {
	             //this part the ring hasn't pass through yet
	             return orgColor;
	          } 
	          else 
	          {
	             //this part the ring has passed through
	             //basically taking the original colors and adding a slight red tint to it.
	             newColor.r = (orgColor.r + 0.5)*0.5;
	             newColor.g = (orgColor.g + 0.2)*0.5;
	             newColor.b = (orgColor.b)*0.5;
	             newColor.a = 0.5;
	             return newColor;
	         }
	      }
      }
      else
	  {
	      return orgColor;
	  }
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