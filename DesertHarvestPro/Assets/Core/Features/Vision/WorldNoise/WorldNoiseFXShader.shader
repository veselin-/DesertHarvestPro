Shader "Custom/WorldNoise" {
Properties {
 _MainTex ("", 2D) = "white" {}
}
 
SubShader {
 
ZTest Always Cull Off ZWrite Off Fog { Mode Off } //Rendering settings
 
 Pass{
  CGPROGRAM
  #pragma vertex vert
  #pragma fragment frag
  #include "UnityCG.cginc" 
  //we include "UnityCG.cginc" to use the appdata_img struct
    
  struct v2f {
   float4 pos : POSITION;
   half2 uv : TEXCOORD0;
  };
   
  //Our Vertex Shader 
  v2f vert (appdata_img v){
   v2f o;
   o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
   o.uv = MultiplyUV (UNITY_MATRIX_TEXTURE0, v.texcoord.xy);
		
	#if UNITY_UV_STARTS_AT_TOP
	if (_MainTex_TexelSize.y < 0)
		o.uv.y = 1-o.uv.y;
	#endif				
	
   return o; 
  }
    
  sampler2D _MainTex; //Reference in Pass is necessary to let us use this variable in shaders
    
  sampler2D _CameraDepthTexture;
  float4x4 _ViewProjectInverse;  
    
    float3 compute_world_space(v2f i){
    	// Returns World Position of a pixel from clip-space depth map..
 
		float depth = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture,i.uv.xy);	
		// H is the viewport position at this pixel in the range -1 to 1.
		float4 H = float4((i.uv.x) * 2 - 1, (i.uv.y) * 2 - 1, depth * 2 - 1, 1.0);
		float4 D = mul(_ViewProjectInverse, H);
		return float3(D.xyz / D.w);	
    }
    
float hash( float n )
{
    return frac(sin(n)*43758.5453);
}
 
 // http://forum.unity3d.com/threads/perlin-noise-procedural-shader.33725/
float noise( float3 x )
{
    // The noise function returns a value in the range -1.0f -> 1.0f
 
    float3 p = floor(x);
    float3 f = frac(x);
 
    f       = f*f*(3.0-2.0*f);
    float n = p.x + p.y*57.0 + 113.0*p.z;
 
    return lerp(lerp(lerp( hash(n+0.0), hash(n+1.0),f.x),
                   lerp( hash(n+57.0), hash(n+58.0),f.x),f.y),
               lerp(lerp( hash(n+113.0), hash(n+114.0),f.x),
                   lerp( hash(n+170.0), hash(n+171.0),f.x),f.y),f.z);
}
    
  //Our Fragment Shader
  fixed4 frag (v2f i) : COLOR{
  	float3 wsPos = compute_world_space(i);
   		 
	fixed4 orgCol = tex2D(_MainTex, i.uv); //Get the orginal rendered color 
	
   	//fixed4 col = fixed4(orgCol.xyz * sin(wsPos.y * 5), 1);
     //fixed4 col = fixed4(orgCol.xyz + noise(wsPos.xyz * 0.3)*2.5,1);
     //orgCol.x = noise(wsPos.x * 0.1)
     fixed4 col = fixed4(orgCol.xyz + noise((wsPos.xyz)*0.1) * (1/(wsPos.y + 1.8)),1);
     //fixed4 col = orgCol;
   return col;
  }
  
  
  ENDCG
 }
} 
 FallBack "Diffuse"
}