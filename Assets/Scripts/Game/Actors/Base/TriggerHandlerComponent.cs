using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    public class TriggerHandlerComponent : ActorComponent<Actor> {
        private Dictionary<Collider, ITriggerable> _triggersDict = new();
        private void OnTriggerEnter(Collider other) {
            ITriggerable triggerable = other.GetComponentInParent<ITriggerable>();

            if (triggerable != null && !_triggersDict.ContainsKey(other)) {
                triggerable.OnActorTriggerEnter(Parent);   
                _triggersDict.Add(other, triggerable);
            }
        }

        private void OnTriggerExit(Collider other) {
            if (_triggersDict.ContainsKey(other)) {
                _triggersDict[other].OnActorTriggerExit(Parent);
                _triggersDict.Remove(other);
            }
        }
    }
}
