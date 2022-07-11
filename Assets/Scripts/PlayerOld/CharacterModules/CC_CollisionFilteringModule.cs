using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    public class CC_CollisionFilteringModule : OldCharacterControllerModule {
        [SerializeField] private List<Collider> _ignoredColliders = new List<Collider>();

        private bool _ignoreCollisions;
        
        public override void SetInputs(OldCharacterInputs inputs) {
            if (inputs.IgnoreCollisionsPressed)
                _ignoreCollisions = !_ignoreCollisions;
        }

        public bool IsColliderValidForCollisions(Collider coll) {
            if (!_ignoreCollisions)
                return true;
            
            if (_ignoredColliders.Contains(coll))
                return false;

            return true;
        }
    }
}
