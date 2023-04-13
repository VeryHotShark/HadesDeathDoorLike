using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    public static class Extension_Actor {
        public static (Vector3 Pos, Quaternion Rot) ClosestPosRotToActor(this IActor actor, IActor targetActor) {
            Vector3 direction = targetActor.FeetPosition.DirectionTo(actor.FeetPosition);
            float radius = targetActor.Radius + actor.Radius;
            Vector3 pos = targetActor.FeetPosition + (direction * radius);
            Quaternion rot = Quaternion.LookRotation(-direction);
            return (pos, rot);
        }
    }
}