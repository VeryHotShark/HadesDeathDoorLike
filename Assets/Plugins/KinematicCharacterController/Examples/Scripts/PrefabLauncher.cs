using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace KinematicCharacterController.Examples
{
    public class PrefabLauncher : MonoBehaviour
    {
        [FormerlySerializedAs("ToLaunch")] public Rigidbody _toLaunch;
        [FormerlySerializedAs("Force")] public float _force;

        void Update()
        {
            if (Keyboard.current.enterKey.wasPressedThisFrame)
            {
                Rigidbody inst = Instantiate(_toLaunch, transform.position, transform.rotation);
                inst.AddForce(transform.forward * _force, ForceMode.VelocityChange);
                Destroy(inst.gameObject, 8f);
            }
        }
    }
}