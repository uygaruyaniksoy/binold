using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class RotateCamera : MonoBehaviour, SwipeHandler.Handler {
	public Transform FocusedObject;
	private Camera _glowCamera;
	public int RotationTime = 500;
	private Vector3 _focusPoint;
	private bool _isRotating;
	private int _rotationMultiplier;
	private int _rotationFramesLeft;
	private int _rotationFrameCount;
	public bool State;
	public bool Lock;

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

	public void RightSwipeHandler() {
		if (_isRotating || Time.deltaTime < 0.01f || !State || Lock) return;
		_isRotating = true;
		_rotationMultiplier = 1;
		_rotationFramesLeft = (int) (RotationTime / 1000f / Time.deltaTime);
		_rotationFrameCount = _rotationFramesLeft;
		State = false;
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
		var glowObject = _glowCamera.GetComponent<Glow>().GlowObject;
		if (glowObject != null)
			glowObject.GetComponent<Highlightable>().arrow.GetComponent<Arrow>().UpdateState(State);
	}
	
	public void DownSwipeHandler(){}
	public void UpSwipeHandler(){}

}
