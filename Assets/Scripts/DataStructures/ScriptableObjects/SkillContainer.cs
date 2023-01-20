using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VHS;

[System.Serializable]
[CreateAssetMenu(menuName = "Skills/Skill", fileName = "Skill")]
public class SkillContainer : ScriptableObject {
    [SerializeReference] public Skill skill;
}
