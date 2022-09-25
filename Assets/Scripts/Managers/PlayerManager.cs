using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace VHS {
    public class PlayerManager : Singleton<PlayerManager> {
        private static Player _playerInstance;
        public static Player PlayerInstance => _playerInstance;
        public static event Action OnPlayerDeath = delegate { };
    }
}
