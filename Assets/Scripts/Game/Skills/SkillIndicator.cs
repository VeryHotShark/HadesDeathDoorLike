using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    public enum IndicatorShape {
        Circle,
        Rectangle,
        Cone,
    }
    public class SkillIndicator : MonoBehaviour, IPoolable {
        private readonly int _tilingOffsetID = Shader.PropertyToID("_MainTex_ST");
        
        private IndicatorShape _shape;
        private MaterialPropertyBlock _propertyBlock;
        private Renderer _renderer;
        
        private void Awake() {
            _renderer = GetComponentInChildren<Renderer>();
            _propertyBlock = new MaterialPropertyBlock();
            _renderer.GetPropertyBlock(_propertyBlock);
        }

        public void InitRectangle(float distance, float width) {
            transform.localScale = new Vector3(width, 1, distance);
            Vector4 tilingOffset = new Vector4(1f, distance / 2.0f, 0, 0);
            _propertyBlock.SetVector(_tilingOffsetID, tilingOffset);
            _renderer.SetPropertyBlock(_propertyBlock);
        }
    }
}
