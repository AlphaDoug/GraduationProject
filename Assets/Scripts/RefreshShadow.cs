using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefreshShadow : MonoBehaviour {

	// Use this for initialization
	public RenderTexture texture;
	public Material material;
	void Start () {
		
	}
	void Update()
	{
		
		
	}
	// Update is called once per frame
	void FixedUpdate () {
	
	}

	/// <summary>
	/// OnRenderImage is called after all rendering is complete to render image.
	/// </summary>
	/// <param name="src">The source RenderTexture.</param>
	/// <param name="dest">The destination RenderTexture.</param>

	void OnRenderImage(RenderTexture src, RenderTexture dest)
	{
		
		Graphics.Blit(src, dest, material);
			Graphics.Blit(src, texture, material);
			//Debug.Log(123);
	}
	/// <summary>
	/// OnPreRender is called before a camera starts rendering the scene.
	/// </summary>
	void OnPreRender()
	{
		
	}
	/// <summary>
	/// OnPreCull is called before a camera culls the scene.
	/// </summary>
	void OnPreCull()
	{
		
	}
	/// <summary>
	/// OnPostRender is called after a camera finishes rendering the scene.
	/// </summary>
	void OnPostRender()
	{
	
	}
	/// <summary>
	/// OnWillRenderObject is called for each camera if the object is visible.
	/// </summary>


	
}
