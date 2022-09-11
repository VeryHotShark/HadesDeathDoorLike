using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    public class NpcBlackboard : Singleton<NpcBlackboard> { // Move to some kind of master Manager and remove Singleton
        private Player _playerInstance;
        public static Player PlayerInstance => Instance._playerInstance ?? FindObjectOfType<Player>();
        
        protected override void OnAwake() {
            _playerInstance = FindObjectOfType<Player>(); // Move to Player Manager, which will also handle Spawning
        }
    }
}
