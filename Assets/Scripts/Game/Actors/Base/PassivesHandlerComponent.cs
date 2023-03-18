using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    public class PassivesHandlerComponent : ActorComponent<Actor> {
        [SerializeField] private List<Passive> _passives;

        private void Awake() {
            foreach (var passive in _passives)
                passive.Init(Parent);
        }

        void OnEnable() {
            foreach (var passive in _passives) 
                passive.Enable();
        }

        void OnDisable() {
            foreach (var passive in _passives) 
                passive.Disable();
        }
    }
}
