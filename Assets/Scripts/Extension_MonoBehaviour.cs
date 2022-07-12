using System;
using System.Collections;
using UnityEngine;

namespace VHS {
    public static class Extension_MonoBehaviour {
        public static void CallWithDelay(this MonoBehaviour _mono, Action _method, float _delay) => _mono.StartCoroutine(CallWithDelayRoutine(_method, _delay));

        private static IEnumerator CallWithDelayRoutine(Action _method, float _delay) {
            yield return new WaitForSeconds(_delay);
            _method();
        }
        
        public static void CallAfterFrame(this MonoBehaviour _mono, Action _method) => _mono.StartCoroutine(CallAfterFrameRoutine(_method));

        private static IEnumerator CallAfterFrameRoutine(Action _method) {
            yield return new WaitForEndOfFrame();
            _method();
        }
    }
}