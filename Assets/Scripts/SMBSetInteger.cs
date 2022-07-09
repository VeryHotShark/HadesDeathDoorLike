using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SetTypeEnum {
    ON_ENTER,
    ON_EXIT
}

public class SMBSetInteger : StateMachineBehaviour {
    [SerializeField] private SetTypeEnum _setType;
    [SerializeField] private string _paramName;
    [SerializeField] private int _paramValue;
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        
        if(_setType == SetTypeEnum.ON_ENTER)
            animator.SetInteger(_paramName, _paramValue);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        base.OnStateExit(animator, stateInfo, layerIndex);
        
        if(_setType == SetTypeEnum.ON_EXIT)
            animator.SetInteger(_paramName, _paramValue);
    }
}