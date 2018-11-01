using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class WiggleImage : MonoBehaviour {
	public Color Color1;
	public Color Color2;

	public Vector3 rot1;
	public Vector3 rot2;

	public Vector3 Size1;
	public Vector3 Size2;

	private float _curTime; 
	public float DeltaTime = 0.1f; 
	
	void Update () {
		if (_curTime < 0 || _curTime > 1) DeltaTime *= -1;

		GetComponent<Image>().color = Color.Lerp(Color1, Color2, _curTime);
		
		transform.rotation = Quaternion.Lerp(Quaternion.Euler(rot1), 
			Quaternion.Euler(rot2),
			_curTime);
		transform.localScale = Vector3.Lerp(Size1, Size2, _curTime);
		
		_curTime += DeltaTime;
	}
}
