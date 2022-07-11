using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    public class OldCameraController : MonoBehaviour {
        
        private Camera _camera;
        private CC_Cursor _cursor;

        public Camera Camera => _camera;
        public Quaternion RotationToCursor => _cursor.transform.rotation;

        private void Awake() {
            _camera = GetComponent<Camera>();
            _cursor = GetComponent<CC_Cursor>();
        }

        public void SetCursorPos(Vector2 mousePos) => _cursor.SetCursorPos(mousePos);
    }
}
