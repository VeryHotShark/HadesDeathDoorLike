using System.Collections;
using System.Collections.Generic;
using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using Pathfinding;
using UnityEngine;

namespace VHS {
    [Category("Custom Pathfinding A*")]
    [Description("SpwanPointWander")]
    public class SpawnPointWander : ActionTask<Npc> {
        public float _radius = 5.0f;
        
        [GetFromAgent]
        private RichAI _richAI = default;

        private Vector3 _spawnPointPos;
        
        protected override string OnInit() {
            _spawnPointPos = agent.FeetPosition;
            return null;
        }

        protected override void OnExecute() {
            Vector3 randomPointInsideRadius = Random.insideUnitSphere.Flatten() * Random.value * _radius;
            Vector3 destination = _spawnPointPos + randomPointInsideRadius;

            _richAI.destination = destination;
            EndAction(true);
        }
        
        public override void OnDrawGizmosSelected() {
            if ( _richAI != null ) {
                Gizmos.color = Color.yellow.WithAlpha(0.5f);
                Gizmos.DrawSphere(_richAI.destination, 3.0f);
            }
        }
    }
}
