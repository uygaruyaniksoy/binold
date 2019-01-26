using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class WiggleText : MonoBehaviour {

	public Material TextMaterial;
	public float Lower;
	public float Upper;
	public float Rate;
	private float _dilation;

	private void Start() {
		_dilation = Lower;
	}

	void Update () {
		if (_dilation > Upper || _dilation < Lower) Rate *= -1;
		_dilation += Rate;
		TextMaterial.SetFloat("_FaceDilate", _dilation);
		TextMaterial.SetFloat("_OutlineSoftness", Upper - _dilation);
	}
}
