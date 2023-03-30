using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ShaderCrew.SeeThroughShader
{
    [AddComponentMenu("")]
    public abstract class ReplacementAndSyncBaseAbstract : MonoBehaviour
    {
        protected Dictionary<string, Shader> UnityToSTSShaderMapping;



        protected virtual void Awake()
        {
            if (this.isActiveAndEnabled)
            {
                ShaderReplacementMappings shaderReplacementMappings = FindObjectOfType<ShaderReplacementMappings>();
                if (shaderReplacementMappings != null)
                {
                    shaderReplacementMappings.Init();
                }
                initializeSTSShaderNameAndSTSShaderMapping();
            }
        }


        protected void initializeSTSShaderNameAndSTSShaderMapping()
        {
            STSCustomShaderMappingsStorage sTSCustomShaderMappingStorage = STSCustomShaderMappingsStorage.Instance;
            Dictionary<string, string> customShaderMappings = sTSCustomShaderMappingStorage.STSCustomShaderMappingsDict;
            Dictionary<string, string> nativeShaderMappings = GeneralUtils.getUnityToSTSShaderMapping();
            Dictionary<string, string> UnityToSTSShaderNameMapping = customShaderMappings;

            UnityToSTSShaderNameMapping = UnityToSTSShaderNameMapping.Concat(nativeShaderMappings.Where(x => !UnityToSTSShaderNameMapping.ContainsKey(x.Key))).ToDictionary(x => x.Key, x => x.Value);

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
    }
}