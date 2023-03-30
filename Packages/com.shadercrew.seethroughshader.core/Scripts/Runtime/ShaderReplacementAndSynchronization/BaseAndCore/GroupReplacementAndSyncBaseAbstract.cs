using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace ShaderCrew.SeeThroughShader
{
    [AddComponentMenu("")]
    public abstract class GroupReplacementAndSyncBaseAbstract : ReplacementAndSyncBaseAbstract
    {

        public Material referenceMaterial;


        public enum ReplacementGroupType { Parent, Box, Id, ListOfGameObjects, ListOfMaterials }
        public ReplacementGroupType replacementGroupType = ReplacementGroupType.Parent;

        public Transform parentTransform;
        public List<Material> materialList;
        public List<GameObject> gameObjectList;
        public string triggerID;
        public TriggerBox triggerBox;


        public Transform[] transformsWithSTS;

        public bool keepMaterialsInSyncWithReference = true;



        public Shader seeThroughShader;

        public List<Material> materialExemptions;
        public LayerMask layerMaskToAdd = ~0;

        protected List<string> materialNoApplyNames;

        private Dictionary<Material, Material> materialCache = new Dictionary<Material, Material>();

        protected abstract bool isReplacement { get; } 

        protected override void Awake()
        {
            if (this.isActiveAndEnabled)
            {
                base.Awake();
                materialNoApplyNames = GeneralUtils.MaterialsNoApplyListToNameList(materialExemptions);

                if (parentTransform == null)
                {
                    parentTransform = this.transform;
                }

                if (replacementGroupType == ReplacementGroupType.ListOfMaterials && materialList.Count > 0)
                {
                    foreach (Material material in materialList)
                    {
                        if(material != null)
                        {
                            Material savedCopy = new Material(material);
                            GeneralUtils.AddSTSInstancePrefix(material);
                            string name = material.name;
                            if (isReplacement)
                            {
                                if (!name.Contains(" - Replaced by " + referenceMaterial.name))
                                {
                                    name += " - Replaced by " + referenceMaterial.name;
                                }
                                material.shader = UnityToSTSShaderMapping.TryGetValue(material.shader.name, out Shader value) ? value : UnityToSTSShaderMapping[SeeThroughShaderConstants.STS_SHADER_DEFAULT_KEY];
                            }
                            else
                            {
                                if (!name.Contains(" - Synced with " + referenceMaterial.name))
                                {
                                    name += " - Synced with " + referenceMaterial.name;
                                }
                            }
                            material.name = name;
                            materialCache.Add(material, savedCopy);
                        }
                    }
                }

                doSetupOfAllMaterials();
            }
        }

        void OnDestroy()
        {

            if (replacementGroupType == ReplacementGroupType.ListOfMaterials && materialList.Count > 0 && materialCache.Count > 0)
            {
                for (int i = 0; i < materialList.Count; i++)
                {
                    if(materialList[i] != null)
                    {
                        materialList[i].CopyPropertiesFromMaterial(materialCache[materialList[i]]);
                        materialList[i].name = materialCache[materialList[i]].name;
                        materialList[i].shader = materialCache[materialList[i]].shader;
                    }
                }
            }
        }

        void Start()
        {
            if (this.isActiveAndEnabled)
            {
                if (referenceMaterial != null)
                {
                    SynchronizeSTSMaterialsWithReferenceMaterial();

                    ////if (transformsWithSTS != null && seeThroughShaderName != null && referenceMaterial != null)
                    //if (UnityToSTSShaderMapping != null && referenceMaterial != null)
                    //{
                    //    if (replacementGroupType == ReplacementGroupType.ListOfMaterials)
                    //    {
                    //        if (materialList != null && materialList.Count > 0)
                    //        {
                    //            //GeneralUtils.updateSeeThroughShaderMaterialProperties(transformsWithSTS, seeThroughShaderName, referenceMaterial);
                    //            GeneralUtils.updateSeeThroughShaderMaterialPropertiesAndKeywords(materialList.ToArray(), UnityToSTSShaderMapping.Values.ToList(), referenceMaterial);
                    //        }
                    //        else
                    //        {
                    //            Debug.LogWarning("No matching materials could be found! Please check your setting of the " + this.GetType().Name + " script on the GameObject with name '" + this.name + "'.");
                    //        }
                    //    }
                    //    else
                    //    {
                    //        if (transformsWithSTS != null && transformsWithSTS.Length > 0)
                    //        {
                    //            GeneralUtils.updateSeeThroughShaderMaterialPropertiesAndKeywords(transformsWithSTS, UnityToSTSShaderMapping.Values.ToList(), referenceMaterial);

                    //        }
                    //        else
                    //        {
                    //            Debug.LogWarning("No matching materials could be found! Please check your setting of the " + this.GetType().Name + " script on the GameObject with name '" + this.name + "'.");
                    //        }
                    //    }

                    //}
                }
            }
        }


        private void LateUpdate()
        {
            if (this.isActiveAndEnabled)
            {
                if (keepMaterialsInSyncWithReference && referenceMaterial != null)
                {
                    SynchronizeSTSMaterialsWithReferenceMaterial();
                }
            }
        }

        private void SynchronizeSTSMaterialsWithReferenceMaterial()
        {
            //if (transformsWithSTS != null && seeThroughShaderName != null && referenceMaterial != null)
            if (UnityToSTSShaderMapping != null && referenceMaterial != null)
            {
                if (replacementGroupType == ReplacementGroupType.ListOfMaterials)
                {
                    if (materialList != null && materialList.Count > 0)
                    {
                        //GeneralUtils.updateSeeThroughShaderMaterialProperties(transformsWithSTS, seeThroughShaderName, referenceMaterial);
                        GeneralUtils.updateSeeThroughShaderMaterialPropertiesAndKeywords(materialList.ToArray(), UnityToSTSShaderMapping.Values.ToList(), referenceMaterial);
                    }
                    else
                    {
                        Debug.LogWarning("No matching materials could be found! Please check your setting of the " + this.GetType().Name + " script on the GameObject with name '" + this.name + "'.");
                    }
                }
                else
                {
                    if (transformsWithSTS != null && transformsWithSTS.Length > 0)
                    {

                        GeneralUtils.updateSeeThroughShaderMaterialPropertiesAndKeywords(transformsWithSTS, UnityToSTSShaderMapping.Values.ToList(), referenceMaterial);

                    }
                    else
                    {
                        Debug.LogWarning("No matching materials could be found! Please check your setting of the " + this.GetType().Name + " script on the GameObject with name '" + this.name + "'.");
                    }
                }
            }
        }



        protected List<GameObject> getGameObjectsDependingOnReplacementGroupType()
        {
            List<GameObject> tempGameObjectList = new List<GameObject>();
            switch (replacementGroupType)
            {
                case ReplacementGroupType.Parent:
                    {
                        //AddDescendantsWithTag(parentTransform, gameObjectList);
                        Transform[] transforms = parentTransform.GetComponentsInChildren<Transform>();

                        foreach (Transform transform in transforms)
                        {
                            if (transform.GetComponent<Renderer>() != null)
                            {
                                AddFilteredGameObject(transform.gameObject, tempGameObjectList);
                            }
                        }
                        ////also add parent
                        //if (parentTransform.GetComponent<Renderer>() != null)
                        //{
                        //    gameObjectList.Add(parentTransform.gameObject);
                        //    AddFilteredGameObject(gameObject, gameObjectList);
                        //}

                        break;
                    }
                case ReplacementGroupType.ListOfGameObjects:
                    {
                        foreach(GameObject gameObject in gameObjectList)
                        {
                            if (gameObject.GetComponent<Renderer>() != null)
                            {
                                //gameObjectList.Add(gO);
                                AddFilteredGameObject(gameObject, tempGameObjectList);
                            }
                        }

                        break;
                    }
                case ReplacementGroupType.ListOfMaterials:
                    {
                       break;
                    }
                case ReplacementGroupType.Box:
                    {
                        if (triggerBox != null)
                        {
                            Collider[] hitColliders = triggerBox.GetColliderInsideBox();
                            foreach (Collider collider in hitColliders)
                            {
                                //gameObjectList.Add(collider.gameObject);
                                AddFilteredGameObject(collider.gameObject, tempGameObjectList);
                            }
                        }
                        break;
                    }
                case ReplacementGroupType.Id:
                    {
                        if (!string.IsNullOrEmpty(triggerID))
                        {
                            TriggerObjectId[] idObjects = GameObject.FindObjectsOfType<TriggerObjectId>();
                            foreach (TriggerObjectId item in idObjects)
                            {
                                if (item.gameObject.GetComponent<TriggerObjectId>() != null && item.gameObject.GetComponent<TriggerObjectId>().triggerID == triggerID)
                                {
                                    //gameObjectList.Add(item.transform.gameObject);
                                    AddFilteredGameObject(item.transform.gameObject, tempGameObjectList);
                                }
                            }
                        }

                        break;
                    }
                default: break;
            }
            return tempGameObjectList;
        }



        private void AddFilteredGameObject(GameObject gameObject, List<GameObject> gameObjectList)
        {
            if ( (gameObject.GetComponent<SeeThroughShaderPlayer>() == null || !gameObject.GetComponent<SeeThroughShaderPlayer>().isActiveAndEnabled)
                && (gameObject.GetComponent<SeeThroughShaderExemption>() == null || !gameObject.GetComponent<SeeThroughShaderExemption>().isActiveAndEnabled) )
            {
                Renderer renderer = gameObject.GetComponent<Renderer>();
                if (renderer != null && renderer.sharedMaterials.Length > 0)
                {
                    if(!gameObjectList.Contains(gameObject))
                    {
                        gameObjectList.Add(gameObject);
                    }
                }
            }
        }


        // equivalent to transform.GetComponentsInChildren<Transform>(); ?
        private void AddDescendantsWithTag(Transform parent, List<GameObject> list)
        {
            foreach (Transform child in parent)
            {
                if (child.gameObject.GetComponent<Renderer>() != null)
                {
                    list.Add(child.gameObject);
                }
                AddDescendantsWithTag(child, list);
            }

        }



        protected void doSetupOfAllMaterials()
        {
            Dictionary<string, Material> materialTracker = new Dictionary<string, Material>();
            List<GameObject> gameObjects = getGameObjectsDependingOnReplacementGroupType();
            List<Transform> tmpTransforms = new List<Transform>();

            foreach (GameObject go in gameObjects)
            {
                Renderer[] renderers = go.GetComponentsInChildren<Renderer>();
                for (int i = 0; i < renderers.Length; i++)
                {
                    if (renderers[i] != null && renderers[i].materials.Length > 0)
                    {
                        Material[] updatedMaterials = renderers[i].materials;
                        for (int j = 0; j < renderers[i].materials.Length; j++)
                        {
                            if (renderers[i].materials[j] != null)
                            {
                                if (isReplacement == !UnityToSTSShaderMapping.ContainsValue(renderers[i].materials[j].shader))
                                {
                                    if (materialNoApplyNames != null && materialNoApplyNames.Contains(renderers[i].materials[j].name.Replace(" (Instance)", "")))
                                    {
                                        if (renderers[i].gameObject.GetComponent<SeeThroughShaderExemption>() == null)
                                        {
                                            renderers[i].gameObject.AddComponent<SeeThroughShaderExemption>();
                                        }
                                    }
                                    // adds SeeThroughShader to materials, depending on layer and if not in materialNoApplyList
                                    else if (((1 << renderers[i].gameObject.layer) & layerMaskToAdd) != 0 && (materialNoApplyNames == null || !materialNoApplyNames.Contains(renderers[i].materials[j].name.Replace(" (Instance)", ""))))
                                    {
                                        GeneralUtils.AddSTSInstancePrefix(renderers[i].materials[j]);
                                        string name = renderers[i].materials[j].name.Replace(" (Instance)", "");
                                        if (isReplacement)
                                        {
                                            if (!name.Contains(" - Replaced by " + referenceMaterial.name))
                                            {
                                                name += " - Replaced by " + referenceMaterial.name;
                                            }
                                        }
                                        else
                                        {
                                            if (!name.Contains(" - Synced with " + referenceMaterial.name))
                                            {
                                                name += " - Synced with " + referenceMaterial.name;
                                            }
                                        }

                                        renderers[i].materials[j].name = name;

                                        if (!materialTracker.ContainsKey(renderers[i].materials[j].name))
                                        {
                                            //Debug.Log("ContainsKey[j]: " + j + " . " + renderers[i].materials[j].name);
                                            Material mat = new Material(renderers[i].materials[j]);
                                            if (isReplacement)
                                            {
                                                mat.shader = UnityToSTSShaderMapping.TryGetValue(mat.shader.name, out Shader value) ? value : UnityToSTSShaderMapping[SeeThroughShaderConstants.STS_SHADER_DEFAULT_KEY];
                                            }
                                            //GeneralUtils.RenameInstancedMaterialNameSynchronization(mat, referenceMaterial.name);
                                            materialTracker.Add(renderers[i].materials[j].name, mat);
                                        }
                                        updatedMaterials[j] = materialTracker[renderers[i].materials[j].name];
                                        //Debug.Log("updatedMaterials[j]: " + j + " . " + renderers[i].materials[j].name);
                                        //if(!tempMaterials.ContainsKey(material))
                                        //{
                                        //    tempMaterials.Add(material,material.name);
                                        //    GeneralUtils.RenameInstancedMaterialNameSynchronization(material, referenceMaterial.name);
                                        //}

                                        if (!tmpTransforms.Contains(renderers[i].gameObject.transform))
                                        {
                                            tmpTransforms.Add(renderers[i].gameObject.transform);
                                        }
                                    }
                                }
                            }

                        }
                        renderers[i].materials = updatedMaterials;
                    }
                }


            }
            transformsWithSTS = tmpTransforms.ToArray();
        }

    }
}