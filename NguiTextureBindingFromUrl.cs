using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
[AddComponentMenu("NGUI/NData/Texture Binding From Url")]
public class NguiTextureBindingFromUrl : NguiBinding
{
	// Base on TextBinding
	
	private readonly Dictionary<Type, EZData.Property> _properties = new Dictionary<Type, EZData.Property>();
	private UITexture _uiTexture;
	public string textureUrl = null;
	public int hack_x = 0;
	public int hack_y = 0;
	
	
	protected override void Unbind()
	{
		base.Unbind();
		
		foreach(var p in _properties)
		{
			p.Value.OnChange -= OnChange;
		}
		_properties.Clear();
	}
	
	protected override void Bind()
	{
		base.Bind();
		
		FillTextProperties(_properties, Path);
		
		foreach(var p in _properties)
		{
			p.Value.OnChange += OnChange;
		}
	}

	
	protected override void OnChange()
	{
		base.OnChange();
		textureUrl = (string)GetTextValue(_properties);
	}
	
	public override void Start()
	{	
		base.Start();
		StartLoadingTexture();
	}
	
	private void StartLoadingTexture ()
	{
		if (!string.IsNullOrEmpty(textureUrl)) 
		{
			_uiTexture = gameObject.GetComponent<UITexture>();
		
			if (!_uiTexture) {
			 	_uiTexture = gameObject.AddComponent<UITexture>();
				//Material mat = new Material(Shader.Find("Unlit/Transparent Colored (AlphaClip)"));
				Material mat = new Material(Shader.Find("Unlit/Transparent Colored"));
				_uiTexture.material = mat;
			}
			
			_uiTexture.MakePixelPerfect();
				
			this.LoadTexture(textureUrl, (texture) => {
				_uiTexture.mainTexture = texture;
				_uiTexture.MakePixelPerfect();
				
				if ((hack_x>0) && (hack_y>0))
				{
					gameObject.transform.localScale = new Vector3 (hack_x, hack_y, gameObject.transform.localScale.z);
				}
					
					/* Scale images if not retina */
					/*
					if (!AppGUISettings.IS_RETINA)
						gameObject.transform.localScale = new Vector3 (gameObject.transform.localScale.x/2, gameObject.transform.localScale.y/2, gameObject.transform.localScale.z);
						*/
			});
		}
	}
	
	private IEnumerator LoadTexture (WWW www, Action<Texture2D> callback)
	{
		yield return www;
		
		if (string.IsNullOrEmpty(www.error)) {
			Texture2D tex = new Texture2D(4, 4, TextureFormat.RGB24, false);
			www.LoadImageIntoTexture(tex);
			callback(tex);
		} else {
			callback(null);
		}
	}
	
	private void LoadTexture (string url, Action<Texture2D> callback)
	{
		StartCoroutine (LoadTexture (new WWW (url), callback));
	}
}
