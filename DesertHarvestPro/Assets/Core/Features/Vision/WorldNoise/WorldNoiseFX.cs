using UnityEngine;
using System.Collections;

public class WorldNoiseFX : MonoBehaviour {

	public Material mat;



	// Called by the camera to apply the image effect
	void OnRenderImage (RenderTexture source, RenderTexture destination){

		mat.SetMatrix("_ViewProjectInverse", (GetComponent<Camera>().projectionMatrix * GetComponent<Camera>().worldToCameraMatrix).inverse);

		//mat is the material containing your shader
		Graphics.Blit(source,destination,mat);
	}

	/*static void CustomGraphicsBlit (RenderTexture source, RenderTexture dest, Material fxMaterial, Material passNr) {
		RenderTexture.active = dest;
		       
		fxMaterial.SetTexture ("_MainTex", source);	        
	        	        
		GL.PushMatrix ();
		GL.LoadOrtho ();	
	    	

		
	    GL.Begin (GL.QUADS);
							
		GL.MultiTexCoord2 (0, 0.0f, 0.0f); 
		GL.Vertex3 (0.0f, 0.0f, 3.0f); // BL
		
		GL.MultiTexCoord2 (0, 1.0f, 0.0f); 
		GL.Vertex3 (1.0f, 0.0f, 2.0f); // BR
		
		GL.MultiTexCoord2 (0, 1.0f, 1.0f); 
		GL.Vertex3 (1.0f, 1.0f, 1.0f); // TR
		
		GL.MultiTexCoord2 (0, 0.0f, 1.0f); 
		GL.Vertex3 (0.0f, 1.0f, 0.0f); // TL
		
		GL.End ();
	    GL.PopMatrix ();
	}	

	void UpdateCameraMatrix(){
		var CAMERA_NEAR = camera.nearClipPlane;
		var CAMERA_FAR = camera.farClipPlane;
		var CAMERA_FOV = camera.fieldOfView;
		var CAMERA_ASPECT_RATIO = camera.aspect;
		
		Matrix4x4 frustumCorners = Matrix4x4.identity;		

		float fovWHalf  = CAMERA_FOV * 0.5f;
		
		Vector3 toRight  = camera.transform.right * CAMERA_NEAR * Mathf.Tan (fovWHalf * Mathf.Deg2Rad) * CAMERA_ASPECT_RATIO;
		Vector3 toTop  = camera.transform.up * CAMERA_NEAR * Mathf.Tan (fovWHalf * Mathf.Deg2Rad);
		
		Vector3 topLeft  = (camera.transform.forward * CAMERA_NEAR - toRight + toTop);
		float CAMERA_SCALE  = topLeft.magnitude * CAMERA_FAR/CAMERA_NEAR;	
		
		topLeft.Normalize();
		topLeft *= CAMERA_SCALE;
		
		Vector3 topRight = (camera.transform.forward * CAMERA_NEAR + toRight + toTop);
		topRight.Normalize();
		topRight *= CAMERA_SCALE;
		
		Vector3 bottomRight  = (camera.transform.forward * CAMERA_NEAR + toRight - toTop);
		bottomRight.Normalize();
		bottomRight *= CAMERA_SCALE;
		
		Vector3 bottomLeft  = (camera.transform.forward * CAMERA_NEAR - toRight - toTop);
		bottomLeft.Normalize();
		bottomLeft *= CAMERA_SCALE;
		
		frustumCorners.SetRow (0, topLeft); 
		frustumCorners.SetRow (1, topRight);		
		frustumCorners.SetRow (2, bottomRight);
		frustumCorners.SetRow (3, bottomLeft);		
		
		mat.SetMatrix ("_FrustumCornersWS", frustumCorners);
		mat.SetVector ("_CameraWS", camera.transform.position);
	}*/
}
