using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    public class DeathProcessorComponent : ChildBehaviour<Actor> {
        protected override void Enable() => Parent.OnDeath += OnDeath;
        protected override void Disable() => Parent.OnDeath -= OnDeath;

        private void OnDeath(IActor actor) => Destroy(gameObject);
    }
}
