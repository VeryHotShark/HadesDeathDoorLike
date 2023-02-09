using System;
using System.Collections;
using System.Collections.Generic;
using ParadoxNotion;
using Sirenix.OdinInspector;
using UnityEngine;

namespace VHS {
    public class Trigger : MonoBehaviour {
        [Title("Trigger Collider")] [EnumToggleButtons, HideLabel] [SerializeField] [OnValueChanged("AddCollider")]
        private ColliderTriggerType _colliderTrigger = ColliderTriggerType.Box;

        [Title("Trigger Actor")] [EnumToggleButtons, HideLabel] [SerializeField]
        private ActorTriggerType _actorTrigger = ActorTriggerType.Player;

        [Flags]
        private enum ActorTriggerType {
            Player = 1 << 1,
            Enemy = 1 << 2,
        }

        private enum ColliderTriggerType {
            Box,
            Sphere,
        }

        public event Action<IActor> OnTriggerEnterCallback = delegate { };
        public event Action<IActor> OnTriggerExitCallback = delegate { };

        private void OnTriggerEnter(Collider other) {
            IActor actor = other.GetComponentInParent<IActor>();

            if (actor != null) {
                bool actorIsPlayer = actor is Player;

                if (actorIsPlayer) {
                    if (_actorTrigger.HasFlag(ActorTriggerType.Player))
                        OnTriggerEnterCallback(actor);
                }
                else {
                    if (_actorTrigger.HasFlag(ActorTriggerType.Enemy))
                        OnTriggerEnterCallback(actor);
                }
            }
        }

        private void OnTriggerExit(Collider other) {
            IActor actor = other.GetComponentInParent<IActor>();

            if (actor != null) {
                bool actorIsPlayer = actor is Player;

                if (actorIsPlayer) {
                    if (_actorTrigger.HasFlag(ActorTriggerType.Player))
                        OnTriggerExitCallback(actor);
                }
                else {
                    if (_actorTrigger.HasFlag(ActorTriggerType.Enemy))
                        OnTriggerExitCallback(actor);
                }
            }
        }

        private void AddCollider() {
            Collider collider = null;

            switch (_colliderTrigger) {
                case ColliderTriggerType.Box:
                    collider = gameObject.GetAddComponent<BoxCollider>();

                    if (TryGetComponent(out SphereCollider sphereCollider))
                        DestroyImmediate(sphereCollider);
                    break;
                case ColliderTriggerType.Sphere:
                    collider = gameObject.GetAddComponent<SphereCollider>();

                    if (TryGetComponent(out BoxCollider boxCollider))
                        DestroyImmediate(boxCollider);
                    break;
            }

            if (collider)
                collider.isTrigger = true;
        }

        private void OnDrawGizmos() {
            Collider collider = GetComponent<Collider>();
            Gizmos.color = Color.red.WithAlpha(0.5f);

            if (!collider)
                return;

            switch (collider) {
                case BoxCollider box:
                    Gizmos.matrix = transform.localToWorldMatrix;
                    Gizmos.DrawCube(box.center, box.size);
                    break;
                case SphereCollider sphere:
                    Gizmos.DrawSphere(transform.position + sphere.center,
                        sphere.radius * transform.lossyScale.MaxAbsolute());
                    break;
                default:
                    Debug.LogWarning("Colliders Other than Box or Sphere not Supported!");
                    break;
            }
        }

        private void Reset() {
            BoxCollider boxCollider = gameObject.GetAddComponent<BoxCollider>();
            boxCollider.isTrigger = true;
        }
    }
}