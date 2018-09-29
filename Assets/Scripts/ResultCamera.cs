using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultCamera : MonoBehaviour {
	public Transform FocusPoint;
	private bool _prevState;
	private bool _state;

	void Start () {
		_prevState = Camera.main.GetComponent<RotateCamera>().State;
		_state = Camera.main.GetComponent<RotateCamera>().State;
	}
	
	void Update () {
		_state = Camera.main.GetComponent<RotateCamera>().State;
		if (_prevState != _state) {
			const int angleToRotate = 90;
			transform.RotateAround(FocusPoint.position, Vector3.up, angleToRotate * (_state ? -1 : 1));			
		}
		_prevState = _state;
	}
}
