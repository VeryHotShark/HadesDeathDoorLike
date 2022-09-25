using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

namespace VHS {
    public class GameController : LevelController {
        private Actor _player;
        private void Awake() {
            _player = FindObjectOfType<Player>(); // Change to DI or PlayerManager refernce
        }
        
        protected override void Enable() {
            _player.OnDeath += OnPlayerDeath;
        }

        protected override void Disable() {
            _player.OnDeath -= OnPlayerDeath;
        }

        private void OnPlayerDeath(IActor actor) {
            Timing.CallDelayed(2.0f, LoadDojo);
        }

        public void LoadDojo() {
            SceneManager.LoadDojo();
            SceneManager.LoadPlayer();
        }
    }

}