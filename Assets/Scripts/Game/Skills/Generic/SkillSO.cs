using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    [CreateAssetMenu(fileName = "Skill", menuName = "VHS/SkillSO")]
    public class SkillSO : ShopItemSO {
        [SerializeReference] private ISkill _skill;

        public ISkill Instance => _skill;
    }
}
