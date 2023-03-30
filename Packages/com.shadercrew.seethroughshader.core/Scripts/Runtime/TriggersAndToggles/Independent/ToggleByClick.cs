using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ShaderCrew.SeeThroughShader
{
    [AddComponentMenu(Strings.COMPONENTMENU_TOGGLE_BY_CLICK)]
    public class ToggleByClick : MonoBehaviour
    {
        public TransitionController seeThroughShaderController;
        bool activated = false;


        RaycastHit[] hits = new RaycastHit[100];

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

        }


        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);


                int hitCount = Physics.RaycastNonAlloc(ray, hits);

 

                for (int i = 0; i < hitCount; i++)
                {
                    if (hits[i].collider != null && hits[i].transform != null)
                    {
                        OnGameObjectClicked(hits[i].transform.gameObject);
                    }
                }



                //RaycastHit raycastHit;
                //RaycastHit[] hit = Physics.RaycastAll(ray);
                //for (int i = 0; i < hit.Length; i++)
                //{
                //    if (hit[i].collider != null && hit[i].transform != null)
                //    {
                //        OnGameObjectClicked(hit[i].transform.gameObject);
                //    }
                //}

                //if (Physics.Raycast(ray, out raycastHit, 100f))
                //{
                //    if (raycastHit.transform != null)
                //    {
                //        OnGameObjectClicked(raycastHit.transform.gameObject);
                //    }
                //}
            }
        }

        public void OnGameObjectClicked(GameObject gameObject)
        {
            if (this.gameObject == gameObject)
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