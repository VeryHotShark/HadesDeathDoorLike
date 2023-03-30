using ShaderCrew.SeeThroughShader;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ShaderCrew.SeeThroughShader
{

    [AddComponentMenu(Strings.COMPONENTMENU_GROUP_SHADER_REPLACEMENT)]
    public class GroupShaderReplacement : GroupReplacementAndSyncBaseAbstract
    {

        protected override bool isReplacement => true;

        //protected override void Awake()
        //{
        //    if (this.isActiveAndEnabled)
        //    {
        //        base.Awake();

        
        //    }
        //}





        //void Start()
        //{
        //    if (this.isActiveAndEnabled)
        //    { 

        //    }
        //}


        //void OnEnable()
        //{

        //}

        //private void OnValidate()
        //{

        //}




        //private void doSetupOfAllMaterials(Transform parentTransform)
        //{
        //    List<string> materialNoApplyNames = GeneralUtils.MaterialsNoApplyListToNameList(materialExemptions);

        //    List<GameObject> gameObjects = getGameObjectsDependingOnReplacementGroupType();

        //    List<Transform> tmpTransforms = new List<Transform>();

        //    foreach (GameObject go in gameObjects)
        //    {
        //        if (go.GetComponent<SeeThroughShaderPlayer>() == null || !gameObject.gameObject.GetComponent<SeeThroughShaderPlayer>().isActiveAndEnabled) //&& go.GetComponent<SeeThroughShaderExemption>() == null)
        //        {
        //            Renderer child = go.GetComponent<Renderer>();
        //            if (child != null && child.materials.Length > 0)
        //            {
        //                foreach (Material material in child.gameObject.GetComponent<Renderer>().materials)
        //                {
        //                    if (material != null)
        //                    {

        //                        //Debug.Log("material.shader: " + material.shader.name + " - " + UnityToSTSShaderMapping.ContainsValue(material.shader));

        //                        //only mats that don't have STS already applied
        //                        if (!UnityToSTSShaderMapping.ContainsValue(material.shader))
        //                        {
        //                            if (materialNoApplyNames != null && materialNoApplyNames.Contains(material.name.Replace(" (Instance)", "")))
        //                            {
        //                                if (child.gameObject.GetComponent<SeeThroughShaderExemption>() == null)
        //                                {
        //                                    child.gameObject.AddComponent<SeeThroughShaderExemption>();
        //                                }
        //                            }
        //                            // adds SeeThroughShader to materials, depending on layer and if not in materialNoApplyList
        //                            else if (((1 << child.gameObject.layer) & layerMaskToAdd) != 0 && (materialNoApplyNames == null || !materialNoApplyNames.Contains(material.name.Replace(" (Instance)", ""))))
        //                            {
        //                                material.shader = UnityToSTSShaderMapping.TryGetValue(material.shader.name, out Shader value) ? value : UnityToSTSShaderMapping[SeeThroughShaderConstants.STS_SHADER_DEFAULT_KEY];

        //                                if (!tmpTransforms.Contains(child.gameObject.transform))
        //                                {
        //                                    tmpTransforms.Add(child.gameObject.transform);
        //                                }

        //                                //// Maybe adjust BaseMap MainTex

        //                                GeneralUtils.RenameInstancedMaterialName(material, referenceMaterial.name);

        //                                //if (referenceMaterial != null)
        //                                //{
        //                                //    Texture disTex = referenceMaterial.GetTexture("_DissolveTex");
        //                                //    material.SetTexture("_DissolveTex", disTex);

        //                                //    Texture disMask = referenceMaterial.GetTexture("_DissolveMask");
        //                                //    material.SetTexture("_DissolveMask", disMask);

        //                                //    Texture obstructionCurve = referenceMaterial.GetTexture("_ObstructionCurve");
        //                                //    material.SetTexture("_ObstructionCurve", disMask);

        //                                //    material.SetColorArray("_DissolveColor", referenceMaterial.GetColorArray("_DissolveColor"));


        //                                //    foreach (string propertyName in GeneralUtils.STS_SYNC_PROPERTIES_LIST)
        //                                //    {
        //                                //        if (referenceMaterial.HasProperty(propertyName))
        //                                //        {
        //                                //            float temp = referenceMaterial.GetFloat(propertyName);
        //                                //            material.SetFloat(propertyName, temp);
        //                                //        }
        //                                //    }
        //                                //}
        //                            }
        //                        }


        //                    }
        //                }
        //            }
        //        }

        //    }
        //    transformsWithSTS = tmpTransforms.ToArray();
        //}

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