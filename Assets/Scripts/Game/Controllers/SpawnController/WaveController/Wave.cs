using System;
using MEC;

namespace VHS {
    public class Wave : BaseBehaviour {
        private SpawnPoint[] _spawnPoints = Array.Empty<SpawnPoint>();

        private void Awake() => _spawnPoints = GetComponentsInChildren<SpawnPoint>();

        public void Spawn(SpawnController spawnController) {
            foreach (SpawnPoint spawnPoint in _spawnPoints) {
                if (spawnPoint.SpawnDelay > 0.0f) {
                    Timing.CallDelayed(spawnPoint.SpawnDelay, () =>
                        spawnController.SpawnEnemy(spawnPoint.Npcs[0], spawnPoint.ProvidePoint()), gameObject);
                }
                else {
                    spawnController.SpawnEnemy(spawnPoint.Npcs[0], spawnPoint.ProvidePoint());
                }
            }
        }
    }
}
