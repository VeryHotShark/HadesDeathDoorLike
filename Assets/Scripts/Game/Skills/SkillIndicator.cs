using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    public class SkillIndicator : MonoBehaviour, IPoolable {
        public Texture2D _arrowSprite;
        public Texture2D _circleSprite;
        
        private readonly int _speedYID = Shader.PropertyToID("_ShapeYSpeed");
        private readonly int _mainTexID = Shader.PropertyToID("_MainTex");
        private readonly int _tilingOffsetID = Shader.PropertyToID("_MainTex_ST");
        
        private MaterialPropertyBlock _propertyBlock;
        private Renderer _renderer;
        private GameObject _quad;
        
        private void Awake() {
            _renderer = GetComponentInChildren<Renderer>();
            _propertyBlock = new MaterialPropertyBlock();
            _renderer.GetPropertyBlock(_propertyBlock);
            _quad = _renderer.gameObject;
        }

        public void InitRectangle(float distance, float width) {
            _quad.transform.localPosition = _quad.transform.localPosition.With(z: 0.5f); 
            transform.localScale = new Vector3(width, 1, distance);
            Vector4 tilingOffset = new Vector4(1f, distance / 2.0f, 0, 0);
            _propertyBlock.SetTexture(_mainTexID, _arrowSprite);
            _propertyBlock.SetVector(_tilingOffsetID, tilingOffset);
            _propertyBlock.SetFloat(_speedYID, -0.2f);
            _renderer.SetPropertyBlock(_propertyBlock);
        }
        
        public void InitCircle(float radius) {
            _quad.transform.localPosition = new Vector3(0, 0.1f, 0);
            transform.localScale = Vector3.one * (radius * 2f);
            _propertyBlock.SetTexture(_mainTexID, _circleSprite);
            _propertyBlock.SetVector(_tilingOffsetID, new Vector4(1f,1f,0f,0f));
            _propertyBlock.SetFloat(_speedYID, 0);
            _renderer.SetPropertyBlock(_propertyBlock);
        }
    }
}
