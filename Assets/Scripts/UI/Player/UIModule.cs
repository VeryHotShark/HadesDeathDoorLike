using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    public class UIModule : ChildBehaviour<UIController> {
        
        private CanvasGroup _canvasGroup;
        
        private void Awake() => _canvasGroup = GetComponent<CanvasGroup>();


        /// <summary>
        /// Called by UIContrller after it finish its initialization so all reference are setup
        /// </summary>
        public virtual void MyEnable() { }
        
        /// <summary>
        /// Same as MyEnable but Disable
        /// </summary>
        public virtual void MyDisable() { }

        public void Show(bool state) => _canvasGroup.alpha = state ? 1f : 0.0f;
    }

    public class UIModule<T> : UIModule where T : LevelController {
        protected T Controller => Parent.Controller as T;
    }
}
