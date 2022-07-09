using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace KinematicCharacterController.Examples
{
    public class StressTestManager : MonoBehaviour
    {
        [FormerlySerializedAs("Camera")] public Camera _camera;
        [FormerlySerializedAs("UIMask")] public LayerMask _uiMask;

        [FormerlySerializedAs("CountField")] public InputField _countField;
        [FormerlySerializedAs("RenderOn")] public Image _renderOn;
        [FormerlySerializedAs("SimOn")] public Image _simOn;
        [FormerlySerializedAs("InterpOn")] public Image _interpOn;
        [FormerlySerializedAs("CharacterPrefab")] public ExampleCharacterController _characterPrefab;
        [FormerlySerializedAs("AIController")] public ExampleAIController _aiController;
        [FormerlySerializedAs("SpawnCount")] public int _spawnCount = 100;
        [FormerlySerializedAs("SpawnDistance")] public float _spawnDistance = 2f;

        private void Start()
        {
            KinematicCharacterSystem.EnsureCreation();
            _countField.text = _spawnCount.ToString();
            UpdateOnImages();

            KinematicCharacterSystem.Settings._autoSimulation = false;
            KinematicCharacterSystem.Settings._interpolate = false;
        }

        private void Update()
        {

            KinematicCharacterSystem.Simulate(Time.deltaTime, KinematicCharacterSystem.CharacterMotors, KinematicCharacterSystem.PhysicsMovers);
        }

        private void UpdateOnImages()
        {
            _renderOn.enabled = _camera.cullingMask == -1;
            _simOn.enabled = Physics.autoSimulation;
            _interpOn.enabled = KinematicCharacterSystem.Settings._interpolate;
        }

        public void SetSpawnCount(string count)
        {
            if (int.TryParse(count, out int result))
            {
                _spawnCount = result;
            }
        }

        public void ToggleRendering()
        {
            if(_camera.cullingMask == -1)
            {
                _camera.cullingMask = _uiMask;
            }
            else
            {
                _camera.cullingMask = -1;
            }
            UpdateOnImages();
        }

        public void TogglePhysicsSim()
        {
            Physics.autoSimulation = !Physics.autoSimulation;
            UpdateOnImages();
        }

        public void ToggleInterpolation()
        {
            KinematicCharacterSystem.Settings._interpolate = !KinematicCharacterSystem.Settings._interpolate;
            UpdateOnImages();
        }

        public void Spawn()
        {
            for (int i = 0; i < _aiController._characters.Count; i++)
            {
                Destroy(_aiController._characters[i].gameObject);
            }
            _aiController._characters.Clear();

            int charsPerRow = Mathf.CeilToInt(Mathf.Sqrt(_spawnCount));
            Vector3 firstPos = ((charsPerRow * _spawnDistance) * 0.5f) * -Vector3.one;
            firstPos.y = 0f;

            for (int i = 0; i < _spawnCount; i++)
            {
                int row = i / charsPerRow;
                int col = i % charsPerRow;
                Vector3 pos = firstPos + (Vector3.right * row * _spawnDistance) + (Vector3.forward * col * _spawnDistance);

                ExampleCharacterController newChar = Instantiate(_characterPrefab);
                newChar.Motor.SetPosition(pos);

                _aiController._characters.Add(newChar);
            }
        }
    }
}