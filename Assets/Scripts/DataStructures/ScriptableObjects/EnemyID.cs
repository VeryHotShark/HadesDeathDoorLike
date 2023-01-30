using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    [CreateAssetMenu(fileName = "EnemyID", menuName = "ID/Enemy")]
    public class EnemyID : EntityID {
        [SerializeField] private Npc _prefab;

        public Npc Prefab => _prefab;
    }
}
