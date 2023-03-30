using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShaderCrew.SeeThroughShader
{
    [AddComponentMenu(Strings.COMPONENTMENU_SHADER_PROPERTY_SYNC)]
    public class ShaderPropertySync : GroupReplacementAndSyncBaseAbstract
    {


        private Transform[] listNodesLeafs;
        public bool syncContinuus = true;

        private string seeThroughShaderName;



        private Dictionary<Material, string> tempMaterials;

        protected override bool isReplacement => false;

        //protected override void Awake()
        //{
        //    if (this.isActiveAndEnabled)
        //    {
        //        base.Awake();
        //        doSetupOfAllMaterials();

        //    }
        //}

        //void OnDisable()
        //{
        //    if(tempMaterials!=null)
        //    {
        //        foreach (KeyValuePair<Material, string> item in tempMaterials)
        //        {
        //            item.Key.name = item.Value;
        //        }
        //    }

        //}


        //private void doSetupOfAllMaterials(Transform parentTransform)
        //{
        //    if(tempMaterials==null)
        //    {
        //        tempMaterials = new Dictionary<Material, string>();
        //    }

        //    Dictionary<string, Material> materialTracker = new Dictionary<string, Material>();


        //    List<GameObject> gameObjects = getGameObjectsDependingOnReplacementGroupType();

        //    List<Transform> tmpTransforms = new List<Transform>();

        //    foreach (GameObject go in gameObjects)
        //    {

        //        Renderer renderer = go.GetComponent<Renderer>();
        //        if (renderer != null && renderer.materials.Length > 0)
        //        {
        //            foreach (Material material in renderer.gameObject.GetComponent<Renderer>().materials)
        //            {
        //                if (material != null)
        //                {

        //                    //Debug.Log("material.shader: " + material.shader.name + " - " + UnityToSTSShaderMapping.ContainsValue(material.shader));

        //                    //only mats that have STS already applied
        //                    if (UnityToSTSShaderMapping.ContainsValue(material.shader))
        //                    {
        //                        if (materialNoApplyNames != null && materialNoApplyNames.Contains(material.name.Replace(" (Instance)", "")))
        //                        {
        //                            if (renderer.gameObject.GetComponent<SeeThroughShaderExemption>() == null)
        //                            {
        //                                renderer.gameObject.AddComponent<SeeThroughShaderExemption>();
        //                            }
        //                        }
        //                        // adds SeeThroughShader to materials, depending on layer and if not in materialNoApplyList
        //                        else if (((1 << renderer.gameObject.layer) & layerMaskToAdd) != 0 && (materialNoApplyNames == null || !materialNoApplyNames.Contains(material.name.Replace(" (Instance)", ""))))
        //                        {



        //                            //if(!tempMaterials.ContainsKey(material))
        //                            //{
        //                            //    tempMaterials.Add(material,material.name);
        //                            //    GeneralUtils.RenameInstancedMaterialNameSynchronization(material, referenceMaterial.name);
        //                            //}

        //                            //if (!tmpTransforms.Contains(renderer.gameObject.transform))
        //                            //{
        //                            //    tmpTransforms.Add(renderer.gameObject.transform);
        //                            //}




        //                            //if (referenceMaterial != null)
        //                            //{
        //                            //    Texture disTex = referenceMaterial.GetTexture("_DissolveTex");
        //                            //    material.SetTexture("_DissolveTex", disTex);

        //                            //    Texture disMask = referenceMaterial.GetTexture("_DissolveMask");
        //                            //    material.SetTexture("_DissolveMask", disMask);

        //                            //    Texture obstructionCurve = referenceMaterial.GetTexture("_ObstructionCurve");
        //                            //    material.SetTexture("_ObstructionCurve", disMask);

        //                            //    material.SetColorArray("_DissolveColor", referenceMaterial.GetColorArray("_DissolveColor"));


        //                            //    foreach (string propertyName in GeneralUtils.STS_SYNC_PROPERTIES_LIST)
        //                            //    {
        //                            //        if (referenceMaterial.HasProperty(propertyName))
        //                            //        {
        //                            //            float temp = referenceMaterial.GetFloat(propertyName);
        //                            //            material.SetFloat(propertyName, temp);

        //                            //            if (!tmpTransforms.Contains(renderer.gameObject.transform))
        //                            //            {
        //                            //                tmpTransforms.Add(renderer.gameObject.transform);
        //                            //            }
        //                            //        }
        //                            //    }

        //                            //    //TODO: ADD KEYWORD STUFF

        //                            //}
        //                        }
        //                    }


        //                }
        //            }
        //        }


        //    }
        //    transformsWithSTS = tmpTransforms.ToArray();
        //}


        //private void doSetupOfAllMaterials(Transform parentTransform)
        //{
        //    Dictionary<string, Material> materialTracker = new Dictionary<string, Material>();

        //    List<GameObject> gameObjects = getGameObjectsDependingOnReplacementGroupType();
        //    List<Transform> tmpTransforms = new List<Transform>();

        //    foreach (GameObject go in gameObjects)
        //    {
        //        Renderer[] renderers = go.GetComponentsInChildren<Renderer>();
        //        for (int i = 0; i < renderers.Length; i++)
        //        {
        //            if (renderers[i] != null && renderers[i].materials.Length > 0)
        //            {
        //                Material[] updatedMaterials = renderers[i].materials;
        //                for (int j = 0; j < renderers[i].materials.Length; j++)
        //                {
        //                    if (renderers[i].materials[j] != null)
        //                    {

        //                        //Debug.Log("material.shader: " + material.shader.name + " - " + UnityToSTSShaderMapping.ContainsValue(material.shader));

        //                        //only mats that have STS already applied
        //                        if (UnityToSTSShaderMapping.ContainsValue(renderers[i].materials[j].shader))
        //                        {
        //                            if (materialNoApplyNames != null && materialNoApplyNames.Contains(renderers[i].materials[j].name.Replace(" (Instance)", "")))
        //                            {
        //                                if (renderers[i].gameObject.GetComponent<SeeThroughShaderExemption>() == null)
        //                                { 
        //                                    renderers[i].gameObject.AddComponent<SeeThroughShaderExemption>();
        //                                }
        //                            }
        //                            // adds SeeThroughShader to materials, depending on layer and if not in materialNoApplyList
        //                            else if (((1 << renderers[i].gameObject.layer) & layerMaskToAdd) != 0 && (materialNoApplyNames == null || !materialNoApplyNames.Contains(renderers[i].materials[j].name.Replace(" (Instance)", ""))))
        //                            {
        //                                GeneralUtils.AddSTSInstancePrefix(renderers[i].materials[j], referenceMaterial.name);
        //                                string name = renderers[i].materials[j].name.Replace(" (Instance)", "");
        //                                if (!name.Contains("Sync by " + referenceMaterial.name))
        //                                {
        //                                    name += " - Sync by " + referenceMaterial.name;
        //                                }
        //                                renderers[i].materials[j].name = name;

        //                                if (!materialTracker.ContainsKey(renderers[i].materials[j].name))
        //                                {
        //                                    //Debug.Log("ContainsKey[j]: " + j + " . " + renderers[i].materials[j].name);
        //                                    Material mat = new Material(renderers[i].materials[j]);
        //                                    //GeneralUtils.RenameInstancedMaterialNameSynchronization(mat, referenceMaterial.name);
        //                                    materialTracker.Add(renderers[i].materials[j].name, mat);
        //                                }
        //                                updatedMaterials[j] = materialTracker[renderers[i].materials[j].name];
        //                                //Debug.Log("updatedMaterials[j]: " + j + " . " + renderers[i].materials[j].name);
        //                                //if(!tempMaterials.ContainsKey(material))
        //                                //{
        //                                //    tempMaterials.Add(material,material.name);
        //                                //    GeneralUtils.RenameInstancedMaterialNameSynchronization(material, referenceMaterial.name);
        //                                //}
                                        
        //                                if (!tmpTransforms.Contains(renderers[i].gameObject.transform))
        //                                {
        //                                    tmpTransforms.Add(renderers[i].gameObject.transform);
        //                                }
        //                            }
        //                        }
        //                    }                           

        //                }
        //                renderers[i].materials = updatedMaterials;
        //            }
        //        }


        //    }
        //    transformsWithSTS = tmpTransforms.ToArray();
        //}


        //void Start()
        //{
        //    if (this.isActiveAndEnabled)
        //    {
        //        //seeThroughShaderName = GeneralUtils.getUnityVersionAndRenderPipelineCorrectedShaderString().versionAndRPCorrectedShader;


        //        if (referenceMaterial != null && children != null && children.Count > 0)
        //        {
        //            foreach (string propertyName in GeneralUtils.STS_SYNC_PROPERTIES_LIST)
        //            {
        //                float temp = referenceMaterial.GetFloat(propertyName);

        //                foreach (Material child in children)
        //                {
        //                    child.SetFloat(propertyName, temp);
        //                }

        //            }
        //            foreach (Material child in children)
        //            {
        //                Texture disTex = referenceMaterial.GetTexture("_DissolveTex");
        //                if (disTex != null)
        //                    child.SetTexture("_DissolveTex", disTex);

        //                Texture dissolveMask = referenceMaterial.GetTexture("_DissolveMask");
        //                if (dissolveMask != null)
        //                    child.SetTexture("_DissolveMask", dissolveMask);

        //                Texture obstructionCurve = referenceMaterial.GetTexture("_ObstructionCurve");
        //                if (obstructionCurve != null)
        //                    child.SetTexture("_ObstructionCurve", obstructionCurve);

        //                child.SetColorArray("_DissolveColor", referenceMaterial.GetColorArray("_DissolveColor"));
        //            }

        //        }
        //        listNodesLeafs = transform.GetComponentsInChildren<Transform>();
        //    }

        //}

        // Update is called once per frame



        //private void LateUpdate()
        //{
        //    if (this.isActiveAndEnabled)
        //    {
        //        if (syncContinuus && referenceMaterial != null)
        //        {
        //            //GeneralUtils.updateSeeThroughShaderMaterialProperties(listNodesLeafs, seeThroughShaderName, referenceMaterial);
        //            GeneralUtils.updateSeeThroughShaderMaterialPropertiesAndKeywords(listNodesLeafs, seeThroughShaderName, referenceMaterial);
        //        }
        //    }
        //}

    }
}