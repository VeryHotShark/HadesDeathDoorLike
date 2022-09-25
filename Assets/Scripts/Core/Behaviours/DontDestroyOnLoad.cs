using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    public class DontDestroyOnLoad : MonoBehaviour {
        private void Awake() => DontDestroyOnLoad(gameObject);
    }
}