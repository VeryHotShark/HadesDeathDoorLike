using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace VHS {
    public class PassivesHandlerComponent : ActorComponent<Actor> {
        [SerializeField] private List<Passive> _startPassives;

        private void Awake() {
            foreach (var passive in _startPassives)
                passive.Init(Parent);
        }

        private void OnEnable() {
            foreach (var passive in _startPassives) 
                passive.Enable();
        }

        private void OnDisable() {
            foreach (var passive in _startPassives) 
                passive.Disable();
        }

        public void AddPassive(Passive passive) {
            
        }
    }
}
