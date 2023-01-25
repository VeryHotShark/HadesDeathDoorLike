using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    public class PlayerSpawnController : BaseBehaviour {
        [SerializeField] private PlayerController _playerPrefab;
        
        private PlayerSpawnPoint _spawnPoint;
        private PlayerController _playerController;

        public Player Player => _playerController != null ? _playerController.Player : null;

        private void Awake() => _spawnPoint = FindObjectOfType<PlayerSpawnPoint>();

        public void Spawn() {
            _playerController = FindObjectOfType<PlayerController>();

            if (!_spawnPoint)
                _spawnPoint = FindObjectOfType<PlayerSpawnPoint>();
            
            if(!_spawnPoint)
                Log("NO SPAWN POINT ON SCENE, SPAWNING AT 0,0,0");

            Transform spawnTransform = _spawnPoint ? _spawnPoint.transform : transform;
            
            if (_playerController) {
                PlayerManager.RegisterPlayer(_playerController.Player);
                _playerController.Character.Motor.SetPositionAndRotation(spawnTransform.position, spawnTransform.rotation);
                return;
            }
            
            _playerController = Instantiate(_playerPrefab, spawnTransform.position, spawnTransform.rotation);
            PlayerManager.RegisterPlayer(_playerController.Player);
        }
    }

}