using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace VHS {
    public class DojoController : LevelController {
        public void LoadGame() {
            SceneManager.LoadPlayer(LoadSceneMode.Single);              
            SceneManager.LoadGame(LoadSceneMode.Additive);
        }
        
        public void LoadMenu() {
            SceneManager.LoadMenu();
        }
    }
}
