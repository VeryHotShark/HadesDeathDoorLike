using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    public class UIController : LevelControllerModule {
        private WavesUI _wavesUI;

        private UIModule[] _modules;

        public LevelController Controller => Parent;

        private void Awake() {
            _wavesUI = GetComponentInChildren<WavesUI>();
            _modules = GetComponentsInChildren<UIModule>();
        }

        public override void MyEnable() {
            foreach (UIModule module in _modules) 
                module.MyEnable();
        }

        public override void MyDisable() {
            foreach (UIModule module in _modules) 
                module.MyDisable();
        }

        public void ShowWaveUI(bool state) {
            _wavesUI?.Show(state);
        }
    }
}
