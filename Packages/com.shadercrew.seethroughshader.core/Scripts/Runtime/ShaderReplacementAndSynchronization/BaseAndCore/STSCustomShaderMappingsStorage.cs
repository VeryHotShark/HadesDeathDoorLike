using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ShaderCrew.SeeThroughShader.ShaderReplacementMappings;

namespace ShaderCrew.SeeThroughShader
{
    public class STSCustomShaderMappingsStorage
    {
        public static Dictionary<string, string> _STSCustomShaderMappingsDict = new Dictionary<string, string>();
        public static List<string> _AllSTSShaders = new List<string>();
        private STSCustomShaderMappingsStorage()
        {
            Shader[] allShaders = Resources.FindObjectsOfTypeAll<Shader>();

            foreach (Shader item in allShaders)
            {
                if (item.FindPropertyIndex(SeeThroughShaderConstants.STS_SHADER_IDENTIFIER_PROPERTY) != -1 && item.name != "Master")
                {
                    if (!GeneralUtils.STS_SHADER_LIST.Contains(item.name))
                    {
                        if (!AllSTSShaders.Contains(item.name))
                        {
                            _STSCustomShaderMappingsDict.Add(item.name, item.name);
                        }
                    }
                    if(!AllSTSShaders.Contains(item.name))
                    {
                        AllSTSShaders.Add(item.name);
                    }
                }
            }
        }

        public Dictionary<string, string> STSCustomShaderMappingsDict
        {
            get { return _STSCustomShaderMappingsDict; }
        }

        public List< string> AllSTSShaders
        {
            get { return _AllSTSShaders; }
        }

        //public void AddCustomShaderMapping(string customShader)
        //{
        //    if (customShader != null && customShader != "")
        //    {
        //        if (!STSCustomShaderMappingDict.Contains(customShader))
        //        {
        //            _STSCustomShaderList.Add(customShader);
        //        }
        //    }
        //}

        public void UpdateCustomShaderMappings(List<ShaderNameMapping> shaderNameMappings)
        {
            if (shaderNameMappings != null && shaderNameMappings.Count > 0)
            {
                foreach (ShaderNameMapping item in shaderNameMappings)
                {
                    if (!STSCustomShaderMappingsDict.ContainsKey(item.initialShader))
                    {
                        _STSCustomShaderMappingsDict.Add(item.initialShader, item.replacementShader);
                    }
                    else
                    {
                        _STSCustomShaderMappingsDict[item.initialShader] = item.replacementShader;
                    }
                }

            }
        }

        public static STSCustomShaderMappingsStorage Instance { get { return Nested.instance; } }

        private class Nested
        {
            static Nested()
            {
            }
            internal static readonly STSCustomShaderMappingsStorage instance = new STSCustomShaderMappingsStorage();
        }
    }
}