using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace ShaderCrew.SeeThroughShader
{
    [AddComponentMenu(Strings.COMPONENTMENU_SHADER_REPLACEMENT_MAPPINGS)]
    public class ShaderReplacementMappings : MonoBehaviour
    {

        bool initialized = false;
        [System.Serializable]
        public class ShaderNameMapping
        {

            public string initialShader;

            public string replacementShader;

            public ShaderNameMapping()
            {

            }

            public ShaderNameMapping(string initalShader, string replacementShader)
            {
                this.initialShader = initalShader;
                this.replacementShader = replacementShader;
            }
        }
        public List<ShaderNameMapping> customShadersReplacementMappings = new List<ShaderNameMapping>();

        STSCustomShaderMappingsStorage sTSCustomShaderStorage;
        void Awake()
        {
            sTSCustomShaderStorage = STSCustomShaderMappingsStorage.Instance;

            if (customShadersReplacementMappings == null)
            {
                //customShaderRegistrationList = new List<Shader>();
            }
            else
            {
                sTSCustomShaderStorage.UpdateCustomShaderMappings(customShadersReplacementMappings);
                initialized = true;
            }
        }

        public void Init()
        {
            if(!initialized)
            {
                sTSCustomShaderStorage = STSCustomShaderMappingsStorage.Instance;
                if (customShadersReplacementMappings != null)
                {
                    sTSCustomShaderStorage.UpdateCustomShaderMappings(customShadersReplacementMappings);
                    initialized = true;
                }
            }


        }

    }
}