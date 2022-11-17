using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShaderCrew.SeeThroughShader {
    [AddComponentMenu(Strings.COMPONENTMENU_PREFAB_INSTANCE)]
    public class PrefabInstance : MonoBehaviour
{

        private PlayersPositionManager posManager;
        // Start is called before the first frame update
        void Start()
        {
            posManager = GameObject.FindObjectOfType<PlayersPositionManager>();
            if (posManager != null)
            {
                //posManager.playableCharacters.Add(this.gameObject);
                //posManager.isInitialized = false;
                //posManager.init();
                posManager.AddPlayerAtRuntime(this.gameObject);
            }
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        private void OnEnable()
        {
            if (posManager != null)
            {
                //posManager.playableCharacters.Add(this.gameObject);
                //posManager.isInitialized = false;
                //posManager.init();
                posManager.AddPlayerAtRuntime(this.gameObject);
            }
        }

        /*
        private void OnDisable()
        {
            if (posManager != null && posManager.playableCharacters.Contains(this.gameObject))
            {
                posManager.playableCharacters.Remove(this.gameObject);
                posManager.isInitialized = false;
                posManager.init();
            }
        }
        */
    }

}
