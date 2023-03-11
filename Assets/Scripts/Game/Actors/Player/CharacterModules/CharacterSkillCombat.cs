using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    public class CharacterSkillCombat : CharacterModule {
        [SerializeReference] private ISkill _activeSkill;

    }
}