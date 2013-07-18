using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
[AddComponentMenu("NGUI/NData/ExistenceBinding")]
public class ExistenceBinding : NguiBooleanBinding
{
	public GameObject ContentPrefab;
	private GameObject _content;
	
	protected override void ApplyNewValue (bool val)
	{
		if (val && _content == null) {
			_content = GameObject.Instantiate (ContentPrefab) as GameObject;
			_content.transform.parent = transform;
			_content.transform.localScale = new Vector3(1,1,1);
			foreach (var b in _content.GetComponentsInChildren<NguiBinding>())
				b.UpdateBinding ();
		}
		if (!val && _content != null) {
			
			// Delete all texture before destroy game object
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