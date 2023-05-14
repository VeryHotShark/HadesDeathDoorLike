using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace VHS {
    public class CameraController : ChildBehaviour<InputController> {
        private Camera _camera;
        private CinemachineBrain _brain;
        private PlayerCursor _playerCursor;

        public Camera Camera => _camera;
        public CinemachineBrain Brain => _brain;
        
        public PlayerCursor PlayerCursor => _playerCursor;
        public Transform CursorTransform => _playerCursor.CursorTransform;

        private void Awake() {
            _camera = GetComponent<Camera>();
            _brain = GetComponent<CinemachineBrain>();
            _playerCursor = GetComponent<PlayerCursor>();
        }

        public void SetCursorPos(Vector2 mousePos) => _playerCursor.SetCursorPos(mousePos);
        public void TeleportToPlayer(Vector3 delta) => _brain.ActiveVirtualCamera.OnTargetObjectWarped(Parent.Character.transform, delta);
    }
}