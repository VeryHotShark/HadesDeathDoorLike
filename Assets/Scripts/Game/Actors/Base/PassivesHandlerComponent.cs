using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace VHS {
    public class PassivesHandlerComponent : ActorComponent<Actor> {
        [SerializeField] private List<PassiveSO> _startPassives;

        private void Awake() {
            foreach (var passive in _startPassives)
                passive.Instance.Init(Parent);

            foreach (var passive in PassiveManager.RuntimePassives) 
                passive.Instance.Init(Parent);
        }

        private void OnEnable() {
            foreach (var passive in _startPassives) 
                passive.Instance.Enable();
            
            foreach (var passive in PassiveManager.RuntimePassives)
                passive.Instance.Enable();
        }

        private void OnDisable() {
            foreach (var passive in _startPassives) 
                passive.Instance.Disable();
            
            foreach (var passive in PassiveManager.RuntimePassives)
                passive.Instance.Disable();
        }
    }
}
