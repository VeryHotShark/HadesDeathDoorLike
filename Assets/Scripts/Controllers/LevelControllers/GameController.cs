using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

namespace VHS {
    public class GameController : LevelController {
        private WaveController _waveController;
        private PlayerSpawnController _playerSpawnController;

        protected override void GetComponents() {
            _waveController = GetComponentInChildren<WaveController>();
            _playerSpawnController = GetComponentInChildren<PlayerSpawnController>();
        }

        protected override void Enable() {
            PlayerManager.OnPlayerDeath += OnPlayerDeath;
        }

        protected override void Disable() {
            PlayerManager.OnPlayerDeath -= OnPlayerDeath;
        }

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