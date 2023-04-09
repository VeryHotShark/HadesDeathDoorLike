using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    public class ShopItemsManager : Singleton<ShopItemsManager> {
        [SerializeField] private List<SkillSO> _skills = new List<SkillSO>();
        [SerializeField] private List<PassiveSO> _passives = new List<PassiveSO>();

        public static PassiveSO GetRandomPassiveSO() => Instance._passives.GetRandomElement();
        public static SkillSO GetRandomSkillSO() => Instance._skills.GetRandomElement();
    }
}
