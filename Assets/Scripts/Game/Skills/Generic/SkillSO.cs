using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    [CreateAssetMenu(fileName = "Skill", menuName = "SkillSO")]
    public class SkillSO : ScriptableObject {
        [SerializeReference] private ISkill _skill;

        public ISkill Instance => _skill;
    }
}
