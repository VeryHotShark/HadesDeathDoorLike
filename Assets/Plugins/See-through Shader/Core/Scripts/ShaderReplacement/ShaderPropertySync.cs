using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShaderCrew.SeeThroughShader
{
    [AddComponentMenu(Strings.COMPONENTMENU_SHADER_PROPERTY_SYNC)]
    public class ShaderPropertySync : MonoBehaviour
    {

        public Material referenceMaterial;
        public List<Material> children;
        private Transform[] listNodesLeafs;
        public bool syncContinuus = true;

        private string seeThroughShaderName;

        void Start()
        {
            if (this.isActiveAndEnabled)
            {
                seeThroughShaderName = GeneralUtils.getUnityVersionAndRenderPipelineCorrectedShaderString().versionAndRPCorrectedShader;


                if (referenceMaterial != null && children != null && children.Count > 0)
                {
                    foreach (string propertyName in GeneralUtils.STS_SYNC_PROPERTIES_LIST)
                    {
                        float temp = referenceMaterial.GetFloat(propertyName);

                        foreach (Material child in children)
                        {
                            child.SetFloat(propertyName, temp);
                        }

                    }
                    foreach (Material child in children)
                    {
                        Texture disTex = referenceMaterial.GetTexture("_DissolveTex");
                        if (disTex != null)
                            child.SetTexture("_DissolveTex", disTex);

                        Texture dissolveMask = referenceMaterial.GetTexture("_DissolveMask");
                        if (dissolveMask != null)
                            child.SetTexture("_DissolveMask", dissolveMask);

                        Texture obstructionCurve = referenceMaterial.GetTexture("_ObstructionCurve");
                        if (obstructionCurve != null)
                            child.SetTexture("_ObstructionCurve", obstructionCurve);

                        child.SetColorArray("_DissolveColor", referenceMaterial.GetColorArray("_DissolveColor"));
                    }

                }
                listNodesLeafs = transform.GetComponentsInChildren<Transform>();
            }

        }

        // Update is called once per frame
        void Update()
        {

        }


        private void LateUpdate()
        {
            if (this.isActiveAndEnabled)
            {
                if (syncContinuus && referenceMaterial != null)
                {
                    //GeneralUtils.updateSeeThroughShaderMaterialProperties(listNodesLeafs, seeThroughShaderName, referenceMaterial);
                    GeneralUtils.updateSeeThroughShaderMaterialPropertiesAndKeywords(listNodesLeafs, seeThroughShaderName, referenceMaterial);
                }
            }
        }

    }
}