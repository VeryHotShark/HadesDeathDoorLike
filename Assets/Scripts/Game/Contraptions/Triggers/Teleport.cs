using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    public class Teleport : MonoBehaviour, ITriggerable {
        [SerializeField] private Transform _teleportTo;
        
        public void OnActorTriggerEnter(IActor Actor) {
            Player player = Actor as Player;
            
            if (player != null) {
                Vector3 startPos = Actor.FeetPosition;
                Vector3 endPos = _teleportTo.transform.position;
                Vector3 delta = endPos - startPos;
                player.CharacterMotor.SetPositionAndRotation(endPos, _teleportTo.transform.rotation);
                player.CameraController.TeleportToPlayer(delta);
            }
        }

        public void OnActorTriggerExit(IActor Actor) {
            Debug.Log("EXIT");
        }

        private void OnDrawGizmos() {
            if(_teleportTo == null)
                return;
            
            Gizmos.DrawLine(transform.position, _teleportTo.position);
        }
    }
}