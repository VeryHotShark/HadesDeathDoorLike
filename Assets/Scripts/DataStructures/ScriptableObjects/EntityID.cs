using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    [CreateAssetMenu(fileName = "EntityID", menuName = "VHS/ID/Entity")]
    public class EntityID : ScriptableObject {
        [SerializeField] private string _name;
    }
}
