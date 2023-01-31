using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    public class UIController : LevelControllerModule {
        private SpawnControllerUI _spawnControllerUI;

        private UIModule[] _modules;

        public LevelController Controller => Parent;

        private void Awake() {
            _spawnControllerUI = GetComponentInChildren<SpawnControllerUI>();
            _modules = GetComponentsInChildren<UIModule>();
        }

        public override void MyEnable() {
            // foreach (UIModule module in _modules) 
                // module.MyEnable();
        }

        public override void MyDisable() {
            // foreach (UIModule module in _modules) 
                // module.MyDisable();
        }
    }
}
