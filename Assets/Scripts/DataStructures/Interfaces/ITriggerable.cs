using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    public interface ITriggerable {
        void OnActorTriggerEnter(IActor actor);
        void OnActorTriggerExit(IActor actor);
    }
}
