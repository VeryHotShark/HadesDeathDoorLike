using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using VHS;

namespace VHS {
    public class PlayerInteractionUI : PlayerUIModule, ILateUpdateListener {
        private RectTransform _rectTransform;
        private TextMeshProUGUI _interactText;

        private IInteractable _currentInteractable;
        
        protected override void Awake() {
            base.Awake();
            _rectTransform = GetComponent<RectTransform>(); 
            _interactText = GetComponent<TextMeshProUGUI>();
            _interactText.enabled = false;
        }
        
        protected override void Enable() {
            UpdateManager.AddLateUpdateListener(this);
            Player.OnInteractableChanged += OnInteractableChanged;
        }

        protected override void Disable() {
            UpdateManager.RemoveLateUpdateListener(this);
            Player.OnInteractableChanged -= OnInteractableChanged;
        }

        private void OnInteractableChanged(IInteractable interactable) {
            _currentInteractable = interactable;
            _interactText.enabled = interactable != null;
        }

        public void OnLateUpdate(float deltaTime) {
            if (_currentInteractable != null) {
                Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(CameraManager.CameraInstance,
                    _currentInteractable.gameObject.transform.position);
                float scaleCorrection = 1f / _rectTransform.lossyScale.z;
                screenPoint = screenPoint * scaleCorrection;
                screenPoint.y += 50.0f;
                _rectTransform.anchoredPosition = screenPoint;
            }
        }
    }
}
