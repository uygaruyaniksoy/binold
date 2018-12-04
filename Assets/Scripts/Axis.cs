using System.Collections;
using System.Collections.Generic;
using System.Xml.Xsl;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class Axis : MonoBehaviour, IPointerClickHandler, SwipeHandler.Handler {
	private const int MoveFactor = 1;
	public Vector3 MoveVector;
	private Camera _camera;
	private Transform _greatParent;
	private bool _lock;
	private bool _vertical;
	private Game _game;
	private int _counter;

	private void Start() {
		_game = GameObject.Find("Game").GetComponent<Game>();
		_camera = Camera.main;
		_greatParent = transform.parent.parent;
		_vertical = MoveVector.y > 0;
	}

	private void OnMouseDown() {
		_lock = true;
		_camera.GetComponent<RotateCamera>().Lock = true;
	}

	private void OnMouseUp() {
		_lock = false;
		_camera.GetComponent<RotateCamera>().Lock = false;
		_camera.GetComponent<RotateCamera>().CheckSideValidity();
		// TODO: fix here
		int gridCountPerMeter = 8;
		_greatParent.position = new Vector3(
				(float) (Mathf.Round(_greatParent.position.x * gridCountPerMeter) / gridCountPerMeter),
				(float) (Mathf.Round(_greatParent.position.y * gridCountPerMeter) / gridCountPerMeter),
				(float) (Mathf.Round(_greatParent.position.z * gridCountPerMeter) / gridCountPerMeter)
			);
		if (_game.CheckValidity(_game.XAXIS | _game.YAXIS) &&
		    _game.CheckValidity(_game.ZAXIS | _game.YAXIS)) {
			_game.HandleLevelOver();
		}
		
	}

	public void OnPointerClick(PointerEventData eventData) {
		OnMouseDown();
	}


	public void LeftSwipeHandler() {
		if (!_vertical && _lock)  {
			_greatParent.position -= MoveFactor * MoveVector * (int)Mathf.Abs(GetComponent<SwipeHandler>().GetDelta().x);
		}
	}

	public void RightSwipeHandler() {
		if (!_vertical && _lock) {
			_greatParent.position += MoveFactor * MoveVector * (int)Mathf.Abs(GetComponent<SwipeHandler>().GetDelta().x);
		}
	}

	public void DownSwipeHandler() {
		if (_vertical && _lock) {
			_greatParent.position -= MoveFactor * MoveVector * (int)Mathf.Abs(GetComponent<SwipeHandler>().GetDelta().y);
		}
	}

	public void UpSwipeHandler() {
		if (_vertical && _lock) {
			_greatParent.position += MoveFactor * MoveVector * (int)Mathf.Abs(GetComponent<SwipeHandler>().GetDelta().y);
		}
	}
}
