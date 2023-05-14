using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace VHS {
    public class PlayerSpawnController : BaseBehaviour {
        [FormerlySerializedAs("_playerPrefab")] [SerializeField] private InputController inputPrefab;
        
        private PlayerSpawnPoint _spawnPoint;
        private InputController _inputController;

        public Player Player => _inputController != null ? _inputController.Player : null;

        private void Awake() => _spawnPoint = FindObjectOfType<PlayerSpawnPoint>();

        public void Spawn() {
            _inputController = FindObjectOfType<InputController>();

            if (!_spawnPoint)
                _spawnPoint = FindObjectOfType<PlayerSpawnPoint>();
            
            if(!_spawnPoint)
                Log("NO SPAWN POINT ON SCENE, SPAWNING AT 0,0,0");

            Transform spawnTransform = _spawnPoint ? _spawnPoint.transform : transform;
            
            if (_inputController) {
                PlayerManager.RegisterPlayer(_inputController.Player);
                _inputController.Character.Motor.SetPositionAndRotation(spawnTransform.position, spawnTransform.rotation);
                return;
            }
            
            _inputController = Instantiate(inputPrefab, spawnTransform.position, spawnTransform.rotation);
            PlayerManager.RegisterPlayer(_inputController.Player);
        }
    }

}