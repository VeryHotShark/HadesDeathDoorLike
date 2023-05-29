using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    public class SlashController : ParticleController {
        private MaterialPropertyBlock _propertyBlock;
        private Renderer _slashRenderer;

        private int _slashAngleID = Shader.PropertyToID("_AngleNormalized");
        private int _innerRadiusID = Shader.PropertyToID("_RadiusNormalized");
        private int _LeftToRightID = Shader.PropertyToID("_LeftToRight");
        
        protected override void Awake() {
            base.Awake();
            _propertyBlock = new MaterialPropertyBlock();
            _slashRenderer = _particle.GetComponentInChildren<Renderer>();
            _slashRenderer.GetPropertyBlock(_propertyBlock);
            
            
        }
        
        public void SetSlashSettings( float angle, Vector3 size,bool leftToRight = true, float innerRadius = 0.5f, float lifetime = 0.3f) {
            var mainModule = _particle.main;
            mainModule.startLifetime = lifetime;
            transform.localScale = size;

            float normalizedAngle = 1.0f - angle * 2.0f / 360.0f;
            normalizedAngle = Mathf.Clamp01(normalizedAngle);
            _propertyBlock.SetFloat(_slashAngleID, normalizedAngle);
            _propertyBlock.SetFloat(_innerRadiusID, innerRadius);
            _propertyBlock.SetFloat(_LeftToRightID, leftToRight ? 1f : 0f);
            _slashRenderer.SetPropertyBlock(_propertyBlock);
            _particle.Play();
        }
        
    }
}
