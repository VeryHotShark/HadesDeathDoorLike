using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

namespace VHS {
    public class NpcWorldSpaceUI : BaseBehaviour {
        private PositionConstraint _positionConstraint;

        private void Awake() {
            for (int i = transform.childCount - 1; i >= 0; i--) 
                Destroy(transform.GetChild(i).gameObject);
            
            _positionConstraint = GetComponent<PositionConstraint>();
        }

        public void Init(Transform attachTransform, Vector3 offset) {
            ConstraintSource positionSource = new ConstraintSource {
                weight = 1.0f,
                sourceTransform = attachTransform
            };

            _positionConstraint.AddSource(positionSource);
            _positionConstraint.translationOffset = offset;
            _positionConstraint.constraintActive = true;
            
            transform.rotation = Quaternion.Euler(60.0f,45.0f,0.0f);
            transform.localScale = Vector3.one * 0.005f;
        }

        public void Attach(Transform attachObject, Vector3 offset, float scale = 1.0f) {
            attachObject.SetParent(transform);
            attachObject.SetLocalPositionAndRotation(offset, Quaternion.identity);
            attachObject.localScale = Vector3.one * scale;
        }
    }
}
