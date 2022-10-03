using System.Collections;
using System.Collections.Generic;
using MEC;
using TMPro;
using UnityEngine;

namespace VHS {
    public class WavesUI : UIModule<GameController> {
        [SerializeField] private TextMeshProUGUI _waveInfo;
        [SerializeField] private TextMeshProUGUI _waveCounter;

        public override void MyEnable() {
            Controller.WaveController.OnWavesStart += OnWavesStart;
            Controller.WaveController.OnWaveChanged += OnWaveChanged;
            Controller.WaveController.OnWavesCleared += OnWavesCleared;
        }
        
        public  override void MyDisable() {
            Controller.WaveController.OnWavesStart -= OnWavesStart;
            Controller.WaveController.OnWaveChanged -= OnWaveChanged;
            Controller.WaveController.OnWavesCleared -= OnWavesCleared;
        }

        private void OnWavesStart() {
            DisplayInfo("START!", 1.5f);   
        }

        private void OnWavesCleared() {
            DisplayInfo("COMPLETED!", 2.0f);
        }

        private void OnWaveChanged(int waveIndex) {
            waveIndex++;
            SetWaveCounter(waveIndex);   
        }

        public void SetWaveCounter(int wave) {
            _waveCounter.SetText(wave.ToString());
        }

        public void DisplayInfo(string info, float duration) {
            _waveInfo.SetText(info);
            Timing.CallDelayed(duration, () => _waveInfo.SetText(""));
        }
    }
}
