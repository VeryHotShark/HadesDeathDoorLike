using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    public class PlayerInteractionComponent : ChildBehaviour<Player> {
        [SerializeField] private float _interactionRadius = 3.0f;

        private Collider[] _colliders = new Collider[10];

        private IInteractable _currentInteractable;

        public override void OnCustomUpdate(float deltaTime) => CheckForInteractables();

        public void TryInteract() {
            if (_currentInteractable != null) 
                _currentInteractable.OnInteract(Parent);
            
            // _currentInteractable.InteractType
        }

        private void CheckForInteractables() {
            int hitCount = Physics.OverlapSphereNonAlloc(transform.position, _interactionRadius, _colliders,
                LayerManager.Masks.DEFAULT);

            IInteractable bestInteractable = null;
            float bestDistance = float.MaxValue;
            
            for (int i = 0; i < hitCount; i++) {
                Collider collider = _colliders[i];
                IInteractable interactable = collider.GetComponentInParent<IInteractable>();

                if(interactable == null)
                    continue;
                
                if(!interactable.IsInteractable(Parent))
                    continue;

                float dst = transform.position.DistanceSquaredTo(collider.transform.position);

                if (dst < bestDistance) {
                    bestDistance = dst;
                    bestInteractable = interactable;
                }
            }

            _currentInteractable = bestInteractable;
        }

        private void OnDrawGizmos() {
            if (_currentInteractable != null) {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(_currentInteractable.gameObject.transform.position, 1.0f);
            }
        }
    }
}
