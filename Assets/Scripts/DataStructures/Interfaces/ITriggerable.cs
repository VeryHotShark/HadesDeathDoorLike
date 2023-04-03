using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    public interface ITriggerable {
        void OnActorTriggerEnter(IActor Actor);
        void OnActorTriggerExit(IActor Actor);
    }
}
