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

        public Player Player => _playerSpawnController.Player;
        public WaveController WaveController => _waveController;
        public ArenaController ArenaController => _arenaController;

        protected override void GetComponents() {
            _waveController = GetComponentInChildren<WaveController>();
            _arenaController = GetComponentInChildren<ArenaController>();
            _playerSpawnController = GetComponentInChildren<PlayerSpawnController>();
        }

        protected override void MyEnable() => PlayerManager.OnPlayerDeath += OnPlayerDeath;
        protected override void MyDisable() => PlayerManager.OnPlayerDeath -= OnPlayerDeath;

        private void OnPlayerDeath(Player player) {
            Timing.CallDelayed(2.0f, LoadDojo);
        }

        protected override void StartLevel() {
            _playerSpawnController.Spawn();

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