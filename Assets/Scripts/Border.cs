using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using UnityEngine;
using UnityEngine.UI;

public class Border : MonoBehaviour {
	private bool _blinking;

	void Start () {
		var rect = GetComponent<RectTransform>().parent.GetComponent<RectTransform>().rect;
		GetComponent<RectTransform>().sizeDelta = new Vector2(rect.width, rect.height);
	}

	public void SetBlinking(Color firstColor, Color secondColor) {
		_blinking = true;
		StartCoroutine(shiftColor(firstColor, secondColor, true));
	}

	public void StopBlinking() {
		StartCoroutine(shiftColor(GetComponent<Image>().color, Color.black, true));
		_blinking = false;
	}

	private IEnumerator shiftColor(Color fc, Color sc, bool blink = false, float time = 1f) {
		var start = Time.time;
		var end = Time.time + time;

		while (true) {
			var now = Time.time;
			var color = Color.Lerp(fc, sc, (now - start) / (end - start));

			GetComponent<Image>().color = color;			
			if (now > end) break;

			yield return new WaitForFixedUpdate();
		}
		if (blink && _blinking) StartCoroutine(shiftColor(sc, fc, true));
	}
	
	
}
