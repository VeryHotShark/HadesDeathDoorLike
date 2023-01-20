using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    public enum InteractType {
        Instant,
        Hold,
    }
    
    public interface IInteractable {
        GameObject gameObject { get; }
        InteractType InteractType { get; }
        
        bool IsInteractable(Player player);
        void OnInteract(Player player);
    }
}
