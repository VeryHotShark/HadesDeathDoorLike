using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    public class CameraController : MonoBehaviour {
        private Camera _camera;
        private PlayerCursor _playerCursor;

        public Camera Camera => _camera;
        public Transform CursorTransform => _playerCursor.CursorTransform;

        private void Awake() {
            _camera = GetComponent<Camera>();
            _playerCursor = GetComponent<PlayerCursor>();
        }

        public void SetCursorPos(Vector2 mousePos) => _playerCursor.SetCursorPos(mousePos);
    }
}