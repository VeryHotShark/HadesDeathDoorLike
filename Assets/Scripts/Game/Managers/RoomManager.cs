using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Trisibo;
using UnityEngine.SceneManagement;

namespace VHS {
    public class RoomManager : Singleton<RoomManager> {
        private const int BOSS_ROOM_INCREMENT = 5;
            
        [SerializeField] private SceneField[] _bossScenes;
        [SerializeField] private SceneField[] _passiveScenes;
        [SerializeField] private SceneField[] _activeScenes;

        private static int _currentRoom = 0;
        
        public static void RequestExitRoom(ExitDoor exitDoor) {
            _currentRoom++;

            bool isBossRoom = _currentRoom % BOSS_ROOM_INCREMENT == 0; 
            
            if(isBossRoom)
                RequestBossScene();
            else
                RequestActiveScene();
        }

        private static void RequestBossScene() {
            int randomIndex = Random.Range(0, Instance._bossScenes.Length);
            SceneField randomScene = Instance._bossScenes[randomIndex];
            LevelManager.LoadScenes(randomScene.BuildIndex);
        }
        
        private static void RequestPassiveScene() {
            int randomIndex = Random.Range(0, Instance._passiveScenes.Length);
            SceneField randomScene = Instance._passiveScenes[randomIndex];
            LevelManager.LoadScenes(randomScene.BuildIndex);
        }

        private static void RequestActiveScene() {
            int randomIndex = Random.Range(0, Instance._activeScenes.Length);
            SceneField randomScene = Instance._activeScenes[randomIndex];
            LevelManager.LoadScenes(randomScene.BuildIndex);
        }
    }
}
