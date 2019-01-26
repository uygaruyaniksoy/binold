using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class Arrow : MonoBehaviour {
	public GameObject XAxis;
	public GameObject YAxis;
	public GameObject ZAxis;

	public void UpdateState(bool state) {
		if (state) {
			HideAxis(XAxis);
			ShowAxis(ZAxis);
		} else {
			HideAxis(ZAxis);
			ShowAxis(XAxis);
		}
	}

	private void ShowAxis(GameObject axis) {
		axis.SetActive(true);
	}

	private void HideAxis(GameObject axis) {
		axis.SetActive(false);
	}

	public void HideAll() {
		XAxis.SetActive(false);
		YAxis.SetActive(false);
		ZAxis.SetActive(false);
	}
	
	public void ShowAll() {
		XAxis.SetActive(true);
		YAxis.SetActive(true);
		ZAxis.SetActive(true);
	}

}
