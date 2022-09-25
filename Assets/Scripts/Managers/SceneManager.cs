using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Linq;
using Sirenix.OdinInspector;

namespace VHS {
    public class SceneManager : Singleton<SceneManager> {

        [ValueDropdown("SelectScene", DropdownTitle = "Scene Selection")]
        [SerializeField] private string _menuScene;
        [ValueDropdown("SelectScene", DropdownTitle = "Scene Selection")]
        [SerializeField] private string _dojoScene;
        [ValueDropdown("SelectScene", DropdownTitle = "Scene Selection")]
        [SerializeField] private string _gameScene;
        [ValueDropdown("SelectScene", DropdownTitle = "Scene Selection")]
        [SerializeField] private string _playerScene;

        public static void LoadMenu(LoadSceneMode loadSceneMode = LoadSceneMode.Single) => SceneManager.Load(Instance._menuScene,loadSceneMode);

        public static void LoadDojo(LoadSceneMode loadSceneMode = LoadSceneMode.Single) => SceneManager.Load(Instance._dojoScene,loadSceneMode);

        public static void LoadGame(LoadSceneMode loadSceneMode = LoadSceneMode.Single) => SceneManager.Load(Instance._gameScene,loadSceneMode);

        public static void LoadPlayer(LoadSceneMode loadSceneMode = LoadSceneMode.Additive) => SceneManager.Load(Instance._playerScene, loadSceneMode);

        public static void Load(string scene, LoadSceneMode loadMode) => UnityEngine.SceneManagement.SceneManager.LoadScene(scene, loadMode);

        public static AsyncOperation LoadAsync(string scene, LoadSceneMode loadMode) => UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(scene, loadMode);

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
