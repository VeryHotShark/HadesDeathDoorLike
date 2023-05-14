using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    public class PlayerUIController : ChildBehaviour<InputController> {
        private PlayerUIModule[] _uiModules;
        public Player Player => Parent.Player;

        private void Awake() => _uiModules = GetComponentsInChildren<PlayerUIModule>();
    }
}
