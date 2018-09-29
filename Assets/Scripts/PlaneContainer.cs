using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlaneContainer : MonoBehaviour, IPointerClickHandler {
    private void OnMouseDown() {
        if (Camera.main.GetComponent<PostProcessing>().GlowCamera.GetComponent<Glow>().GlowObject != null) {
            Camera.main.GetComponent<PostProcessing>().GlowCamera.GetComponent<Glow>().GlowObject.GetComponent<Highlightable>().arrow.SetActive(false);
        }
        Camera.main.GetComponent<PostProcessing>().GlowCamera.GetComponent<Glow>().Deactivate();
    }

    public void OnPointerClick(PointerEventData eventData) {
        OnMouseDown();
    }
}
