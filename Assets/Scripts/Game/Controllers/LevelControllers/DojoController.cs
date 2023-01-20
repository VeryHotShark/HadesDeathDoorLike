using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace VHS {
    public class DojoController : LevelController {
        private PlayerSpawnController _playerSpawnController;

        protected override void GetComponents() {
            _playerSpawnController = GetComponentInChildren<PlayerSpawnController>();
        }

        protected override void StartLevel() {
            _playerSpawnController.Spawn();
        }

        public void LoadGame() {
            LevelManager.LoadScenes(LevelManager.GameScene);
        }
        
        public void LoadMenu() {
            LevelManager.LoadScenes(LevelManager.MenuScene);
        }
    }
}
