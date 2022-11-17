using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShaderCrew.SeeThroughShader
{
    public class AnimationCurveSO : ScriptableObject
    {

        [SerializeField]
        public AnimationCurve curve;

        [SerializeField]
        public bool isBakedToTexture = false;
    }
}