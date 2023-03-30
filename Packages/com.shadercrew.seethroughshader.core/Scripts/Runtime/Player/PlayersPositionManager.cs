using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ShaderCrew.SeeThroughShader
{
    [AddComponentMenu(Strings.COMPONENTMENU_PLAYER_POSITION_MANAGER)]
    public class PlayersPositionManager : MonoBehaviour
    {
        int _PlayersPos_ID;
        int _ArrayLength_ID;


        Vector4[] playersPositions = new Vector4[SeeThroughShaderConstants.PLAYERS_POS_ARRAY_LENGTH];


        // Start is called before the first frame update
        public List<GameObject> playableCharacters;
        private List<GameObject> playableCharactersDestinct;

        public bool isInitialized = false;
        public bool effectOffIfPlayerDisabled = false;


        private ZoneDataStorage zoneDataStorage = ZoneDataStorage.Instance;


        private PlayersDataStorage playersDataStorage = PlayersDataStorage.Instance;

        void OnEnable()
        {
            init();
        }

        void OnDisable()
        {
            isInitialized = false;
            Shader.SetGlobalVectorArray(_PlayersPos_ID, new Vector4[SeeThroughShaderConstants.PLAYERS_POS_ARRAY_LENGTH]);
            Shader.SetGlobalFloat(_ArrayLength_ID, 0);
        }


        void Start()
        {
            init();
        }

        void OnDestroy()
        {
            isInitialized = false;
            Shader.SetGlobalVectorArray(_PlayersPos_ID, new Vector4[SeeThroughShaderConstants.PLAYERS_POS_ARRAY_LENGTH]);
            Shader.SetGlobalFloat(_ArrayLength_ID, 0);
        }

        // Update is called once per frame
        void Update()
        {
            UpdatePlayerPosShaderVectorArray();
            //zoneDataStorage.UpdateZonesDatasShaderFloatArray(); // moved inside individual zones

        }

        //private void Reset()
        //{
        //    if (SeeThroughShaderStatus.playerPositionMangerHolder != null)
        //    {
        //        this.enabled = false;
        //        Debug.Log("PlayerPositionManager is already used on GameObject " + SeeThroughShaderStatus.playerPositionMangerHolder.name);
        //    }
        //    else
        //    {
        //        SeeThroughShaderStatus.playerPositionMangerHolder = this.gameObject;
        //    }
        //}

        private void init()
        {
            if (!isInitialized)
            {
                _PlayersPos_ID = Shader.PropertyToID(SeeThroughShaderConstants.PLAYERS_POS_SHADER_PROPERTY_NAME);
                _ArrayLength_ID = Shader.PropertyToID(SeeThroughShaderConstants.PLAYERS_POS_ARRAY_COUNT_PROPERTY_NAME);


                if (playableCharacters.Count > 0)
                {

                    foreach (GameObject gplayer in playableCharacters)
                    {
                        if (gplayer.activeSelf)
                        {
                            if (gplayer.GetComponent<SeeThroughShaderPlayer>() == null)
                            {
                                gplayer.AddComponent<SeeThroughShaderPlayer>();
                            }
                        }

                    }

                    // better performances but doesn't enable to have changes register during play mode
                    // clean up duplicates
                    playableCharactersDestinct = playableCharacters.Distinct().ToList();
                    // clean up null elements
                    playableCharactersDestinct.RemoveAll(item => item == null);
                    // only have active players affect STS
                    playableCharactersDestinct.RemoveAll(item => item.activeSelf == false);

                    UpdatePlayerPosShaderVectorArray();
                    PlayersDataStorage.Instance.GeneratePlayerDataForAllPlayerPositions();
                }


            }

            isInitialized = true;
        }

        public void RemovePlayerAtRuntime(GameObject player)
        {
            if (player != null)
            {
                bool updateRequired = false;
                if (playableCharactersDestinct.Contains(player))
                {                    
                    playableCharactersDestinct.Remove(player);
                    updateRequired = true;
                }

                if (updateRequired)
                {
                    if(playableCharacters != null && playableCharacters.Contains(player))
                    {
                        playableCharacters.Remove(player);
                    }
                    UpdatePlayerPosShaderVectorArray();
                    PlayersDataStorage playersDataStorage = PlayersDataStorage.Instance;
                    playersDataStorage.RemovePlayerDataInstancedID(player.transform.GetInstanceID());
                    // TODO: could add removing ID also
                    playersDataStorage.UpdatePlayersDatasShaderFloatArray();
        
                }
            }
        }

        public void AddPlayerAtRuntime(GameObject player) 
        {
            if (player != null)
            {
                if (player.activeSelf)
                {
                    if (player.GetComponent<SeeThroughShaderPlayer>() == null)
                    {
                        player.AddComponent<SeeThroughShaderPlayer>();
                    }
                }

                bool updateRequired = false;
                if(playableCharactersDestinct == null)
                {
                    playableCharactersDestinct = new List<GameObject>();
                    playableCharactersDestinct.Add(player);
                    updateRequired = true;
                } else if (!playableCharactersDestinct.Contains(player))
                {
                    playableCharactersDestinct.Add(player);
                    updateRequired = true;                    
                }

                if(updateRequired)
                {
                    if(playableCharacters==null)
                    {
                        playableCharacters = new List<GameObject>();
                    }
                    playableCharacters.Add(player);
                    UpdatePlayerPosShaderVectorArray();
                    PlayersDataStorage.Instance.GeneratePlayerDataForSpecificPlayerPositions(player.transform.GetInstanceID(), playableCharactersDestinct.Count-1);
                }

            }
        }

        private void UpdatePlayerPosShaderVectorArray()
        {
            //// clean up duplicates
            //playableCharactersDestinct = playableCharacters.Distinct().ToList();
            //// clean up null elements
            //playableCharactersDestinct.RemoveAll(item => item == null);
            //playableCharactersDestinct.RemoveAll(item => item.activeSelf == false);

            if (playableCharactersDestinct != null)
            {
                //Vector4[] playersPositions = new Vector4[100];
                for (int i = 0; i < playersPositions.Length; i++)
                {
                    playersPositions[i] = Vector4.zero;
                }

                for (int i = 0; i < playableCharactersDestinct.Count; i++)
                {
                    if (!effectOffIfPlayerDisabled)
                    {
                        if (playableCharactersDestinct[i] != null)
                        {
                            //playersPositions[i] = new Vector4(playableCharactersDestinct[i].transform.position.x,
                            //                                  playableCharactersDestinct[i].transform.position.y,
                            //                                  playableCharactersDestinct[i].transform.position.z,
                            //                                  playableCharactersDestinct[i].transform.GetInstanceID());
                            playersPositions[i].x = playableCharactersDestinct[i].transform.position.x;
                            playersPositions[i].y = playableCharactersDestinct[i].transform.position.y;
                            playersPositions[i].z = playableCharactersDestinct[i].transform.position.z;
                            //playersPositions[i].w = playableCharactersDestinct[i].transform.GetInstanceID();
                            playersPositions[i].w = playersDataStorage.GetPlayerID(playableCharactersDestinct[i].transform.GetInstanceID());


                        }
                    }
                    else
                    {
                        if (playableCharactersDestinct[i] != null && playableCharactersDestinct[i].gameObject.activeSelf)
                        {
                            //playersPositions[i] = new Vector4(playableCharactersDestinct[i].transform.position.x,
                            //                                  playableCharactersDestinct[i].transform.position.y,
                            //                                  playableCharactersDestinct[i].transform.position.z,
                            //                                  playableCharactersDestinct[i].transform.GetInstanceID());
                           
                            playersPositions[i].x = playableCharactersDestinct[i].transform.position.x;
                            playersPositions[i].y = playableCharactersDestinct[i].transform.position.y;
                            playersPositions[i].z = playableCharactersDestinct[i].transform.position.z;
                            playersPositions[i].w = playersDataStorage.GetPlayerID(playableCharactersDestinct[i].transform.GetInstanceID());
                        }
                        else if (playableCharactersDestinct[i] != null)
                        {
                            playersPositions[i].w = -playersDataStorage.GetPlayerID(playableCharactersDestinct[i].transform.GetInstanceID());
                        }
                    }

                }
                if (playableCharactersDestinct.Count > 0)
                {
                    Shader.SetGlobalVectorArray(_PlayersPos_ID, playersPositions);
                    Shader.SetGlobalFloat(_ArrayLength_ID, playableCharactersDestinct.Count);
                }
                else
                {
                    Shader.SetGlobalFloat(_ArrayLength_ID, 0);
                }
            }
            else
            {
                Shader.SetGlobalFloat(_ArrayLength_ID, 0);
            }

        }

    }
}