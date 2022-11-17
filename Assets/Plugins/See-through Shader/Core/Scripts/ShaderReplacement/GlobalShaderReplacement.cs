using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Rendering;

namespace ShaderCrew.SeeThroughShader
{
    [AddComponentMenu(Strings.COMPONENTMENU_GLOBAL_SHADER_REPLACEMENT)]
    public class GlobalShaderReplacement : MonoBehaviour
    {
        public Dictionary<Material, Shader> cachedOriginalShaders = new Dictionary<Material, Shader>();
        public Shader replacementShader;
        private string seeThroughShaderName;

        public Material referenceMaterial;

        public bool replacementByReplacementShader;
        public LayerMask layerMasksWithReplacement = ~0;

        public Camera replacementCamera;

        Dictionary<string, Shader> UnityToSTSShaderMapping;

        Transform[] transformsWithSTS;
        void Update()
        {
            if (this.isActiveAndEnabled)
            {
                updateGlobalShaderVariables();
                updateShaderKeywords();
            }
        }

        void Awake()
        {
            if (this.isActiveAndEnabled)
            {
                initializeSTSShaderNameAndSTSShaderMapping();
                applyReplacementShader();
                updateGlobalShaderVariables();
                updateShaderKeywords();
            }

        }

        private void initializeSTSShaderNameAndSTSShaderMapping()
        {
            seeThroughShaderName = GeneralUtils.getUnityVersionAndRenderPipelineCorrectedShaderString().versionAndRPCorrectedShader;

            // always replacementShader  because option for shader selection got removed
            if (replacementShader == null)
            {
                replacementShader = Shader.Find(seeThroughShaderName);
            }
            else
            {
                seeThroughShaderName = replacementShader.name;
            }

            Dictionary<string, string> UnityToSTSShaderNameMapping = GeneralUtils.getUnityToSTSShaderMapping();


            if (UnityToSTSShaderMapping == null)
            {
                UnityToSTSShaderMapping = new Dictionary<string, Shader>();
            }
            foreach (string key in UnityToSTSShaderNameMapping.Keys.ToList())
            {
                Shader shader = Shader.Find(UnityToSTSShaderNameMapping[key]);
                UnityToSTSShaderMapping[key] = shader ?? Shader.Find(UnityToSTSShaderNameMapping[SeeThroughShaderConstants.STS_SHADER_DEFAULT_KEY]);
            }
        }


        private void OnValidate()
        {
            if (this.isActiveAndEnabled)
            {
                updateGlobalShaderVariables();
                updateShaderKeywords();

                if (replacementByReplacementShader)
                {
                    if (replacementCamera == null && GetComponent<Camera>() != null)
                    {
                        replacementCamera = GetComponent<Camera>();
                    }
                }

            }
        }
        void OnEnable()
        {
            if (this.isActiveAndEnabled)
            {
                initializeSTSShaderNameAndSTSShaderMapping();

                applyReplacementShader();

                if(replacementByReplacementShader)
                {
                    if (GetComponent<Camera>() != null)
                    {
                        replacementCamera = GetComponent<Camera>();
                    }
                }

            }

        }

        // only necessary when using [ExecuteInEditMode]
        //void OnDisable()
        //{

        //    //resetReplacementShadersAndSwappedShaders();
        //}

        //private void OnDestroy()
        //{
        //    //resetReplacementShadersAndSwappedShaders();
        //}

        //void OnApplicationQuit()
        //{
        //    //resetReplacementShadersAndSwappedShaders();
        //}



        private void applyReplacementShader()
        {
            //if (replacementShader != null)
            //{
            Shader.SetGlobalFloat(SeeThroughShaderConstants.PROPERTY_IS_REPLACEMENT_SHADER, 1);

            if (replacementByReplacementShader && replacementShader != null)
            {
                if (replacementCamera != null)
                {
                    replacementCamera.SetReplacementShader(replacementShader, "RenderType");
                }
            }
            else
            {
                applyReplacementShaderToGameObjectsMaterials(listAllGameObjectsFromLayerMask(layerMasksWithReplacement));
            }

            //}
        }

