using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class RotateCamera : MonoBehaviour, SwipeHandler.Handler {
	public Transform FocusedObject;
	public bool State;
	public bool Lock;
	public int RotationTime = 500;
	private Camera _glowCamera;
	private Vector3 _focusPoint;
	private bool _isRotating;
	private int _rotationMultiplier;
	private int _rotationFramesLeft;
	private int _rotationFrameCount;
	public Game Game;

	void Start ()
	{
		_focusPoint = FocusedObject.position;
		_glowCamera = Camera.main.GetComponent<PostProcessing>().GlowCamera;
	}
	
	void Update() {
		if (_isRotating) {
			const int angleToRotate = 90;
			var rotationSpeed = (float) angleToRotate / _rotationFrameCount;
			var angle = _rotationMultiplier * rotationSpeed;
			transform.RotateAround(_focusPoint, Vector3.up, angle);
			if (--_rotationFramesLeft == 0) {
				_isRotating = false;
			}
		}
	}

	public void CheckSideValidity() {
		if (State) {
			if (Game.CheckValidity(Game.ZAXIS | Game.YAXIS)) {
				Game.InnerBorder.SetBlinking(Color.black, Color.green);
			} else {
				Game.InnerBorder.StopBlinking();
			}
		} else {
			if (Game.CheckValidity(Game.XAXIS | Game.YAXIS)) {
				Game.InnerBorder.SetBlinking(Color.black, Color.green);
			} else {
				Game.InnerBorder.StopBlinking();
			}
		}
	}

	public void RightSwipeHandler() {
		if (_isRotating || Time.deltaTime < 0.01f || !State || Lock) return;
		_isRotating = true;
		_rotationMultiplier = 1;
		_rotationFramesLeft = (int) (RotationTime / 1000f / Time.deltaTime);
		_rotationFrameCount = _rotationFramesLeft;
		State = false;
		CheckSideValidity();
		var glowObject = _glowCamera.GetComponent<Glow>().GlowObject;
		if (glowObject != null)
			glowObject.GetComponent<Highlightable>().arrow.GetComponent<Arrow>().UpdateState(State);
	}
	
	public void LeftSwipeHandler(){
		if (_isRotating || Time.deltaTime < 0.01f || State || Lock) return;
		_isRotating = true;
		_rotationMultiplier = -1;
		_rotationFramesLeft = (int) (RotationTime / 1000f / Time.deltaTime);
		_rotationFrameCount = _rotationFramesLeft;
		State = true;
		CheckSideValidity();
		var glowObject = _glowCamera.GetComponent<Glow>().GlowObject;
		if (glowObject != null)
			glowObject.GetComponent<Highlightable>().arrow.GetComponent<Arrow>().UpdateState(State);
	}
	
	public void DownSwipeHandler(){}
	public void UpSwipeHandler(){}

}
