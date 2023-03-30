using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ShaderCrew.SeeThroughShader
{
    [AddComponentMenu(Strings.COMPONENTMENU_TOGGLE_BY_UI)]
    public class ToggleByUI : MonoBehaviour
    {

        public TransitionController seeThroughShaderController;

        public Button button;
        bool activated = false;
        bool initialized = false;

        // Start is called before the first frame update
        void Start()
        {
            if (this.isActiveAndEnabled)
            {
                InitializeToggle();
            }
        }

        //private void OnEnable()
        //{
        //    if (seeThroughShaderController == null)
        //    {
        //        InitializeToggle();
        //    }
        //}

        private void OnDisable()
        {
            if (seeThroughShaderController != null)
            {
                seeThroughShaderController.destroy();
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
            seeThroughShaderController = new TransitionController(this.transform);

            if (!initialized)
            {             
                if (button != null)
                {
                    button.onClick.AddListener(delegate { UIButtonOnClick(); });
                }
                initialized = true;
            }
        }


        public void activateSTSEffect()
        {
            seeThroughShaderController.notifyOnTriggerEnter(this.transform);

        }

        public void dectivateSTSEffect()
        {
            seeThroughShaderController.notifyOnTriggerExit(this.transform);

        }

        private void UIButtonOnClick()
        {
            if (!activated)
            {
                activateSTSEffect();
                activated = true;
            }
            else
            {
                dectivateSTSEffect();
                activated = false;
            }
        }
    }
}