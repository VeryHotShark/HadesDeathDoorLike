using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace VHS {
    public class DojoController : LevelController {
        public void LoadGame() {
            LevelManager.LoadScenes( LevelManager.PlayerScene, LevelManager.GameScene);
        }
        
        public void LoadMenu() {
            LevelManager.LoadScenes(LevelManager.MenuScene);
        }
    }
}
