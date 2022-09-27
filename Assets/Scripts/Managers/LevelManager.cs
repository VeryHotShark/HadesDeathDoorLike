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
using UnityEditor;
using UnityEngine.InputSystem;

namespace VHS {
    public class LevelManager : Singleton<LevelManager> {

        [ValueDropdown("SelectScene", DropdownTitle = "Scene Selection")]
        [SerializeField] private string _menuScene;
        [ValueDropdown("SelectScene", DropdownTitle = "Scene Selection")]
        [SerializeField] private string _dojoScene;
        [ValueDropdown("SelectScene", DropdownTitle = "Scene Selection")]
        [SerializeField] private string _gameScene;
        [ValueDropdown("SelectScene", DropdownTitle = "Scene Selection")]
        [SerializeField] private string _playerScene;
        [ValueDropdown("SelectScene", DropdownTitle = "Scene Selection")]
        [SerializeField] private string _loadScene;

        [SerializeField] private Image _loadBar;
        [SerializeField] private TextMeshProUGUI _loadText;
        
        private Canvas _canvas;
        
        public static string MenuScene => Instance._menuScene;
        public static string DojoScene => Instance._dojoScene;
        public static string GameScene => Instance._gameScene;
        public static string PlayerScene => Instance._playerScene;

        protected override void OnAwake() {
            _canvas = GetComponentInChildren<Canvas>();
            _canvas.gameObject.SetActive(false);
        }

        public static void LoadScenes(params string[] scenes) {
            if (scenes.Length == 0) 
                return;
            
            Timing.RunCoroutine(_LoadScenesRoutine(scenes));
        }

        private static IEnumerator<float> _LoadScenesRoutine(string[] scenes) {
            Instance._loadBar.fillAmount = 0.0f;
            Instance._loadText.SetText("Loading...");
            Instance._canvas.gameObject.SetActive(true);

            float totalProgress = 0.0f;

            for (int i = 0; i < scenes.Length; i++) {
                string scene = scenes[i];
                
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
        
        private static IEnumerable SelectScene()
        {
            var filesPath = Directory.GetFiles("Assets/Scenes");
            var fileNameList = filesPath
                .Select(Path.GetFileName)
                .Select(file => file.Split(".")[0])
                .Distinct()
                .ToList();

            return fileNameList;
        }
        
        [MenuItem("Tools/Hyperstrange/OpenScenes/Game")]
        private static void OpenGame() {
            
        }
    }
}
