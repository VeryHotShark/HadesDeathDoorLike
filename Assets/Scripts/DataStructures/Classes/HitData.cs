using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    public class HitData {
        public int damage;
        public IActor actor;
        public GameObject dealer;
        public PlayerAttackType playerAttackType;
        public Vector3 position;
        public Vector3 direction;
    }
}
