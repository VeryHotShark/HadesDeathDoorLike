using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    public class MenuController : LevelController {
        public void StartGame() {
            SceneManager.LoadDojo();
            SceneManager.LoadPlayer();
        }
    }
}
