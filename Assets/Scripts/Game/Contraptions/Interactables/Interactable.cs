using System.Collections;
using System.Collections.Generic;
using MEC;
using UnityEngine;

namespace VHS {
    public class Interactable : BaseBehaviour, IInteractable {
        [SerializeField] private InteractType _interactType;

        private bool _interacted = false;
        
        public InteractType InteractType => _interactType;

        public virtual bool IsInteractable(Player player) => !_interacted;

        public virtual void OnInteract(Player player) {
            _interacted = true;
        }
    }
}
