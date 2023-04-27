using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    [CreateAssetMenu(menuName = "VHS/Models/Enemy", fileName = "EnemyModel")]
    public class EnemyModel : ScriptableObject {
        public bool CanBeStaggered = true;
    }
}
