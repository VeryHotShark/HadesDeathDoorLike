using System;

namespace VHS {
    public class Wave : BaseBehaviour {
        private SpawnPoint[] _spawnPoints = Array.Empty<SpawnPoint>();

        private void Awake() => _spawnPoints = GetComponentsInChildren<SpawnPoint>();

        public void Spawn(SpawnController spawnController) {
            foreach (SpawnPoint spawnPoint in _spawnPoints)
                spawnController.SpawnEnemy(spawnPoint.Npcs[0], spawnPoint.ProvidePoint());
        }
    }
}
