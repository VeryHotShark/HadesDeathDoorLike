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
            MMTimeScaleEvent.Trigger(MMTimeScaleMethods.For, 0.0f, 1.0f, true, 5.0f, true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        
        public static void ResumeGame() {
            // Time.timeScale = 1.0f;
            Instance._isPaused = false;
            MMTimeScaleEvent.Trigger(MMTimeScaleMethods.Reset, 1f, 1f, true, 5.0f, false);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;
        }
    }
}
