using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    public class PlayerUIModule : ChildBehaviour<PlayerUIController> {
        
        private CanvasGroup _canvasGroup;
        
        public Player Player => Parent.Player;
        
        private void Awake() => _canvasGroup = GetComponent<CanvasGroup>();
        
        
        public void Show(bool state) => _canvasGroup.alpha = state ? 1f : 0.0f;
    }
}
