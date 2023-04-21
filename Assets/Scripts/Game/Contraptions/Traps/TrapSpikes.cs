using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace VHS {
    public class TrapSpikes : Trap {
        [SerializeField] private float _activationDelay;
        [SerializeField] private float _spikeHeight;
        [SerializeField] private Transform _spikeMesh;

        protected override void OnActivate(IActor actor) {
            Sequence spikeSequence = DOTween.Sequence();
            
            float startY = _spikeMesh.localPosition.y;
            float desiredY = startY + _spikeHeight;

            spikeSequence
                .Append(_spikeMesh.DOLocalMoveY(desiredY, 0.1f).OnComplete(CheckForDamage))
                .AppendInterval(0.6f)
                .Append(_spikeMesh.DOLocalMoveY(startY, 0.3f))
                .SetDelay(_activationDelay);
            
            spikeSequence.Play();
        }

        private void CheckForDamage() {
            Vector3 halfExtent = transform.localScale / 2.0f;
            Collider[] colliders = Physics.OverlapBox(transform.position, halfExtent.With(y: 3.0f) , transform.rotation, LayerManager.Masks.ACTORS);

            foreach (Collider col in colliders) {
                IHittable hittable = col.GetComponentInParent<IHittable>();

                if (hittable != null) {
                    HitData hitData = new HitData() {
                        damage = 1,
                        dealer = this.gameObject,
                        direction = Vector3.up,
                        position = transform.position
                    };
                    
                    hittable.Hit(hitData);
                }
            }
        }
    }
}
