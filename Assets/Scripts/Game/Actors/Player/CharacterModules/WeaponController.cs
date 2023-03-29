using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    public class WeaponController : ChildBehaviour<Player> {
        [SerializeField] private WeaponID _meleeWeaponID;
        [SerializeField] private Transform _weaponSocket;

        private Weapon _meleeWeapon;
        public Weapon MeleeWeapon => _meleeWeapon;

        public bool HasMeleeWeapon => _meleeWeapon != null;

        private void Awake() => SpawnMeleeWeapon();

        private void SpawnMeleeWeapon() {
            _meleeWeapon = Instantiate(_meleeWeaponID.Prefab, _weaponSocket);
            _meleeWeapon.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            _meleeWeapon.Init(Parent);
            Parent.OnWeaponChanged(_meleeWeapon);
        }
    }
}
