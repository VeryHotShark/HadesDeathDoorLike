using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    [CreateAssetMenu(fileName = "Passive", menuName = "VHS/PassiveSO")]
    public class PassiveSO : ShopItemSO {
        [SerializeReference] private Passive _passive;

        public Passive Instance => _passive;
    }
}
