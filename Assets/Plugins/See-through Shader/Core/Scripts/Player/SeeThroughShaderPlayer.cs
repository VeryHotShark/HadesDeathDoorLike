using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShaderCrew.SeeThroughShader
{
    [AddComponentMenu("")]
    public class SeeThroughShaderPlayer : MonoBehaviour
    {
        void Start()
        {

        }

        void OnDestroy()
        {
            PlayersPositionManager posManager = GameObject.FindObjectOfType<PlayersPositionManager>();
            if (posManager != null)
            {
                posManager.RemovePlayerAtRuntime(this.gameObject);
            }
        }



    }
}