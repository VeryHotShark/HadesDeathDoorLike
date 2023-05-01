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
        [SerializeField] private SceneField[] _activeScenesLeft;
        [SerializeField] private SceneField[] _activeScenesRight;

        private static int _currentRoom = 0;
        
        public static void RequestExitRoom(ExitDoor exitDoor) {
            float dot = Vector3.Dot(Vector3.right, exitDoor.transform.forward);

            bool rightDoor = Mathf.Abs(dot) > 0.5f;
            
            _currentRoom++;

            bool isBossRoom = _currentRoom % BOSS_ROOM_INCREMENT == 0; 
            
            if(isBossRoom)
                RequestBossScene();
            else
                RequestActiveScene(rightDoor);
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

        private static void RequestActiveScene(bool isRightDoor) {
            int randomIndex = -1;
            SceneField randomScene = null;
            
            if (isRightDoor) {
                randomIndex = Random.Range(0, Instance._activeScenesRight.Length);
                randomScene = Instance._activeScenesRight[randomIndex];
            }
            else {
                randomIndex = Random.Range(0, Instance._activeScenesLeft.Length);
                randomScene = Instance._activeScenesLeft[randomIndex];
            }
            
            LevelManager.LoadScenes(randomScene.BuildIndex);
        }
    }
}
