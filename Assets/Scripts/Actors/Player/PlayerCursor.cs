using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    public class PlayerCursor : MonoBehaviour {
        [SerializeField] private Transform _cursorTransform;
        [SerializeField] private Transform _characterReferencePoint;

        private Plane _cursorPlane;
        private CameraController _cameraController;

        public Transform CursorTransform => _cursorTransform;

        private void Awake() {
            _cameraController = GetComponent<CameraController>();
	        _cursorPlane = new Plane(Vector3.up, -_characterReferencePoint.position.y);
	        Cursor.visible = false;
	        Cursor.lockState = CursorLockMode.Confined;
        }

        public void SetCursorPos(Vector2 mousePos) {
            _cursorPlane.distance = -_characterReferencePoint.position.y;

            Ray ray = _cameraController.Camera.ScreenPointToRay(mousePos);

            if (_cursorPlane.Raycast(ray, out float distance)) {
                Vector3 pos = ray.GetPoint(distance);
                Quaternion rot = Quaternion.LookRotation(pos - _characterReferencePoint.position);
                _cursorTransform.SetPositionAndRotation(pos, rot);
            }
        }
    }
}