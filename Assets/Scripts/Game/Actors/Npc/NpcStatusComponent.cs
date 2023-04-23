using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    public class NpcStatusComponent : NpcComponent, IUpdateListener {
        private List<Status> _statuses = new List<Status>();
        private Dictionary<Type, Status> _statusesDict = new Dictionary<Type, Status>();

        protected override void Enable() => UpdateManager.AddUpdateListener(this);
        protected override void Disable() => UpdateManager.RemoveUpdateListener(this);

        public void OnUpdate(float deltaTime) {
            // if(!Parent.IsAlive)
                // return;
            
            for (int i = _statuses.Count - 1; i >= 0; i--) {
                Status status = _statuses[i];
                status.OnTick(deltaTime);
            }
        }

        public void ApplyStatus(Status status) {
            if (!ContainsStatus(status)) {
                _statuses.Add(status);
                _statusesDict.Add(status.GetType(), status);
                status.Init(Parent, this);
                status.OnApplied();
            }
            else
                status.OnReapplied();
            
        }

        public void RemoveStatus(Status status) {
            _statusesDict.Remove(status.GetType());
            _statuses.Remove(status);
            status.OnRemoved();
        }

        public bool ContainsStatus(Status status) => _statusesDict.ContainsKey(status.GetType());
    }
}
