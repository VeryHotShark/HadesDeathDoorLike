using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using KinematicCharacterController.Examples;
using UnityEngine.Serialization;

namespace KinematicCharacterController.Examples
{
    public class Teleporter : MonoBehaviour
    {
        [FormerlySerializedAs("TeleportTo")] public Teleporter _teleportTo;

        public UnityAction<ExampleCharacterController> OnCharacterTeleport;

        public bool IsBeingTeleportedTo { get; set; }

        private void OnTriggerEnter(Collider other)
        {
            if (!IsBeingTeleportedTo)
            {
                ExampleCharacterController cc = other.GetComponent<ExampleCharacterController>();
                if (cc)
                {
                    cc.Motor.SetPositionAndRotation(_teleportTo.transform.position, _teleportTo.transform.rotation);

                    if (OnCharacterTeleport != null)
                    {
                        OnCharacterTeleport(cc);
                    }
                    _teleportTo.IsBeingTeleportedTo = true;
                }
            }

            IsBeingTeleportedTo = false;
        }
    }
}