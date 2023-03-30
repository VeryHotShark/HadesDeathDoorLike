using ShaderCrew.SeeThroughShader;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ShaderCrew.SeeThroughShader
{
    [AddComponentMenu(Strings.COMPONENTMENU_TRIGGER_BY_BOX)]
    public class TriggerByBox : MonoBehaviour
    {
        public TriggerBox triggerBox;
        public bool thisIsEnterTrigger = false;
        public bool thisIsExitTrigger = false;
        //public Material refMaterial;
        public bool optionalResetColliders = false;
        //public List<Material> listMaterialNoApply;
        //Shader seeThroughShader;
        private TransitionController seeThroughShaderController;
        private List<GameObject> listPlayerInside = new List<GameObject>();


        //private string seeThroughShaderName;
        //Dictionary<string, string> UnityToSTSShaderNameMapping;


        List<GameObject> listGameObjects;
        //List<Material> listM;
        void Start()
        {
            if (this.isActiveAndEnabled)
            {
                //seeThroughShaderName = GeneralUtils.getUnityVersionAndRenderPipelineCorrectedShaderString().versionAndRPCorrectedShader;
                //seeThroughShader = Shader.Find(seeThroughShaderName);

                //UnityToSTSShaderNameMapping = GeneralUtils.getUnityToSTSShaderMapping();


                if (triggerBox != null)
                {

                    listGameObjects = buildListGameObjects();
                    if (listGameObjects != null && listGameObjects.Count > 0)
                    {
                        if (seeThroughShaderController == null)
                        {
                            seeThroughShaderController = new TransitionController(listGameObjects);
                        }
                    }
                }

                if (this.gameObject.GetComponent<Collider>() != null && this.gameObject.GetComponent<Collider>().isTrigger == false)
                {
                    this.gameObject.GetComponent<Collider>().isTrigger = true;
                }
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

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

        public void SetSeeThroughActive(Collider other)
        {
            if (triggerBox != null)
            {

                if (listGameObjects != null && listGameObjects.Count > 0)
                {
                    if (seeThroughShaderController == null)
                    {
                        seeThroughShaderController = new TransitionController(listGameObjects);
                    }
                    seeThroughShaderController.notifyOnTriggerEnter(listGameObjects, other.transform);
                }



            }
        }

        public void SetSeeThroughInActive(Collider other)
        {
            if (triggerBox != null)
            {

                if (listGameObjects != null && listGameObjects.Count > 0)
                {
                    seeThroughShaderController = new TransitionController(listGameObjects);
                    seeThroughShaderController.notifyOnTriggerExit(listGameObjects, other.transform);
                }


            }
        }

        public void setFloorColliders()
        {
            if (triggerBox != null)
            {
                triggerBox.enableAllInsideColliders();
                Collider[] hitColliders = triggerBox.GetColliderInsideBox();


            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (this.isActiveAndEnabled && other.gameObject.GetComponent<SeeThroughShaderPlayer>() != null)
            {
                if (thisIsEnterTrigger)
                {

                    bool isInside = false;
                    List<GameObject> groupMembersFound = new List<GameObject>();
                    foreach (Transform t in this.gameObject.transform.root)
                    {
                        if (t.gameObject != this.gameObject)
                        {
                            if (t.gameObject.GetComponent<TriggerByBox>() && t.gameObject.GetComponent<TriggerByBox>().triggerBox == this.triggerBox)
                            {
                                if (t.gameObject.GetComponent<TriggerByBox>().listPlayerInside.Count > 0 && t.gameObject.GetComponent<TriggerByBox>().listPlayerInside.Contains(other.gameObject))
                                {
                                    isInside = true;
                                    groupMembersFound.Add(t.gameObject);
                                }
                            }
                        }
                    }

                    TriggerByBox[] groupMembersFoundScene = GameObject.FindObjectsOfType<TriggerByBox>();
                    if (groupMembersFoundScene.Length > 0)
                    {
                        foreach (TriggerByBox item in groupMembersFoundScene)
                        {
                            if (item.listPlayerInside.Count > 0)
                            {
                                if (item.listPlayerInside.Contains(other.gameObject))
                                {
                                    isInside = true;
                                    groupMembersFound.Add(item.transform.gameObject);
                                }
                            }
                        }
                    }

                    if (!listPlayerInside.Contains(other.gameObject) && !isInside)
                    {
                        listPlayerInside.Add(other.gameObject);
                        SetSeeThroughActive(other);
                    }

                }
                else if (thisIsExitTrigger)
                {
                    bool isInside = false;
                    List<GameObject> groupMembersFound = new List<GameObject>();
                    foreach (Transform t in this.gameObject.transform.root)
                    {
                        if (t.gameObject != this.gameObject)
                        {
                            if (t.gameObject.GetComponent<TriggerByBox>() && t.gameObject.GetComponent<TriggerByBox>().triggerBox == this.triggerBox)
                            {
                                if (t.gameObject.GetComponent<TriggerByBox>().listPlayerInside.Count > 0 && t.gameObject.GetComponent<TriggerByBox>().listPlayerInside.Contains(other.gameObject))
                                {
                                    isInside = true;
                                    groupMembersFound.Add(t.gameObject);
                                }
                            }
                        }
                    }

                    TriggerByBox[] groupMembersFoundScene = GameObject.FindObjectsOfType<TriggerByBox>();
                    if (groupMembersFoundScene.Length > 0)
                    {
                        foreach (TriggerByBox item in groupMembersFoundScene)
                        {
                            if (item.listPlayerInside.Count > 0)
                            {
                                if (item.listPlayerInside.Contains(other.gameObject))
                                {
                                    isInside = true;
                                    groupMembersFound.Add(item.transform.gameObject);
                                }
                            }
                        }
                    }

                    if (isInside)
                    {
                        if (groupMembersFound.Count > 0)
                        {
                            foreach (GameObject item in groupMembersFound)
                            {
                                if (item.GetComponent<TriggerByBox>() != null && item.GetComponent<TriggerByBox>().listPlayerInside.Count > 0)
                                {
                                    if (item.GetComponent<TriggerByBox>().listPlayerInside.Contains(other.gameObject))
                                    {
                                        item.GetComponent<TriggerByBox>().listPlayerInside.Remove(other.gameObject);
                                    }
                                }
                            }
                        }
                        SetSeeThroughInActive(other);

                    }

                }


                if (optionalResetColliders)
                {
                    setFloorColliders();
                }

            }
        }

        //public List<Material> buildListMaterial()
        //{
        //    Collider[] hitColliders = triggerBox.GetColliderInsideBox();
        //    List<Material> listMaterial = new List<Material>();
        //    if (hitColliders != null && hitColliders.Length > 0)
        //    {
        //        foreach (Collider item in hitColliders)
        //        {
        //            GeneralUtils.AddIfSeeThroughShaderMaterial(item.gameObject, UnityToSTSShaderNameMapping.Values.ToList(), listMaterial);
        //        }
        //        return listMaterial;
        //    }
        //    return null;
        //}        
        
        public List<GameObject> buildListGameObjects()
        {
            Collider[] hitColliders = triggerBox.GetColliderInsideBox();
            List<GameObject> listGameObjects = new List<GameObject>();
            if (hitColliders != null && hitColliders.Length > 0)
            {
                foreach (Collider item in hitColliders)
                {
                    listGameObjects.Add(item.gameObject);
                }
                return listGameObjects;
            }
            return null;
        }
                          
    }
}