using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class Axis : MonoBehaviour, IPointerClickHandler, SwipeHandler.Handler {
	public Vector3 MoveFactor;
	private Camera _camera;
	private Transform _greatParent;
	private bool _lock;
	private bool _vertical;
	private Game _game;

	private void Start() {
		_game = GameObject.Find("Game").GetComponent<Game>();
		_camera = Camera.main;
		_greatParent = transform.parent.parent;
		_vertical = MoveFactor.y > 0;
	}

	private void OnMouseDown() {
		_lock = true;
		_camera.GetComponent<RotateCamera>().Lock = true;
	}

	private void OnMouseUp() {
		_lock = false;
		_camera.GetComponent<RotateCamera>().Lock = false;
		_camera.GetComponent<RotateCamera>().CheckSideValidity();
		if (_game.CheckValidity()) {
			_game.HandleLevelOver();
		}
		
	}

	public void OnPointerClick(PointerEventData eventData) {
		OnMouseDown();
	}


	public void LeftSwipeHandler() {
		if (!_vertical && _lock)
			_greatParent.position -= MoveFactor * (int)Mathf.Abs(GetComponent<SwipeHandler>().GetDelta().x);
	}

	public void RightSwipeHandler() {
		if (!_vertical && _lock)
		_greatParent.position += MoveFactor * (int)Mathf.Abs(GetComponent<SwipeHandler>().GetDelta().x);
	}

	public void DownSwipeHandler() {
		if (_vertical && _lock)
			_greatParent.position -= MoveFactor * (int)Mathf.Abs(GetComponent<SwipeHandler>().GetDelta().y);
	}

	public void UpSwipeHandler() {
		if (_vertical && _lock)
			_greatParent.position += MoveFactor * (int)Mathf.Abs(GetComponent<SwipeHandler>().GetDelta().y);
	}
}
