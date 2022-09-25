using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Linq;
using Sirenix.OdinInspector;

namespace VHS {
    public class SceneManager : Singleton<SceneManager> {

        [ValueDropdown("SelectScene", DropdownTitle = "Scene Selection")]
        [SerializeField] private string _dojoScene;
        [ValueDropdown("SelectScene", DropdownTitle = "Scene Selection")]
        [SerializeField] private string _gameScene;
        [ValueDropdown("SelectScene", DropdownTitle = "Scene Selection")]
        [SerializeField] private string _playerScene;

        public static void LoadDojo() {
            SceneManager.Load(Instance._dojoScene, LoadSceneMode.Single);
        }
        
        public static void LoadGame() {
            SceneManager.Load(Instance._gameScene,LoadSceneMode.Single);
        }

        public static void LoadPlayer() {
            SceneManager.Load(Instance._playerScene, LoadSceneMode.Additive);
        }
        
        public static void Load(string scene, LoadSceneMode loadMode) {
            UnityEngine.SceneManagement.SceneManager.LoadScene(scene, loadMode);
        }

        public static AsyncOperation LoadAsync(string scene, LoadSceneMode loadMode) {
            return UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(scene, loadMode);
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
    }
}
