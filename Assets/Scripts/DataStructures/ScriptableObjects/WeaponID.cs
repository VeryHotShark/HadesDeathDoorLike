using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    [CreateAssetMenu(fileName = "WeaponID", menuName = "VHS/ID/Weapon")]
    public class WeaponID : EntityID {
        [SerializeField] private Weapon _prefab;

        public Weapon Prefab => _prefab;
    }
}
