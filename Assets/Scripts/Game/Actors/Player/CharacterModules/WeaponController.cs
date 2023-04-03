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
            _meleeWeapon = Instantiate(_meleeWeaponID.Prefab, _weaponSocket);
            _meleeWeapon.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            _meleeWeapon.Init(Parent);
            Parent.OnWeaponChanged(_meleeWeapon);
        }
    }
}
