using UnityEngine;
using UnityEngine.Serialization;

public class SwipeHandler : MonoBehaviour {
	public interface Handler {
		void LeftSwipeHandler();
		void RightSwipeHandler();
		void DownSwipeHandler();
		void UpSwipeHandler();
	}

	[FormerlySerializedAs("handler")] [SerializeField]
	private Component _handler;
	private Handler _handlerFunctions;
	private Vector2 _delta;

	private Vector2 _lastMousePosition;

	void Start () {
		_handlerFunctions = (Handler) _handler;
	}
	
	void Update () {
		if (Input.mousePresent && !Input.GetMouseButton(0)) {
			_lastMousePosition = Vector2.zero;
			return;
		}
		if (Input.mousePresent && Input.GetMouseButton(0) && _lastMousePosition.magnitude < 0.01f) {
			_lastMousePosition = Input.mousePosition;
			return;
		}
		if (!Input.mousePresent && (Input.touchCount <= 0 || Input.GetTouch(0).phase != TouchPhase.Moved)) return;

		var deltaPosition = Input.mousePresent ? (Vector2) Input.mousePosition - _lastMousePosition : Input.GetTouch(0).deltaPosition;
		_delta = deltaPosition;

		if (deltaPosition.magnitude < 0.1f) return;

		if (Mathf.Abs(deltaPosition.x) > Mathf.Abs(deltaPosition.y)) {
			if (deltaPosition.x < 0) {
				_handlerFunctions.LeftSwipeHandler();
			} else {
				_handlerFunctions.RightSwipeHandler();
			}
		} else {
			if (deltaPosition.y < 0) {
				_handlerFunctions.DownSwipeHandler();
			} else {
				_handlerFunctions.UpSwipeHandler();
			}
		}

		_lastMousePosition = Input.mousePosition;
	}

	public Vector2 GetDelta() {
		return _delta;
	}
}
