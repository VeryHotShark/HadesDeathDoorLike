using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    public class NpcStatusComponent : NpcComponent, IUpdateListener {
        private List<Status> _statuses = new List<Status>();

        protected override void Enable() => UpdateManager.AddUpdateListener(this);
        protected override void Disable() => UpdateManager.RemoveUpdateListener(this);

        public void OnUpdate(float deltaTime) {
            for (int i = _statuses.Count - 1; i >= 0; i--) {
                Status status = _statuses[i];
                status.OnTick(deltaTime);
            }
        }

        public void ApplyStatus(Status status) {
            if (!ContainsStatus(status)) {
                _statuses.Add(status);
                status.Init(Parent, this);
                status.OnApplied();
            }
            else
                status.OnReapplied();
            
        }

        public void RemoveStatus(Status status) {
            _statuses.Remove(status);
            status.OnRemoved();
        }

        public bool ContainsStatus(Status status) {
            return _statuses.Contains(status);
        }
    }
}
