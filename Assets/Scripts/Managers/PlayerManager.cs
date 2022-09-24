using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace VHS {
    public class PlayerManager : Singleton<PlayerManager> {
        [SerializeField] private Scene _playerScene;

        public void LoadPlayerScene() {
            SceneManager.Load(_playerScene, LoadSceneMode.Additive);
        }
    }
}
