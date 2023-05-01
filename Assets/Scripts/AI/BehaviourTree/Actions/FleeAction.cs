using System.Collections;
using System.Collections.Generic;
using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using Pathfinding;
using UnityEngine;

namespace VHS {
    [Category("Custom Pathfinding A*")]
    [Description("Flee from provided position")]
    public class FleeAction : ActionTask<RichAI> {
        public float _angleDeviation = 20.0f;
        public Vector2 _fleeRange = Vector2.up;
        public BBParameter<Vector3> _fleeTarget;

        [GetFromAgent]
        private RichAI _richAI = default;
        
        protected override void OnExecute() {
            Vector3 startPos = _richAI.GetFeetPosition();
            Vector3 direction = _fleeTarget.value.DirectionTo(startPos);

            if (_angleDeviation != 0.0f)
                direction = Quaternion.Euler(0.0f, Random.Range(-_angleDeviation, _angleDeviation), 0.0f) * direction;
            
            Vector3 offset = _fleeRange.Random() * direction;
            Vector3 destination = startPos + offset;
            
            var recastGraph = AstarPath.active.data.recastGraph;
            if(recastGraph.Linecast(startPos, destination, null, out GraphHitInfo hitInfo)) {
                destination = hitInfo.point + hitInfo.tangent * _fleeRange.Random();
            }

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
