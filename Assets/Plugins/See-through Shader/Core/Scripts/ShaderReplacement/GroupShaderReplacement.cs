using ShaderCrew.SeeThroughShader;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ShaderCrew.SeeThroughShader
{

    [AddComponentMenu(Strings.COMPONENTMENU_GROUP_SHADER_REPLACEMENT)]
    public class GroupShaderReplacement : MonoBehaviour
    {
        public Transform parentTransform;
        public Shader seeThroughShader;
        private string seeThroughShaderName;

        Dictionary<string, Shader> UnityToSTSShaderMapping;


        public Material referenceMaterial;
        public List<Material> materialExemptions;
        public LayerMask layerMaskToAdd = ~0;

        public Transform[] transformsWithSTS;
        public bool keepMaterialsInSyncWithReference = true;

        public ReplacementGroupType replacementGroupType = ReplacementGroupType.Parent;

        public enum ReplacementGroupType { Parent, Box, Id }

        public string triggerID;
        public TriggerBox triggerBox;

        void Awake()
        {
            if (this.isActiveAndEnabled)
            {
                //seeThroughShaderName = GeneralUtils.getUnityVersionAndRenderPipelineCorrectedShaderString().versionAndRPCorrectedShader;

                //// always null because option for shader selection got removed
                //if (seeThroughShader == null)
                //{
                //    seeThroughShader = Shader.Find(seeThroughShaderName);
                //}
                //else
                //{
                //    seeThroughShaderName = seeThroughShader.name;
                //}

                Dictionary<string, string> UnityToSTSShaderNameMapping = GeneralUtils.getUnityToSTSShaderMapping();

                if(UnityToSTSShaderMapping == null)
                {
                    UnityToSTSShaderMapping = new Dictionary<string, Shader>();
                }
                foreach (string key in UnityToSTSShaderNameMapping.Keys.ToList())
                {
                    Shader shader = Shader.Find(UnityToSTSShaderNameMapping[key]);
                    UnityToSTSShaderMapping[key] = shader ?? Shader.Find(UnityToSTSShaderNameMapping[SeeThroughShaderConstants.STS_SHADER_DEFAULT_KEY]);
                }



                if (parentTransform == null)
                {
                    parentTransform = this.transform;
                }
                doSetupOfAllMaterials(parentTransform);
            }
        }


        //void Start()
        //{
        //    if (this.isActiveAndEnabled)
        //    { 

        //    }
        //}


        void OnEnable()
        {

        }

        private void OnValidate()
        {

        }

        private void LateUpdate()
        {
            if (this.isActiveAndEnabled)
            {
                if (keepMaterialsInSyncWithReference && referenceMaterial != null)
                {
                    //if (transformsWithSTS != null && seeThroughShaderName != null && referenceMaterial != null)
                    if (transformsWithSTS != null && UnityToSTSShaderMapping != null && referenceMaterial != null)
                    {
                        if (transformsWithSTS.Length > 0)
                        {

                            //GeneralUtils.updateSeeThroughShaderMaterialProperties(transformsWithSTS, seeThroughShaderName, referenceMaterial);
                            GeneralUtils.updateSeeThroughShaderMaterialPropertiesAndKeywords(transformsWithSTS, UnityToSTSShaderMapping.Values.ToList(), referenceMaterial);

                        }
                        else
                        {
                            Debug.LogWarning("No matching materials could be found! Please check your setting of the GroupShaderReplacement script on the GameObject with name '" + this.name + "'.");
                        }
                    } 
                }
            }
        }

        private List<GameObject> getGameObjectsDependingOnReplacementGroupType()
        {
            List<GameObject> gameObjects = new List<GameObject>();
            switch (replacementGroupType)
            {
                case ReplacementGroupType.Parent:
                    {
                        AddDescendantsWithTag(parentTransform, gameObjects);
                        break;
                    }
                case ReplacementGroupType.Box:
                    {
                        if(triggerBox != null)
                        {
                            Collider[] hitColliders = triggerBox.GetColliderInsideBox();
                            foreach (Collider collider in hitColliders)
                            {
                                gameObjects.Add(collider.gameObject);
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
                                    gameObjects.Add(item.transform.gameObject);
                                }
                            }
                        }

                        break;
                    }
                default: break;
            }
            return gameObjects;
        }

        private void doSetupOfAllMaterials(Transform parentTransform)
        {
            //if(UnityToSTSShaderMapping == null)
            //{
            //    UnityToSTSShaderMapping = GeneralUtils.getUnityToSTSShaderMapping();
            //}

            //if (seeThroughShaderName == null || seeThroughShaderName.Equals(""))
            //{
            //    if (seeThroughShader != null)
            //    {
            //        seeThroughShaderName = seeThroughShader.name;
            //    }
            //    else
            //    {
            //        seeThroughShaderName = GeneralUtils.getUnityVersionAndRenderPipelineCorrectedShaderString().versionAndRPCorrectedShader;
            //    }
            //}

            List<string> materialNoApplyNames = GeneralUtils.MaterialsNoApplyListToNameList(materialExemptions);

            //List<GameObject> listAllNodeLeafs = new List<GameObject>();


            List<GameObject> gameObjects = getGameObjectsDependingOnReplacementGroupType();

            List<Transform> tmpTransforms = new List<Transform>();
            //bool isHDRP = GeneralUtils.getUnityVersionAndRenderPipelineCorrectedShaderString().renderPipeline == "HDRP";
            //bool isURP = GeneralUtils.getUnityVersionAndRenderPipelineCorrectedShaderString().renderPipeline == "URP";
            foreach (GameObject go in gameObjects)
            {
                if (go.GetComponent<SeeThroughShaderPlayer>() == null || !gameObject.gameObject.GetComponent<SeeThroughShaderPlayer>().isActiveAndEnabled) //&& go.GetComponent<SeeThroughShaderExemption>() == null)
                {
                    Renderer child = go.GetComponent<Renderer>();
                    if (child != null && child.materials.Length > 0)
                    {
                        foreach (Material material in child.gameObject.GetComponent<Renderer>().materials)
                        {
                            if (material != null)
                            {
                                // if material has already SeeThroughShader AND is in no apply list -> add SeeThroughShaderExemption component
                                //if (material.shader == seeThroughShader && materialNoApplyNames != null && materialNoApplyNames.Contains(material.name.Replace(" (Instance)", "")))
                                //{

                                // why should material already have STS?
                                //if (UnityToSTSShaderMapping.ContainsValue(material.shader.name) && materialNoApplyNames != null && materialNoApplyNames.Contains(material.name.Replace(" (Instance)", "")))
                                //{

                                //only mats that don't have STS already applied
                                if ( !UnityToSTSShaderMapping.ContainsValue(material.shader) ) 
                                {
                                    if (materialNoApplyNames != null && materialNoApplyNames.Contains(material.name.Replace(" (Instance)", "")))
                                    {
                                        if (child.gameObject.GetComponent<SeeThroughShaderExemption>() == null)
                                        {
                                            child.gameObject.AddComponent<SeeThroughShaderExemption>();
                                        }
                                    }
                                    // adds SeeThroughShader to materials, depending on layer and if not in materialNoApplyList
                                    else if (((1 << child.gameObject.layer) & layerMaskToAdd) != 0 && (materialNoApplyNames == null || !materialNoApplyNames.Contains(material.name.Replace(" (Instance)", ""))))
                                    {
                                        //if (isHDRP)
                                        //{
                                        //    Material materialClone = new Material(material);
                                        //    material.shader = seeThroughShader;
                                        //    GeneralUtils.adjustHDRPMaterial(materialClone, material);
                                        //}
                                        //else if (isURP)
                                        //{
                                        //    Material materialClone = new Material(material);
                                        //    material.shader = seeThroughShader;
                                        //    GeneralUtils.adjustURPMaterial(materialClone, material);
                                        //}
                                        //else
                                        //{
                                        //    material.shader = seeThroughShader;
                                        //}

                                        material.shader = UnityToSTSShaderMapping.TryGetValue(material.shader.name, out Shader value) ? value : UnityToSTSShaderMapping[SeeThroughShaderConstants.STS_SHADER_DEFAULT_KEY];

                                        //if (isURP)
                                        //{
                                        //    Material materialClone = new Material(material);
                                        //    GeneralUtils.adjustURPMaterial(materialClone, material);
                                        //}

                                        GeneralUtils.RenameInstancedMaterialName(material, referenceMaterial.name);
                                        //material.SetFloat("_RaycastMode", 0);
                                        //material.SetFloat("_TriggerMode", 1);
                                        if (referenceMaterial != null)
                                        {
                                            Texture disTex = referenceMaterial.GetTexture("_DissolveTex");
                                            material.SetTexture("_DissolveTex", disTex);

                                            Texture disMask = referenceMaterial.GetTexture("_DissolveMask");
                                            material.SetTexture("_DissolveMask", disMask);

                                            Texture obstructionCurve = referenceMaterial.GetTexture("_ObstructionCurve");
                                            material.SetTexture("_ObstructionCurve", disMask);

                                            material.SetColorArray("_DissolveColor", referenceMaterial.GetColorArray("_DissolveColor"));


                                            foreach (string propertyName in GeneralUtils.STS_SYNC_PROPERTIES_LIST)
                                            {
                                                if (referenceMaterial.HasProperty(propertyName))
                                                {
                                                    float temp = referenceMaterial.GetFloat(propertyName);
                                                    material.SetFloat(propertyName, temp);

                                                    if (!tmpTransforms.Contains(child.gameObject.transform))
                                                    {
                                                        tmpTransforms.Add(child.gameObject.transform);
                                                    }
                                                }
                                            }

                                            //TODO: ADD KEYWORD STUFF

                                        }
                                    }
                                }

                               
                            }
                        }
                    }
                }
                
            }
            transformsWithSTS = tmpTransforms.ToArray();
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

        //private List<string> materialsNoApplyListToNameList()
        //{
        //    List<string> materialNoApplyNames;
        //    if (materialExemptions != null && materialExemptions.Count > 0)
        //    {
        //        materialNoApplyNames = new List<string>();
        //        foreach (Material mat in materialExemptions)
        //        {
        //            if (!materialNoApplyNames.Contains(mat.name))
        //            {
        //                materialNoApplyNames.Add(mat.name);
        //            }
        //        }
        //        return materialNoApplyNames;
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

    }
}