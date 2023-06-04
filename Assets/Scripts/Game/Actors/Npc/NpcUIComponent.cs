using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    public class NpcUIComponent : NpcComponent {
        [SerializeField] private NpcWorldSpaceUI _worldSpaceUIPrefab;
        [SerializeField] private NpcStatusUI _statusUIPrefab;

        private NpcStatusUI _statusUI;

        private NpcWorldSpaceUI _worldSpaceUI;

        public NpcWorldSpaceUI WorldSpaceUI => _worldSpaceUI;

        protected override void Enable() {
            Parent.OnStatusApplied += OnStatusApplied;
            Parent.OnStatusRemoved += OnStatusRemoved;
        }

        protected override void Disable() {
            Parent.OnStatusApplied -= OnStatusApplied;
            Parent.OnStatusRemoved -= OnStatusRemoved;
        }

        private void OnStatusApplied(Status status) => _statusUI.AddStatus(status);
        private void OnStatusRemoved(Status status) => _statusUI.RemoveStatus(status);

        public override void OnActorInitialized(Npc actor) {
            base.OnActorInitialized(actor);
            _worldSpaceUI = Instantiate(_worldSpaceUIPrefab);
            _worldSpaceUI.Init(Parent.transform, Vector3.up * (Parent.Height + 2.0f));

            _statusUI = Instantiate(_statusUIPrefab);
            Attach(_statusUI.transform);
        }

        public void Attach(Transform attachObject, Vector3? offset = null, float scale = 1.0f) =>
            _worldSpaceUI.Attach(attachObject, offset.GetValueOrDefault(Vector3.zero), scale);
    }
}
