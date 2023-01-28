using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

namespace VHS {
    
    public class GameController : LevelController {
        
        private UIController _uiController;
        private SpawnController _spawnController;
        private PlayerSpawnController _playerSpawnController;

        private ExitDoor[] _exitDoors = new ExitDoor[0];

        public Player Player => _playerSpawnController.Player;
        
        public SpawnController SpawnController => _spawnController;

        protected override void GetComponents() {
            _exitDoors = FindObjectsOfType<ExitDoor>();
            _spawnController = GetComponentInChildren<SpawnController>();
            _playerSpawnController = GetComponentInChildren<PlayerSpawnController>();
        }

        protected override void MyEnable() {
            PlayerManager.OnPlayerDeath += OnPlayerDeath;

            if(_spawnController)
                _spawnController.OnFinished += OnLevelFinished;
        }

        protected override void MyDisable() {
            PlayerManager.OnPlayerDeath -= OnPlayerDeath;
            
            if(_spawnController)
                _spawnController.OnFinished -= OnLevelFinished;
        }

        private void OnPlayerDeath(Player player) => Timing.CallDelayed(2.0f, LoadDojo);

        private void OnLevelFinished() {
            foreach (ExitDoor exitDoor in _exitDoors)
                exitDoor.SetLocked(false);            
        }

        protected override void StartLevel() {
            _playerSpawnController.Spawn();

            foreach (ExitDoor exitDoor in _exitDoors)
                exitDoor.SetLocked(true);
            
            if(_spawnController)
                _spawnController.StartSpawn();
        }

        public void LoadDojo() => LevelManager.LoadScenes(LevelManager.DojoScene);
    }

}