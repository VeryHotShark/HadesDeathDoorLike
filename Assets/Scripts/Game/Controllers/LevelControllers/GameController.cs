using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

namespace VHS {
    public enum LevelType {
        Arena,
        Waves,
        Boss,
        Shop,
    }
    
    public class GameController : LevelController {
        [SerializeField] private LevelType _levelType;
        
        private UIController _uiController;
        private WaveController _waveController;
        private ArenaController _arenaController;
        private PlayerSpawnController _playerSpawnController;

        private ExitDoor[] _exitDoors = new ExitDoor[0];

        public Player Player => _playerSpawnController.Player;
        
        // TODO Later Change so there is only one CombatController base class To handle Start and End of Level
        public WaveController WaveController => _waveController;
        public ArenaController ArenaController => _arenaController;

        protected override void GetComponents() {
            _exitDoors = FindObjectsOfType<ExitDoor>();
            _waveController = GetComponentInChildren<WaveController>();
            _arenaController = GetComponentInChildren<ArenaController>();
            _playerSpawnController = GetComponentInChildren<PlayerSpawnController>();
        }

        protected override void MyEnable() {
            PlayerManager.OnPlayerDeath += OnPlayerDeath;

            if (_arenaController)
                _arenaController.OnArenaCleared += OnLevelFinished;

            if (_waveController)
                _waveController.OnWavesCleared += OnLevelFinished;
        }

        protected override void MyDisable() {
            PlayerManager.OnPlayerDeath -= OnPlayerDeath;
            
            if (_arenaController)
                _arenaController.OnArenaCleared -= OnLevelFinished;

            if (_waveController)
                _waveController.OnWavesCleared -= OnLevelFinished;
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

            switch (_levelType) {
                case LevelType.Arena:
                    _arenaController.StartArena();
                    break;
                    
                case LevelType.Waves:
                    _waveController.StartWaves();
                    break;
                    
                case LevelType.Boss:
                    break;
                    
                case LevelType.Shop:
                    break;
                
                default:
                    break;
            }
        }

        public void LoadDojo() {
            LevelManager.LoadScenes(LevelManager.DojoScene);
        }
    }

}