using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
namespace VHS {
    public class ShopItemSO : ScriptableObject {
        [SerializeField] private Sprite _icon;
        [SerializeField] private string _name;
        [SerializeField] private string _description;

        public Sprite Icon => _icon;
        public string Name => _name;
        public string Description => _description;
    }
}
