using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    public interface IState {
        void OnEnter();
        void OnExit();
        void OnReset();
        void OnTick(float deltaTime);

        bool CanEnterState() => true;
        bool CanExitState() => true;
    }
}
