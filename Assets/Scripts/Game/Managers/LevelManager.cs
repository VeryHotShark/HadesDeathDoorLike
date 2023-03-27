using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System.Linq;
using System.Management.Instrumentation;
using MEC;
using Sirenix.OdinInspector;
using TMPro;
using Trisibo;
using UnityEditor;
using UnityEngine.InputSystem;

namespace VHS {
    public class LevelManager : Singleton<LevelManager> {

        [SerializeField] private SceneField _menuScene;
        [SerializeField] private SceneField _dojoScene;
        [SerializeField] private SceneField _gameScene;
        [SerializeField] private SceneField _loadScene;

        [SerializeField] private Image _loadBar;
        [SerializeField] private TextMeshProUGUI _loadText;
        
        private Canvas _canvas;

        protected override void OnAwake() {
            _canvas = GetComponentInChildren<Canvas>();
            _canvas.gameObject.SetActive(false);
        }

        public static void LoadDojo() => LoadScenes(Instance._dojoScene.BuildIndex);
        public static void LoadMenu() => LoadScenes(Instance._menuScene.BuildIndex);
        public static void LoadGame() => LoadScenes(Instance._gameScene.BuildIndex);

        public static void LoadScenes(params int[] scenes) {
            if (scenes.Length == 0) 
                return;
            
            Timing.RunCoroutine(_LoadScenesRoutine(scenes));
        }

        private static IEnumerator<float> _LoadScenesRoutine(int[] scenes){
            Instance._loadBar.fillAmount = 0.0f;
            Instance._loadText.SetText("Loading...");
            Instance._canvas.gameObject.SetActive(true);

            float totalProgress = 0.0f;

            for (int i = 0; i < scenes.Length; i++) {
                int scene = scenes[i];
                
                AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(scene,
                    i == 0 ? LoadSceneMode.Single : LoadSceneMode.Additive);

                while (!asyncOperation.isDone) {
                    totalProgress = (i + asyncOperation.progress) / scenes.Length;
                    Instance._loadBar.fillAmount = totalProgress;
                    yield return Timing.WaitForOneFrame;
                }
            }

            Instance._loadBar.fillAmount = 1.0f;
            Instance._loadText.SetText("Press To Continue");
            Instance._canvas.gameObject.SetActive(false);
        }
        
        [MenuItem("Tools/Hyperstrange/OpenScenes/Game")]
        private static void OpenGame() {
            
        }
    }
}
