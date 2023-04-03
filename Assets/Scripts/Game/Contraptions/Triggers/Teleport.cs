using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    public class Teleport : BaseBehaviour, ITriggerable {
        public void OnActorTriggerEnter(IActor Actor) {
            Debug.Log("ENTER");
        }

        public void OnActorTriggerExit(IActor Actor) {
            Debug.Log("EXIT");
        }
    }
}