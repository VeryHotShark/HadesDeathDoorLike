using System.Collections;
using System.Collections.Generic;
using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using Pathfinding;
using UnityEngine;

namespace VHS {
    [Category("Custom Pathfinding A*")]
    [Description("Wander")]
    public class WanderAction : ActionTask<Npc> {
        public float _forwardOffset = 4.0f;
        public float _wanderRadius = 5.0f;
        
        [GetFromAgent]
        private RichAI _richAI = default;
        
        protected override void OnExecute() {
            Vector3 randomRadius = Random.insideUnitSphere.Flatten() * _wanderRadius;
            Vector3 forwardOffsetPos = agent.Forward * _forwardOffset;
            Vector3 destination = agent.FeetPosition + forwardOffsetPos + randomRadius;

            _richAI.destination = destination;
            EndAction(true);
        }
        
        public override void OnDrawGizmosSelected() {
            if ( _richAI != null ) {
                Gizmos.color = Color.blue.WithAlpha(0.5f);
                Gizmos.DrawSphere(_richAI.destination, 3.0f);
            }
        }
    }
}
