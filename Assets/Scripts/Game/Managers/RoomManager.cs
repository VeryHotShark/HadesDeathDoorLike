using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Trisibo;
using UnityEngine.SceneManagement;

namespace VHS {
    public class RoomManager : Singleton<RoomManager> {
        [SerializeField] private SceneField[] _bossScenes;
        [SerializeField] private SceneField[] _passiveScenes;
        [SerializeField] private SceneField[] _activeScenes;
        
        public static void RequestExitRoom(ExitDoor exitDoor) {
            int randomIndex = Random.Range(0, Instance._activeScenes.Length);
            SceneField randomScene = Instance._activeScenes[randomIndex];
            LevelManager.LoadScenes(randomScene.BuildIndex);
        }
    }
}
