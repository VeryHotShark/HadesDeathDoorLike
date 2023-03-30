using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShaderCrew.SeeThroughShader
{
    public class ToggleByEnableDisable : MonoBehaviour
    {
        public TransitionController seeThroughShaderController;

        bool initialized = false;


        void Start()
        {
            if (this.isActiveAndEnabled)
            {
                InitializeToggle();
            }
        }

        private void OnEnable()
        {
            if (seeThroughShaderController == null)
            {
                InitializeToggle();
                activateSTSEffect();
            }
        }

        private void OnDisable()
        {
            if (seeThroughShaderController != null)
            {
                dectivateSTSEffect();
            }
        }

        private void OnDestroy()
        {
            if (seeThroughShaderController != null)
            {
                seeThroughShaderController.destroy();
            }
        }
        private void InitializeToggle()
        {
            if (!initialized)
            {
                seeThroughShaderController = new TransitionController(this.transform);
                initialized = true;
            }


        }



        private void activateSTSEffect()
        {
            seeThroughShaderController.notifyOnTriggerEnter(this.transform);

        }

        private void dectivateSTSEffect()
        {
            seeThroughShaderController.notifyOnTriggerExit(this.transform);

        }

    }
}