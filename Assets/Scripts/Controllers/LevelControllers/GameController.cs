using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

namespace VHS {
    public class GameController : LevelController {
        private UIController _uiController;
        private WaveController _waveController;
        private PlayerSpawnController _playerSpawnController;

        public WaveController WaveController => _waveController;

        protected override void GetComponents() {
            _waveController = GetComponentInChildren<WaveController>();
            _playerSpawnController = GetComponentInChildren<PlayerSpawnController>();
        }

        protected override void MyEnable() => PlayerManager.OnPlayerDeath += OnPlayerDeath;

        protected override void MyDisable() => PlayerManager.OnPlayerDeath -= OnPlayerDeath;

        private void OnPlayerDeath(Player player) {
            Timing.CallDelayed(2.0f, LoadDojo);
        }

        protected override void StartLevel() {
            _playerSpawnController.Spawn();
            _waveController.StartWaves();
        }

        public void LoadDojo() {
            LevelManager.LoadScenes(LevelManager.DojoScene);
        }
    }

}