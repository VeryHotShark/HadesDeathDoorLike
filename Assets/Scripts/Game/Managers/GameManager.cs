using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace VHS {
    public class GameManager : Singleton<GameManager> {
        private bool _isPaused;
        public static bool IsPaused => Instance._isPaused;
        
        public static void PauseGame() {
            // Time.timeScale = 0.0f;
            Instance._isPaused = true;
            SetTimescale(0.0f,true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        
        public static void ResumeGame() {
            // Time.timeScale = 1.0f;
            Instance._isPaused = false;
            ResetTimescale(1.0f, true);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;
        }

        public static void SetTimescale(float timeScale, bool lerp = false, float lerpSpeed = 5.0f, float duration = 0.0f ,bool infinite = true) => MMTimeScaleEvent.Trigger(MMTimeScaleMethods.For, timeScale, duration, lerp, lerpSpeed, infinite);

        public static void ResetTimescale(float duration = 0.0f, bool lerp = false, float lerpSpeed = 5.0f) => MMTimeScaleEvent.Trigger(MMTimeScaleMethods.Reset, 1f, duration, lerp, lerpSpeed, false);
    }
}
