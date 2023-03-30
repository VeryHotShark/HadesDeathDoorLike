using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShaderCrew.SeeThroughShader
{
    [AddComponentMenu(Strings.COMPONENTMENU_TRIGGER_BY_PARENT)]
    public class TriggerByParent : MonoBehaviour
    {
        public bool isDedicatedEnterExitTrigger;
        public bool dedicatedEnterTrigger;
        public bool dedicatedExitTrigger;
        public GameObject dedicatedTriggerParent;
        TransitionController seeThroughShaderController;
        private List<GameObject> listPlayerInside = new List<GameObject>();


        void Start()
        {
            if (this.isActiveAndEnabled)
            {
                InitializeTrigger();
            }
        }


        private void InitializeTrigger()
        {
            if (this.gameObject.GetComponent<Collider>() != null && this.gameObject.GetComponent<Collider>().isTrigger == false)
            {
                this.gameObject.GetComponent<Collider>().isTrigger = true;
            }

            if (isDedicatedEnterExitTrigger == false)
            {
                dedicatedEnterTrigger = false;
                dedicatedExitTrigger = false;
            }

            Transform parentTransform = null;
            if (seeThroughShaderController != null)
            {
                seeThroughShaderController.destroy();
            }
            if (dedicatedEnterTrigger || dedicatedExitTrigger)
            {
                if (dedicatedTriggerParent != null)
                {
                    parentTransform = dedicatedTriggerParent.transform;

                }
                else
                {
                    Debug.Assert(true, "Use of dedicatedEnterTrigger and dedicatedExitTrigger requires a dedicatedTriggerParent GameObject! This won't work! Please add a Dedicated Trigger Parent");
                }
            }
            else
            {
                parentTransform = transform;
                
            }
            if (parentTransform != null)
            {
                seeThroughShaderController = new TransitionController(parentTransform);
            }



        }



        //private void OnEnable()
        //{
        //    if (seeThroughShaderController == null)
        //    {
        //        InitializeTrigger();
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

        private void OnTriggerEnter(Collider other)
        {
            if (this.isActiveAndEnabled && other.gameObject.GetComponent<SeeThroughShaderPlayer>() != null)
            {
                if (!dedicatedEnterTrigger && !dedicatedExitTrigger)
                {
                    if (dedicatedTriggerParent == null)
                    {
                        seeThroughShaderController.notifyOnTriggerEnter(this.transform, other.transform);
                    }
                    else
                    {
                        seeThroughShaderController.notifyOnTriggerEnter(dedicatedTriggerParent.transform, other.transform);
                    }
                }
                else if (dedicatedEnterTrigger)
                {
                    List<GameObject> playersInside = checkIfPlayerIsInside(other, false);

                    if (!listPlayerInside.Contains(other.gameObject) && playersInside.Count <= 0)
                    {
                        listPlayerInside.Add(other.gameObject);
                        seeThroughShaderController.notifyOnTriggerEnter(dedicatedTriggerParent.transform, other.transform);
                    }


                }
                else if (dedicatedExitTrigger)
                {
                    List<GameObject> playersInside = checkIfPlayerIsInside(other, true);

                    if (playersInside.Count > 0)
                    {

                        seeThroughShaderController.notifyOnTriggerExit(dedicatedTriggerParent.transform, other.transform);
                    }

                }
            }
        }

        private List<GameObject> checkIfPlayerIsInside(Collider playerCollider, bool removePlayerFromTrigger)
        {
            List<GameObject> playersInside = new List<GameObject>();
            foreach (Transform child in dedicatedTriggerParent.transform)
            {
                if (child.gameObject != this.gameObject)
                {
                    TriggerByParent childTriggerByParent = child.gameObject.GetComponent<TriggerByParent>();

                    if (childTriggerByParent != null && childTriggerByParent.dedicatedTriggerParent == this.dedicatedTriggerParent)
                    {
                        if (childTriggerByParent.listPlayerInside.Count > 0 && childTriggerByParent.listPlayerInside.Contains(playerCollider.gameObject))
                        {
                            playersInside.Add(playerCollider.gameObject);
                            if (removePlayerFromTrigger)
                            {
                                childTriggerByParent.listPlayerInside.Remove(playerCollider.gameObject);
                            }
                        }
                    }
                }
            }

            return playersInside;
        }

        private void OnTriggerExit(Collider other)
        {

            if (this.isActiveAndEnabled && other.gameObject.GetComponent<SeeThroughShaderPlayer>() != null)
            {
                if (!dedicatedEnterTrigger && !dedicatedExitTrigger)
                {
                    seeThroughShaderController.notifyOnTriggerExit(this.transform, other.transform);
                }
            }
        }
    }
}