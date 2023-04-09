using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    public class UIModule : ChildBehaviour<UIController> {

        private bool _isCanvasInteractable;
        private CanvasGroup _canvasGroup;
        
        public virtual void MyAwake() {
            _canvasGroup = GetComponent<CanvasGroup>();
            _isCanvasInteractable = _canvasGroup.interactable;
        }


        /// <summary>
        /// Called by UIContrller after it finish its initialization so all reference are setup
        /// </summary>
        public virtual void MyEnable() { }
        
        /// <summary>
        /// Same as MyEnable but Disable
        /// </summary>
        public virtual void MyDisable() { }

        public void Show(bool state) {
            _canvasGroup.alpha = state ? 1f : 0.0f;
            _canvasGroup.interactable = state && _isCanvasInteractable;
            
            if(state)
                OnShow();
            else
                OnHide();
        }

        public virtual void OnShow() { }
        public virtual void OnHide() { }
    }

    public class UIModule<T> : UIModule where T : LevelController {
        protected T Controller => Parent.Controller as T;
    }
}
