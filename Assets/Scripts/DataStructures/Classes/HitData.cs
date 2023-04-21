using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    public class HitData {
        public int damage;
        public IActor instigator;
        public IHittable hittable;
        public GameObject dealer;
        public PlayerAttackType playerAttackType;
        public Vector3 position;
        public Vector3 direction;
    }
}
