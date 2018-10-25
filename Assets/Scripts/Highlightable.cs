using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Highlightable : MonoBehaviour, IPointerClickHandler {
    public GameObject arrow;
    
    public void OnMouseDown() {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        var glowObject = Camera.main.transform.GetChild(1).GetComponent<Glow>().GlowObject;
        if (glowObject != null) {
            Camera.main.transform.GetChild(1).GetComponent<Glow>().Deactivate();
            glowObject.GetComponent<Highlightable>().arrow.SetActive(false);
        }
        Camera.main.transform.GetChild(1).GetComponent<Glow>().Activate(gameObject);
        arrow.SetActive(true);
        var state = Camera.main.GetComponent<RotateCamera>().State;
        arrow.GetComponent<Arrow>().UpdateState(state);
    }

    public void OnPointerClick(PointerEventData eventData) {
        OnMouseDown();
    }
}
