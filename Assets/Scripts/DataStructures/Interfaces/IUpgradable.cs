using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    public interface IUpgradable {
        
        void OnSelected();
        void OnRemoved();
        void OnUpgrade(int upgrade);
    }
}
