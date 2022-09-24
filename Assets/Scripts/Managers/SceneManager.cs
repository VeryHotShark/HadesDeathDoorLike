using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace VHS {
    public class SceneManager : Singleton<SceneManager> {
        
        [SerializeField] private Scene _dojoScene;
        [SerializeField] private Scene _gameScene;

        public static void LoadDojo() {
            SceneManager.Load(Instance._dojoScene, LoadSceneMode.Single);
        }
        
        public static void LoadGame() {
            SceneManager.Load(Instance._dojoScene, LoadSceneMode.Single);
        }
        
        public static void Load(Scene scene, LoadSceneMode loadMode) {
            UnityEngine.SceneManagement.SceneManager.LoadScene(scene.name, loadMode);
        }

        public static AsyncOperation LoadAsync(Scene scene, LoadSceneMode loadMode) {
            return UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(scene.name, loadMode);
        }
    }
}
