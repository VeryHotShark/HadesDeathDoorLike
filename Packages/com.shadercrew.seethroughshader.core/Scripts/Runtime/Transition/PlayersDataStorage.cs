using System.Collections.Generic;
using UnityEngine;

namespace ShaderCrew.SeeThroughShader
{
    public sealed class PlayersDataStorage
    {
        private static Dictionary<int, PlayerData> _PlayerDataDict = new Dictionary<int, PlayerData>();

        private static int _LastPlayerID = 2344;
        private static Dictionary<int, int> _PlayerIDMapping = new Dictionary<int, int>();

        //private static bool updateNecessary = true;
        private PlayersDataStorage()
        {
        }

        public Dictionary<int, PlayerData> PlayerDataDict
        {
            get { return _PlayerDataDict; }
        }


        private int getNextPlayerID
        {
            get { return _LastPlayerID++; }
        }

        public int GetPlayerID(int instancedID)
        {
            if (_PlayerIDMapping.TryGetValue(instancedID, out int playerID))
            {
                return playerID;
            }
            else
            {
                playerID = getNextPlayerID;
                _PlayerIDMapping.Add(instancedID, playerID);
                return playerID;
            }
        }


        public void GeneratePlayerDataForAllPlayerPositions()
        {
            Vector4[] playerPosArray = Shader.GetGlobalVectorArray(SeeThroughShaderConstants.PLAYERS_POS_SHADER_PROPERTY_NAME);
            int arrayLength = (int)Shader.GetGlobalFloat((SeeThroughShaderConstants.PLAYERS_POS_ARRAY_COUNT_PROPERTY_NAME));

            if (playerPosArray != null && playerPosArray.Length > 0 && _PlayerDataDict.Count <= 0 && arrayLength > 0)
            {
                for (int i = 0; i < arrayLength; i++)
                {
                    _PlayerDataDict.Add(Mathf.Abs((int)playerPosArray[i][3]), new PlayerData(Mathf.Abs((int)playerPosArray[i][3]), i));
                }
                UpdatePlayersDatasShaderFloatArray();
            }
        }

        public void GeneratePlayerDataForSpecificPlayerPositions(int instancedId, int index)
        {
            _PlayerDataDict.Add(GetPlayerID(instancedId), new PlayerData(GetPlayerID(instancedId), index));
            UpdatePlayersDatasShaderFloatArray();
        }

        public void AddOrUpdatePlayerData(PlayerData playerData)
        {
            if (playerData != null)
            {
                if (!_PlayerDataDict.ContainsKey(Mathf.Abs(playerData.PlayerID)))
                {
                    _PlayerDataDict.Add(Mathf.Abs(playerData.PlayerID), playerData);
                }
                else
                {
                    _PlayerDataDict[Mathf.Abs(playerData.PlayerID)] = playerData;
                }

                //updateNecessary = true;
            }
            //else
            //{
            //    updateNecessary = false;
            //}

        }

        public void RemovePlayerData(PlayerData playerData)
        {
            if (playerData != null)
            {
                RemovePlayerData(Mathf.Abs(playerData.PlayerID));
            }
        }

        public void RemovePlayerData(int playerID)
        {
            playerID = Mathf.Abs(playerID);
            if (_PlayerDataDict.ContainsKey(playerID))
            {
                _PlayerDataDict.Remove(playerID);
            }            
        }

        public void RemovePlayerDataInstancedID(int instancedID)
        {
            int playerID = GetPlayerID(instancedID);
            RemovePlayerData(playerID);
        }

        public PlayerData GetPlayerData(int playerID)
        {
            playerID = Mathf.Abs(playerID);
            if (_PlayerDataDict.TryGetValue(playerID, out PlayerData playerData))
            {
                return playerData;
            }
            else
            {
                return new PlayerData(playerID);
            }
        }


        public PlayerData GetPlayerDataInstancedID(int instancedID)
        {
            int playerID = GetPlayerID(instancedID);
            return GetPlayerData(playerID);
        }

        public void RemoveAllTransitionDatasFromZone(ZoneData zoneData)
        {
            if (zoneData != null)
            {
                foreach (PlayerData playerData in _PlayerDataDict.Values)
                {
                    playerData.RemoveZoneTransitionData(zoneData);

                }
            }
        }
        //public void AddTransitionData(TransitionData transitionData, int playerID)
        //{
        //    if (_PlayerDataDict.TryGetValue(playerID, out PlayerData playerData))
        //    {
        //        playerData.AddOrUpdateTransitionData(transitionData);
        //        //playerData.RemoveOldTransitionData();  //maybe do this for all transitionDatas
        //        _PlayerDataDict[playerID] = playerData; //shouldn't be necessary cause reference
        //    }
        //    else
        //    {
        //        Debug.LogWarning("Player with instanceID: " + playerID + " has NO playerData assigned");
        //    }

        //}


        public void UpdatePlayersDatasShaderFloatArray()
        {
            //if(updateNecessary)
            //{
            Vector4[] playerPosArray = Shader.GetGlobalVectorArray(SeeThroughShaderConstants.PLAYERS_POS_SHADER_PROPERTY_NAME);
            float arrayLength = Shader.GetGlobalFloat("_ArrayLength");
            List<float> floatList = new List<float>();
            for (int i = 0; i < arrayLength; i++)
            {
                if (_PlayerDataDict.TryGetValue(Mathf.Abs((int)playerPosArray[i][3]), out PlayerData playerData))
                {
                    playerData.PlayerIndex = i;
                    playerData.RemoveOldTransitionData();
                    floatList.AddRange(playerData.ToShaderFloatArraySegment());
                    //Debug.Log("playerData with id: " + playerData.PlayerID + ":" + " index: " + playerData.PlayerIndex);
                    //foreach (TransitionData transitionData in playerData.TransitionDataDict.Values)
                    //{
                    //    Debug.Log("matID: " + transitionData.matID + ", tValue: " + transitionData.tValue + ", direction: " + transitionData.direction + " maxDuration: " + transitionData.tDuration);
                    //}
                }
                else
                {
                    Debug.LogWarning("Player with instanceID: " + (int)playerPosArray[i][3] + " has NO playerData assigned");
                }
            }

            float[] playerDataFloatArray = new float[SeeThroughShaderConstants.PLAYERS_DATA_ARRAY_LENGTH];
            if (floatList.Count > playerDataFloatArray.Length)
            {
                Debug.LogWarning("PlayerData has gotten too big. Please contact the ShaderCrew Support.");
            }
            else
            {
                floatList.CopyTo(playerDataFloatArray);
            }
            Shader.SetGlobalFloatArray(SeeThroughShaderConstants.PLAYERS_DATA_SHADER_PROPERTY_NAME, playerDataFloatArray);

            //    updateNecessary = false;
            //}

        }


        public static PlayersDataStorage Instance { get { return Nested.instance; } }

        private class Nested
        {
            static Nested()
            {
            }
            internal static readonly PlayersDataStorage instance = new PlayersDataStorage();
        }
    }
}