namespace VHS {
    public class Wave : ChildBehaviour<WaveController> {
        private SpawnPoint[] _spawnPoints = new SpawnPoint[0];

        private void Awake() => _spawnPoints = GetComponentsInChildren<SpawnPoint>();

        public void StartWave() {
            foreach (SpawnPoint spawnPoint in _spawnPoints)
                Parent.OnSpawnRequest(spawnPoint.Npcs[0], spawnPoint.ProvidePoint());
        }
    }
}
