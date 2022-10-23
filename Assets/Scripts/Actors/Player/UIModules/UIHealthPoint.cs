using System.Collections;
using System.Collections.Generic;
using TheraBytes.BetterUi;
using UnityEngine;

namespace VHS {
    public class UIHealthPoint : MonoBehaviour {
        [SerializeField] private BetterImage _fillImage;

        public void Fill(bool state) => _fillImage.enabled = state;
    }
}
