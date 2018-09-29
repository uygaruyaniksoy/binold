using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Glow : MonoBehaviour {
	public GameObject GlowObject;
	[Range(0, 20)]
	public int CullingMask = 9;

	public Material NormalMaterial;
	public Material SelectedMaterial;

	public void Deactivate() {
		if (GlowObject == null) return;
		GlowObject.GetComponent<Renderer>().materials = 
			GlowObject.GetComponent<Renderer>().materials.Take(1).Concat(new[] {NormalMaterial}).ToArray();
		GlowObject.gameObject.layer = 0;
		for (var i = 0; i < GlowObject.transform.childCount; i++) {
			SetChildrenLayer(GlowObject.transform.GetChild(i), 0);
		}
		GlowObject = null;
	}

	public void Activate(GameObject glowObject) {
		if (GlowObject != null) SetChildrenLayer(GlowObject.transform, 0);
		GlowObject = glowObject;
		GlowObject.GetComponent<Renderer>().materials =
			GlowObject.GetComponent<Renderer>().materials.Take(1).Concat(new[] {SelectedMaterial}).ToArray();
		GetComponent<Camera>().cullingMask = 1 << CullingMask;
		SetChildrenLayer(GlowObject.transform, CullingMask);
	}

	private void SetChildrenLayer(Transform glowObject, int cullingMask) {
		glowObject.gameObject.layer = cullingMask;
		for (var i = 0; i < glowObject.childCount; i++) {
			SetChildrenLayer(glowObject.GetChild(i), cullingMask);
		}
	}
}
