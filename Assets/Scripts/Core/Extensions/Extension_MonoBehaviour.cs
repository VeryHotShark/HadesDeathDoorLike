using System;
using System.Collections;
using UnityEngine;

namespace VHS {
    public static class Extension_MonoBehaviour {
        /// <summary>
        /// DEPRACTED PLEASE USE MEC Plugin
        /// </summary>
        public static Coroutine CallWithDelay(this MonoBehaviour _mono, Action _method, float _delay) => _mono.StartCoroutine(CallWithDelayRoutine(_method, _delay));

        private static IEnumerator CallWithDelayRoutine(Action _method, float _delay) {
            yield return new WaitForSeconds(_delay);
            _method();
        }

        public static Coroutine CallAfterFrame(this MonoBehaviour _mono, Action _method) => _mono.StartCoroutine(CallAfterFramesRoutine(_method, 1));
        
        public static Coroutine CallAfterFrames(this MonoBehaviour _mono, Action _method, int _frames) =>
            _mono.StartCoroutine(CallAfterFramesRoutine(_method, _frames));
        
        private static IEnumerator CallAfterFramesRoutine(Action _method, int _frames) {
            for (int i = 0; i < _frames; i++) 
                yield return new WaitForEndOfFrame();
                
            _method();
        }

        // Wait Until, Predicate
        // Repeat Until, Predicate
        
    }
}