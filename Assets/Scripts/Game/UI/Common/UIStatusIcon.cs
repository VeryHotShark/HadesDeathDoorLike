using System;
using System.Collections;
using System.Collections.Generic;
using TheraBytes.BetterUi;
using UnityEngine;
using UnityEngine.Lumin;

namespace VHS {
    public class UIStatusIcon : MonoBehaviour {

        private BetterImage _durationImage;
        private BetterImage[] _images;

        private Status _status;

        private void Awake() {
            _images = GetComponentsInChildren<BetterImage>();
            _durationImage = _images[1];
        }

        public void SetStatus(Status status) {
            _status = status;
            
            foreach (BetterImage image in _images) {
                image.sprite = _status.Icon;
            }
        }

        public void UpdateStatusDuration() {
            _durationImage.fillAmount = _status.DurationNormalized;
        }
    }
}
