using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelOver : MonoBehaviour {

	public void Animate() {
		for (int i = 0; i < transform.childCount; i++) {
			var child = transform.GetChild(i);
			child.GetComponent<CanvasGroup>().alpha = 0;
			AnimateElement(child, i);
		}
	}

	public void AnimateElement(Transform element, int order) {
		const float delay = .5f;
		StartCoroutine(WaitDelay(element, order * delay));
	}

	private IEnumerator WaitDelay(Transform element, float delay) {
		var start = Time.time;
		var end = start + delay;
		
		while (true) {
			var now = Time.time;
			if (now > end) break; 
			yield return new WaitForFixedUpdate();
			
		}
		StartCoroutine(ChangeAlpha(element));
	}


	private IEnumerator ChangeAlpha(Transform element) {
		while (true) {
			element.GetComponent<CanvasGroup>().alpha += 0.1f;
			if (element.GetComponent<CanvasGroup>().alpha >= 1f) break;
			yield return new WaitForFixedUpdate();
		}
	}
	
}
