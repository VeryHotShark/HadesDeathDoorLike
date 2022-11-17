using System.Collections.Generic;
using UnityEngine;

namespace ShaderCrew.SeeThroughShader
{
    public class PlayerData
    {
        public int PlayerIndex { get; set; }
        public int PlayerID { get; set; }
        public Dictionary<int, TransitionData> TransitionDataDict { get; set; }

        public Dictionary<int, TransitionData> ZoneTransitionDataDict { get; set; }

        //public PlayerData()
        //{
        //    init();
        //}

        public PlayerData(int PlayerID)
        {
            this.PlayerID = PlayerID;
            init();
        }

        public PlayerData(int PlayerID, int PlayerIndex)
        {
            this.PlayerID = PlayerID;
            this.PlayerIndex = PlayerIndex;
            init();
        }

        private void init()
        {
            this.TransitionDataDict = new Dictionary<int, TransitionData>();
            this.ZoneTransitionDataDict = new Dictionary<int, TransitionData>();
        }


        public TransitionData GetTransitionData(int matID)
        {
            return GetTransitionDataFromDictionary(TransitionDataDict, matID);
        }

        public TransitionData GetZoneTransitionData(int zoneID)
        {
            return GetTransitionDataFromDictionary(ZoneTransitionDataDict, zoneID);
        }

        private TransitionData GetTransitionDataFromDictionary(Dictionary<int, TransitionData> dict, int id)
        {
            if (dict.TryGetValue(id, out TransitionData transitionData))
            {
                return transitionData;
            }
            else
            {
                return new TransitionData(id);
            }
        }


        public void RemoveZoneTransitionData(ZoneData zoneData)
        {
            if (zoneData != null)
            {
                if (ZoneTransitionDataDict.ContainsKey(zoneData.id))
                {
                    ZoneTransitionDataDict.Remove(zoneData.id);
                }
            }
        }


        public void AddOrUpdateTransitionData(TransitionData transitionData)
        {
            AddOrUpdateTransitionDataDictionary(TransitionDataDict, transitionData);
        }

        public void AddOrUpdateZoneTransitionData(TransitionData transitionData)
        {
            AddOrUpdateTransitionDataDictionary(ZoneTransitionDataDict, transitionData);
        }

        private void AddOrUpdateTransitionDataDictionary(Dictionary<int, TransitionData> dict, TransitionData transitionData)
        {
            if (!dict.ContainsKey(transitionData.matID))
            {
                dict.Add(transitionData.matID, transitionData);
            }
            else
            {
                dict[transitionData.matID] = transitionData;
            }
        }


        public void RemoveOldTransitionData()
        {
            RemoveOldTransitionDataFromDictionary(TransitionDataDict);
        }

        public void RemoveOldZoneTransitionData()
        {
            RemoveOldTransitionDataFromDictionary(TransitionDataDict);
        }

        private void RemoveOldTransitionDataFromDictionary(Dictionary<int, TransitionData> dict)
        {
            if (dict.Count > 0)
            {
                List<int> keys = new List<int>(dict.Keys);
                foreach (int key in keys)
                {
                    if (Time.timeSinceLevelLoad - dict[key].tValue > dict[key].tDuration && dict[key].direction == -1)
                    {
                        dict.Remove(key);
                    }
                }
            }
        }


        public float[] ToShaderFloatArraySegment()
        {
            List<float> list = new List<float>();
            list.Add(PlayerID);
            list.Add(PlayerIndex);
            if (TransitionDataDict.Count > 0)
            {
                list.Add(TransitionDataDict.Count);
            }
            else
            {
                list.Add(0);
            }

            foreach (TransitionData transitionData in TransitionDataDict.Values)
            {
                list.AddRange(transitionData.ToShaderFloatArraySegment());
            }

            if (ZoneTransitionDataDict.Count > 0)
            {
                list.Add(ZoneTransitionDataDict.Count);
            }
            else
            {
                list.Add(0);
            }

            foreach (TransitionData zoneTransitionData in ZoneTransitionDataDict.Values)
            {
                list.AddRange(zoneTransitionData.ToShaderFloatArraySegment());
            }

            return list.ToArray();
        }
    }
}