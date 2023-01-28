using System.Collections;
using System.Collections.Generic;
using MEC;
using TMPro;
using UnityEngine;

namespace VHS {
    public class SpawnControllerUI : UIModule<GameController> {
        [SerializeField] private TextMeshProUGUI _mainInfo;

        public override void MyEnable() {
            Controller.SpawnController.OnStarted += OnWavesStart;
            Controller.SpawnController.OnFinished += OnWavesCleared;
        }
        
        public  override void MyDisable() {
            Controller.SpawnController.OnStarted -= OnWavesStart;
            Controller.SpawnController.OnFinished -= OnWavesCleared;
        }

        private void OnWavesStart() {
            DisplayInfo("START!", 1.5f);   
        }

        private void OnWavesCleared() {
            DisplayInfo("COMPLETED!", 2.0f);
        }

        public void DisplayInfo(string info, float duration) {
            _mainInfo.SetText(info);
            Timing.CallDelayed(duration, () => _mainInfo.SetText(""));
        }
    }
}