        private List<GameObject> listAllGameObjectsFromLayerMask(LayerMask layerMask)
        {
            GameObject[] gameObjectArray = FindObjectsOfType(typeof(GameObject)) as GameObject[];
            List<GameObject> gameObjectList = new List<GameObject>();
            for (int i = 0; i < gameObjectArray.Length; i++)
            {
                if (doesLayerMaskContainLayer(layerMask, gameObjectArray[i].layer))
                {
                    gameObjectList.Add(gameObjectArray[i]);
                }
            }

            //Debug.Log("gameObjectList.Count: " + gameObjectList.Count);
            if (gameObjectList.Count == 0)
            {
                return null;
            }
            return gameObjectList;
        }

        private bool doesLayerMaskContainLayer(LayerMask mask, int layer)
        {
            return mask == (mask | (1 << layer));
        }

        private void applyReplacementShaderToGameObjectsMaterials(List<GameObject> gameObjects)
        {
            //if (seeThroughShaderName != null && !seeThroughShaderName.Equals(""))
            //{
            //    if (replacementShader != null)
            //    {
            //        seeThroughShaderName = replacementShader.name;
            //    }
            //    else
            //    {
            //        seeThroughShaderName = GeneralUtils.getUnityVersionAndRenderPipelineCorrectedShaderString().versionAndRPCorrectedShader;

            //    }
            //}
            if (gameObjects != null && gameObjects.Count > 0)
            {
                //bool isHDRP = GeneralUtils.getUnityVersionAndRenderPipelineCorrectedShaderString().renderPipeline == "HDRP";
                //bool isURP = GeneralUtils.getUnityVersionAndRenderPipelineCorrectedShaderString().renderPipeline == "URP";

                List<Transform> tmpTransformsWithSTS = new List<Transform>();
                foreach (GameObject gameObject in gameObjects)
                {
                    if (gameObject != null && gameObject.GetComponent<Renderer>() != null)
                    {
                        if (!tmpTransformsWithSTS.Contains(gameObject.transform))
                        {
                            tmpTransformsWithSTS.Add(gameObject.transform);
                        }

                        Material[] materials = gameObject.GetComponent<Renderer>().materials;
                        for (int i = 0; i < materials.Length; i++)
                        {
                            //if (materials[i].shader.name != seeThroughShaderName)
                            if (!UnityToSTSShaderMapping.ContainsValue(materials[i].shader))
                            {                                
                                if (!cachedOriginalShaders.ContainsKey(materials[i]))
                                {
                                    cachedOriginalShaders.Add(materials[i], materials[i].shader);
                                    //string replacementKeyword = SeeThroughShaderConstants.KEYWORD_REPLACEMENT;
                                    //if (isHDRP)
                                    //{
                                    //    Material materialClone = new Material(materials[i]);
                                    //    materials[i].shader = replacementShader;
                                    //    materials[i].SetFloat(SeeThroughShaderConstants.PROPERTY_IS_REPLACEMENT_SHADER, 1);
                                    //    materials[i].EnableKeyword(SeeThroughShaderConstants.KEYWORD_REPLACEMENT);
                                    //    GeneralUtils.adjustHDRPMaterial(materialClone, materials[i]);
                                    //}
                                    //else if (isURP)
                                    //{
                                    //    Material materialClone = new Material(materials[i]);
                                    //    materials[i].shader = replacementShader;
                                    //    materials[i].SetFloat(SeeThroughShaderConstants.PROPERTY_IS_REPLACEMENT_SHADER, 1);
                                    //    materials[i].EnableKeyword(SeeThroughShaderConstants.KEYWORD_REPLACEMENT);
                                    //    GeneralUtils.adjustURPMaterial(materialClone, materials[i]);
                                    //}
                                    //else
                                    //{
                                    //    materials[i].shader = replacementShader;
                                    //    materials[i].SetFloat(SeeThroughShaderConstants.PROPERTY_IS_REPLACEMENT_SHADER, 1);
                                    //    materials[i].EnableKeyword(SeeThroughShaderConstants.KEYWORD_REPLACEMENT);
                                    //}

                                    materials[i].shader = UnityToSTSShaderMapping.TryGetValue(materials[i].shader.name, out Shader value) ? value : UnityToSTSShaderMapping[SeeThroughShaderConstants.STS_SHADER_DEFAULT_KEY];
                                    materials[i].SetFloat(SeeThroughShaderConstants.PROPERTY_IS_REPLACEMENT_SHADER, 1);
                                    materials[i].EnableKeyword(SeeThroughShaderConstants.KEYWORD_REPLACEMENT);

                                    //if (isURP)
                                    //{
                                    //    Material materialClone = new Material(materials[i]);
                                    //    GeneralUtils.adjustURPMaterial(materialClone, materials[i]);
                                    //}

                                    GeneralUtils.RenameInstancedMaterialName(materials[i], referenceMaterial.name);
                                }
                            }

                        }
                    }

                }
                if(tmpTransformsWithSTS.Count > 0)
                {
                    transformsWithSTS = tmpTransformsWithSTS.ToArray();
                }
            }
        }



