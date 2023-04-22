using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VHS {
    [Serializable]
    public abstract class Status {
        private Npc _npc;
        public Npc Npc => _npc;

        public void Init(Npc npc) => npc = _npc;

        public abstract void OnApplied();
        public abstract void OnTick(float dt);
        public abstract void OnRemoved();
    }
}
