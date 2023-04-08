using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    public class WeaponController : ChildBehaviour<Player> {
        [SerializeField] private WeaponID _meleeWeaponID;
        [SerializeField] private WeaponID _rangeWeaponID;
        [SerializeField] private Transform _weaponSocket;

        private WeaponMelee _meleeWeapon;
        private WeaponRange _rangeWeapon;
        
        public WeaponMelee MeleeWeapon => _meleeWeapon;
        public WeaponRange WeaponRange => _rangeWeapon;

        private void Awake() {
            SpawnMeleeWeapon();
            SpawnRangeWeapon();
        }

        protected override void Enable() => Parent.OnCharacterStateChanged += OnCharacterStateChanged;
        protected override void Disable() => Parent.OnCharacterStateChanged -= OnCharacterStateChanged;

        private void OnCharacterStateChanged(CharacterModule module) {
            switch (module) {
                case CharacterRangeCombat range:
                    _meleeWeapon.SetVisible(false);
                    break;
                case CharacterSkillCombat skill:
                case CharacterMeleeCombat melee:
                    _meleeWeapon.SetVisible(true);
                    break;
            }  
        }

        private void SpawnMeleeWeapon() {
            _meleeWeapon = Instantiate(_meleeWeaponID.Prefab, _weaponSocket) as WeaponMelee;
            _meleeWeapon.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            _meleeWeapon.Init(Parent);
            Parent.OnWeaponChanged(_meleeWeapon);
        }

        private void SpawnRangeWeapon() {
            _rangeWeapon = Instantiate(_rangeWeaponID.Prefab, _weaponSocket) as WeaponRange;
            _rangeWeapon.Init(Parent);
        }
    }
}
