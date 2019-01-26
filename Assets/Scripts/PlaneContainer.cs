using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlaneContainer : MonoBehaviour, IPointerUpHandler {
    private void OnMouseUp() {
        if (Camera.main.GetComponent<PostProcessing>().GlowCamera.GetComponent<Glow>().GlowObject != null) {
            Camera.main.GetComponent<PostProcessing>().GlowCamera.GetComponent<Glow>().GlowObject.GetComponent<Highlightable>().arrow.SetActive(false);
        }
        Camera.main.GetComponent<PostProcessing>().GlowCamera.GetComponent<Glow>().Deactivate();
    }

    public void OnPointerUp(PointerEventData eventData) {
        OnMouseUp();
    }
}
