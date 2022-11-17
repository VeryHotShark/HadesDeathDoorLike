using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShaderCrew.SeeThroughShader
{
    public sealed class ZoneDataStorage
    {
        private static Dictionary<int, ZoneData> _ZoneDataDict = new Dictionary<int, ZoneData>();

        private static bool updateNecessary = true;
        private ZoneDataStorage()
        {
        }

        public Dictionary<int, ZoneData> ZoneDataDict
        {
            get { return _ZoneDataDict; }
        }

        public void AddOrUpdateZoneData(ZoneData zoneData)
        {
            if (zoneData != null)
            {
                if (!_ZoneDataDict.ContainsKey(zoneData.zoneInstanceID))
                {
                    _ZoneDataDict.Add(zoneData.zoneInstanceID, zoneData);
                }
                else
                {
                    _ZoneDataDict[zoneData.zoneInstanceID] = zoneData;
                }
                updateNecessary = true;
            }
            else
            {
                updateNecessary = false;
            }
        }

        public void RemoveZoneData(ZoneData zoneData)
        {
            if (zoneData != null)
            {
                if (_ZoneDataDict.ContainsKey(zoneData.zoneInstanceID))
                {
                    _ZoneDataDict.Remove(zoneData.zoneInstanceID);
                }
                updateNecessary = true;
            }
            else
            {
                updateNecessary = false;
            }
        }

        // not used
        //public ZoneData GetZoneData(int zoneInstanceID)
        //{
        //    if (_ZoneDataDict.TryGetValue(zoneInstanceID, out ZoneData zoneData))
        //    {
        //        return zoneData;
        //    }
        //    return null;
        //}


        public void UpdateZonesDatasShaderFloatArray()
        {
            if (updateNecessary && _ZoneDataDict.Values.Count >= 0)
            {
                float[] zonesDataFloatArray = new float[SeeThroughShaderConstants.ZONES_DATA_ARRAY_LENGTH];

                if (_ZoneDataDict.Values.Count > 0)
                {
                    List<float> floatList = new List<float>();
                    foreach (ZoneData zoneData in _ZoneDataDict.Values)
                    {
                        floatList.AddRange(zoneData.ToShaderFloatArraySegment());
                        //Debug.Log("ID: " + zoneData.id);
                    }


                    if (floatList.Count > zonesDataFloatArray.Length)
                    {
                        Debug.LogWarning("ZonesData has gotten too big. Please contact the ShaderCrew Support.");
                    }
                    else
                    {
                        floatList.CopyTo(zonesDataFloatArray);
                    }
                }

                Shader.SetGlobalFloatArray(SeeThroughShaderConstants.ZONES_DATA_SHADER_PROPERTY_NAME, zonesDataFloatArray);
                Shader.SetGlobalFloat(SeeThroughShaderConstants.ZONES_DATA_COUNT_SHADER_PROPERTY_NAME, _ZoneDataDict.Values.Count);
                updateNecessary = false;
            }
        }


        public static ZoneDataStorage Instance { get { return Nested.instance; } }

        private class Nested
        {
            static Nested()
            {
            }
            internal static readonly ZoneDataStorage instance = new ZoneDataStorage();
        }
    }
}