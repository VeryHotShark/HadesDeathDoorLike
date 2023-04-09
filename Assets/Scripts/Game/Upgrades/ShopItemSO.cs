using Sirenix.OdinInspector;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace VHS {
    public enum ShopItemType {
        Upgrades,
        Passive,
        Skill,
        Buy,
    }
    
    public class ShopItemSO : ScriptableObject {
        [SerializeField, ReadOnly] private string _guid = "GENERATE ID";
        [SerializeField] private Sprite _icon;
        [SerializeField] private string _name;
        [SerializeField] private string _description;

        public Sprite Icon => _icon;
        public string Name => _name;
        public string ID => _guid;
        public string Description => _description;
        
        [ContextMenu("Assign ID")]
        private void AssignUniqueId() {
        #if UNITY_EDITOR
                    Undo.RecordObject(this, $"Assign Unique ID {this}");
                    _guid = GUID.Generate().ToString();
        #endif
        }
        
        private void Reset() => AssignUniqueId();
    }
}
