using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class RightPanelExistenceBinding : NguiBooleanBinding
{
	public GameObject ContentPrefab;
	private GameObject _content;
	public int transformZ = 0;
	
	protected override void ApplyNewValue (bool val)
	{
		if (val && _content == null) {
			_content = GameObject.Instantiate (ContentPrefab) as GameObject;
			_content.transform.parent = transform;
			_content.transform.localScale = new Vector3(1,1,1);
			
			_content.GetComponent<UIAnchor>().panelContainer = gameObject.GetComponent<UIPanel>();
			_content.transform.localPosition = new Vector3(_content.transform.localPosition.x, _content.transform.localPosition.y, transformZ);
			
			foreach (var b in _content.GetComponentsInChildren<NguiBinding>())
				b.UpdateBinding ();
		}
		if (!val && _content != null) {
			
			// Delete all textures before destroying game object
			UITexture[] textureList = _content.GetComponentsInChildren<UITexture>();
			foreach(UITexture _texture in textureList)
			{
				DestroyImmediate(_texture.material.mainTexture, true);
			}	
			
			GameObject.Destroy (_content);
			_content = null;
		}
	}
	
}