        private void updateGlobalShaderVariables()
        {
            if (referenceMaterial != null)
            {
                Texture disTex = referenceMaterial.GetTexture(SeeThroughShaderConstants.PROPERTY_DISSOLVE_TEX);
                Shader.SetGlobalTexture(SeeThroughShaderConstants.PROPERTY_DISSOLVE_TEX_GLOBAL, disTex);

                Texture dissolveMask = referenceMaterial.GetTexture(SeeThroughShaderConstants.PROPERTY_DISSOLVE_MASK);
                Shader.SetGlobalTexture(SeeThroughShaderConstants.PROPERTY_DISSOLVE_MASK_GLOBAL, dissolveMask);

                Texture obstructionCurve = referenceMaterial.GetTexture(SeeThroughShaderConstants.PROPERTY_OBSTRUCTION_CURVE);
                Shader.SetGlobalTexture(SeeThroughShaderConstants.PROPERTY_OBSTRUCTION_CURVE_GLOBAL, obstructionCurve);

                Shader.SetGlobalColor(SeeThroughShaderConstants.PROPERTY_DISSOLVE_COLOR_GLOBAL, referenceMaterial.GetColor(SeeThroughShaderConstants.PROPERTY_DISSOLVE_COLOR));
                foreach (string propertyName in GeneralUtils.STS_SYNC_PROPERTIES_LIST)
                {
                    if (referenceMaterial.HasProperty(propertyName))
                    {
                        float temp = referenceMaterial.GetFloat(propertyName);
                        Shader.SetGlobalFloat(propertyName + SeeThroughShaderConstants.PROPERTY_GLOBAL, temp);
                    }
                }
            }
        }



        private void updateShaderKeywords()
        {
            if (referenceMaterial != null)
            {
                if (transformsWithSTS != null && transformsWithSTS.Length > 0 && UnityToSTSShaderMapping != null)
                {

                    //GeneralUtils.updateSeeThroughShaderMaterialProperties(transformsWithSTS, seeThroughShaderName, referenceMaterial);
                    GeneralUtils.updateSeeThroughShaderMaterialPropertiesAndKeywords(transformsWithSTS, UnityToSTSShaderMapping.Values.ToList(), referenceMaterial);

                }
            }
        }

        // currently not used
        //private void resetReplacementShadersAndSwappedShaders()
        //{
        //    Shader.SetGlobalFloat("_IsReplacementShader", 0);
        //    GetComponent<Camera>().ResetReplacementShader();
        //    if (cachedOriginalShaders.Count > 0)
        //    {
        //        foreach (KeyValuePair<Material, Shader> entry in cachedOriginalShaders)
        //        {
        //            entry.Key.shader = entry.Value;
        //        }
        //        cachedOriginalShaders.Clear();

        //    }
        //}
    }
}