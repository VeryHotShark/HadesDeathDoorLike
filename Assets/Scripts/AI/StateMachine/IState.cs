using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState {
    void OnEnter();
    void OnExit();
    void OnReset();
    void OnTick(float deltaTime);
}
