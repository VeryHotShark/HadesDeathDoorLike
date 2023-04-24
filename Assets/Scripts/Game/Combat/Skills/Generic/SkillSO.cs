using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    [CreateAssetMenu(fileName = "Skill", menuName = "VHS/Combat/SkillSO")]
    public class SkillSO : ShopItemSO {
        [SerializeReference] private Skill _skill;

        public Skill Instance => _skill;
    }
}
