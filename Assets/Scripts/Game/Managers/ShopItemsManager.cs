using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    public class ShopItemsManager : Singleton<ShopItemsManager> {
        [SerializeField] private SkillSO[] _skills;
        [SerializeField] private PassiveSO[] _passives;

        public static PassiveSO GetRandomPassiveSO() => Instance._passives.GetRandomElement();
        public static PassiveSO GetRandomSkillSO() => Instance._passives.GetRandomElement();
    }
}
