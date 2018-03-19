using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ShadowMapCaculate{
public class ShadowCaculate : MonoBehaviour {


	[SerializeField]
	private Transform shadowCheckPoint;


	public RenderTexture depthTexture;
	[HideInInspector]
	public Camera shadowCamera;

	public bool isInShadow;

	Texture2D tex;

	void Start () {
		
		depthTexture.height=Screen.height;
		depthTexture.width=Screen.width;
		shadowCamera=   CameraForShadow.shadowCamera.GetComponent<Camera>();
		tex=getTexture2d(depthTexture);
	
		//player=transform;
		//print(texture.IsCreated());
	}
	void Update()
	{
		
		//Vector3 pos =shadowCamera.WorldToViewportPoint(player.position);
		Vector4 Pwolrd=new Vector4(shadowCheckPoint.position.x,shadowCheckPoint.position.y,shadowCheckPoint.position.z,1);

		Vector4 Pcamera=shadowCamera.worldToCameraMatrix*Pwolrd;


		//Debug.Log(Pcamera+"camera");
		
		Vector3 Pview3=shadowCamera.WorldToViewportPoint(shadowCheckPoint.position);
		Vector4 Pview=new Vector4(Pview3.x,Pview3.y,Pview3.z,1);
	

		Vector4 pos= shadowCamera.projectionMatrix*Pcamera;
		//Debug.Log(pos);
		double depth=pos.z/pos.w;
		


		
		//Debug.Log(shadowCamera.worldToCameraMatrix);
		//pos=new Vector3(pos.x*Screen.width,pos.y*Screen.height,pos.z);
		
		
  
		 int width = depthTexture.width;
        int height = depthTexture.height;
		Vector3 screenpos=new Vector3(pos.x/pos.w*width/2+width/2,pos.y/pos.w*height/2+height/2,pos.z/pos.w*0.5f+0.5f);
		//Debug.Log(screenpos.z*10+"screen");


        RenderTexture.active = depthTexture;
        tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        tex.Apply();
		
		Color color= tex.GetPixel((int) screenpos.x,(int) screenpos.y);
		float sample=color.r;



		if((screenpos.z-sample)>0.003f)isInShadow=true;
		else isInShadow=false;

		test1=screenpos.z;
		test2=sample;
		//Debug.Log(isInShadow);
		//Debug.Log((sample)*10);

	}

	float test1;
	float test2;
  public Texture2D getTexture2d(RenderTexture renderT)
    {
        if (renderT == null)
            return null;

        int width = renderT.width;
        int height = renderT.height;
        Texture2D tex2d = new Texture2D(width, height, TextureFormat.ARGB32, false);
        RenderTexture.active = renderT;
        tex2d.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        tex2d.Apply();

        //byte[] b = tex2d.EncodeToPNG();
        //Destroy(tex2d); 

        //File.WriteAllBytes(Application.dataPath + "1.jpg", b); 
        return tex2d;
    }

	
	/// <summary>
	/// OnGUI is called for rendering and handling GUI events.
	/// This function can be called multiple times per frame (one call per event).
	/// </summary>
	
	
	// Update is called once per frame

}
}