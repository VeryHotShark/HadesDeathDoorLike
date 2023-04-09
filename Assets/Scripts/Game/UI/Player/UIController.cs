using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    public class UIController : ChildBehaviour<LevelController> {

        private UIModule[] _modules = new UIModule[0];
        private UIShopModule _shopModule;

        public LevelController Controller => Parent;
        public UIShopModule ShopModule => _shopModule;

        private void Awake() {
            _shopModule = GetComponentInChildren<UIShopModule>();
            _modules = GetComponentsInChildren<UIModule>();
            
            foreach (UIModule module in _modules) 
                module.MyAwake();
        }

        protected  override void Enable() {
            foreach (UIModule module in _modules) 
                module.MyEnable();
        }

        protected override void Disable() {
            foreach (UIModule module in _modules) 
                module.MyDisable();
        }
    }
}
