using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ShaderCrew.SeeThroughShader
{
    public class TransitionController
    {
        private List<Material> materials;

        //private string currentMode;

        public TransitionController()
        { }

        //public TransitionController(Transform transform)
        //{

        //    initTrigger(transform);
        //}

        public TransitionController(Transform transform)
        {
            List<GameObject> dummyList = new List<GameObject>();
            dummyList.Add(transform.gameObject);
            initTrigger(dummyList);
        }

        public TransitionController(List<GameObject> gameObjects)
        {
            initTrigger(gameObjects);
        }
        public TransitionController(List<Material> listMaterial)
        {
            initTrigger(listMaterial);

        }

        //private void initTrigger(Transform transform)
        //{

        //    Renderer[] renderers = transform.GetComponentsInChildren<Renderer>();
        //    for (int i = 0; i < renderers.Length; i++)
        //    {
        //        if (renderers[i] != null && renderers[i].materials.Length > 0)
        //        {
        //            for (int j = 0; j < renderers[i].materials.Length; j++)
        //            {
        //                setSeeThroughShaderMaterialToTriggerMode(renderers[i].materials[j]);
        //            }
        //        }
        //    }

        //    //currentMode = "Trigger";
        //}



        //private void initTrigger(Transform transform)
        //{
        //    Debug.Log("initTriggerinitTriggerinitTriggerinitTrigger"    );
        //    Dictionary<string, Material> materialTracker = new Dictionary<string, Material>();

        //    Renderer[] renderers = transform.GetComponentsInChildren<Renderer>();
        //    for (int i = 0; i < renderers.Length; i++)
        //    {
        //        if (renderers[i] != null && renderers[i].materials.Length > 0)
        //        {
        //            Material[] updatedMaterials = renderers[i].materials;
        //            for (int j = 0; j < renderers[i].materials.Length; j++)
        //            {
        //                if (isSeeThroughShaderMaterial(renderers[i].materials[j]))
        //                {
        //                    if (!renderers[i].materials[j].name.Contains(SeeThroughShaderConstants.STS_INSTANCE_PREFIX))
        //                    {
        //                        GeneralUtils.AddSTSTriggerPrefix(renderers[i].materials[j]);
        //                    }

        //                    string name = renderers[i].materials[j].name;
        //                    name = name.Replace(" (Instance)", "");
        //                    //if (!name.Contains("Trigger by " + transform.name))
        //                    //{
        //                    //    name += " - Trigger by " + transform.name;
        //                    //}

        //                    renderers[i].materials[j].name = name;
        //                }

        //                if (!materialTracker.ContainsKey(renderers[i].materials[j].name))
        //                {
        //                    Material mat = new Material(renderers[i].materials[j]);
        //                    //mat.name = mat.name.Replace(" (Instance)", "");
        //                    setSeeThroughShaderMaterialToTriggerMode(mat);
        //                    materialTracker.Add(renderers[i].materials[j].name, mat);
        //                }
        //                updatedMaterials[j] = materialTracker[renderers[i].materials[j].name];

        //            }

        //            renderers[i].materials = updatedMaterials;
        //        }

        //    }


        //}



        private void initTrigger(List<GameObject> gameObjects)
        {
            Dictionary<string, Material> materialTracker = new Dictionary<string, Material>();

            foreach (GameObject item in gameObjects)
            {
                Renderer[] renderers = item.GetComponentsInChildren<Renderer>();
                for (int i = 0; i < renderers.Length; i++)
                {
                    if (renderers[i] != null && renderers[i].materials.Length > 0)
                    {
                        Material[] updatedMaterials = renderers[i].materials;
                        for (int j = 0; j < renderers[i].materials.Length; j++)
                        {
                            if (isSeeThroughShaderMaterial(renderers[i].materials[j]))
                            {
                                if (!renderers[i].materials[j].name.Contains(SeeThroughShaderConstants.STS_INSTANCE_PREFIX))
                                {
                                    GeneralUtils.AddSTSTriggerPrefix(renderers[i].materials[j]);
                                }

                                string name = renderers[i].materials[j].name;
                                name = name.Replace(" (Instance)", "");
                                //if (!name.Contains("Trigger by " + transform.name))
                                //{
                                //    name += " - Trigger by " + transform.name;
                                //}

                                renderers[i].materials[j].name = name;
                            }

                            if (!materialTracker.ContainsKey(renderers[i].materials[j].name))
                            {
                                Material mat = new Material(renderers[i].materials[j]);
                                setSeeThroughShaderMaterialToTriggerMode(mat);
                                materialTracker.Add(renderers[i].materials[j].name, mat);
                            }
                            updatedMaterials[j] = materialTracker[renderers[i].materials[j].name];

                        }

                        renderers[i].materials = updatedMaterials;
                    }
                }
            }

        }

        private void initTrigger(List<Material> listMaterial)
        {
            foreach (Material mat in listMaterial)
            {
                setSeeThroughShaderMaterialToTriggerMode(mat);
            }
            //currentMode = "Trigger";
        }

        // called from BuildingAutoDetector
        public void setupAutoDetect(Transform transform)
        {
            Renderer[] renderers = transform.GetComponentsInChildren<Renderer>();
            for (int i = 0; i < renderers.Length; i++)
            {
                if (renderers[i] != null && renderers[i].materials.Length > 0)
                {
                    for (int j = 0; j < renderers[i].materials.Length; j++)
                    {
                        setSeeThroughShaderMaterialToRaycastMode(renderers[i].materials[j]);
                    }
                }
            }

            //currentMode = "Raycast";
        }

        private void resetMaterials()
        {
            if (materials != null && materials.Count > 0)
            {
                foreach (Material mat in materials)
                {
                    mat.SetFloat("_RaycastMode", 0);
                    mat.SetFloat("_TriggerMode", 0);
                }
                materials.Clear();
            }
        }

        public void destroy()
        {
            resetMaterials();
        }

        private void setSeeThroughShaderMaterialToTriggerMode(Material mat)
        {

            if (isSeeThroughShaderMaterial(mat))
            {
                //Debug.Log("setSeeThroughShaderMaterialToTriggerMode mat name: " + mat);
                mat.SetFloat("_RaycastMode", 0);
                mat.SetFloat("_TriggerMode", 1);
                if (materials == null)
                {
                    materials = new List<Material>();
                }
                materials.Add(mat);
            }
        }


        private void setSeeThroughShaderMaterialToRaycastMode(Material mat)
        {
            if (isSeeThroughShaderMaterial(mat))
            {
                mat.SetFloat("_RaycastMode", 1);
                mat.SetFloat("_TriggerMode", 0);
                if (materials == null)
                {
                    materials = new List<Material>();
                }
                materials.Add(mat);
            }
        }

        private bool isSeeThroughShaderMaterial(Material mat)
        {
            //if (mat != null && mat.shader != null && GeneralUtils.STS_SHADER_LIST.Contains(mat.shader.name))
            if (mat != null && mat.shader != null && STSCustomShaderMappingsStorage.Instance.AllSTSShaders.Contains(mat.shader.name))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void notifyOnTriggerEnter(List<Material> materialList, Transform player = null)
        {

            if (player == null)
            {
                transitionEffect(materialList, 1);
                //Debug.Assert(false, "Sorry, from now on you have to supply the transform of the playable character that collided with the trigger!");
            }
            else
            {
                transitionEffect(materialList, 1, player);
            }

        }

        public void notifyOnTriggerExit(List<Material> materialList, Transform player = null)
        {
            if (player == null)
            {
                transitionEffect(materialList, -1);
                //Debug.Assert(false, "Sorry, from now on you have to supply the transform of the playable character that collided with the trigger!");
            }
            else
            {
                transitionEffect(materialList, -1, player);
            }
        }


        public void notifyOnTriggerEnter(Transform transform, Transform player = null)
        {
            notifyOnTriggerEnter(getMaterialList(transform), player);
        }

        public void notifyOnTriggerExit(Transform transform, Transform player = null)
        {
            notifyOnTriggerExit(getMaterialList(transform), player);
        }


        public void notifyOnTriggerEnter(List<GameObject> gameObjects, Transform player = null)
        {
            notifyOnTriggerEnter(getMaterialList(gameObjects), player);
        }

        public void notifyOnTriggerExit(List<GameObject> gameObjects, Transform player = null)
        {
            notifyOnTriggerExit(getMaterialList(gameObjects), player);
        }


        //private List<Material> getMaterialList(Transform transform)
        //{
        //    List<Material> materialList = new List<Material>();
        //    Renderer[] renderers = transform.GetComponentsInChildren<Renderer>();
        //    //Renderer[] renderers = transform.root.GetComponentsInChildren<Renderer>();
        //    if (renderers != null && renderers.Length > 0)
        //    {
        //        for (int i = 0; i < renderers.Length; i++)
        //        {
        //            if (renderers[i] != null && renderers[i].materials.Length > 0)
        //            {
        //                for (int j = 0; j < renderers[i].materials.Length; j++)
        //                {
        //                    if (isSeeThroughShaderMaterial(renderers[i].materials[j]))
        //                    {
        //                        materialList.Add(renderers[i].materials[j]);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    return materialList;
        //}


        private List<Material> getMaterialList(Transform transform)
        {
            List<Material> materialList = new List<Material>();
            AddAllSTSMaterialsFromGameObjectToList(transform.gameObject, materialList);
            return materialList;
        }

        private List<Material> getMaterialList(List<GameObject> gameObjects)
        {
            List<Material> materialList = new List<Material>();

            foreach (GameObject item in gameObjects)
            {
                AddAllSTSMaterialsFromGameObjectToList(item, materialList);
            }
            return materialList;
        }

        private void AddAllSTSMaterialsFromGameObjectToList(GameObject gameObject, List<Material> materialList)
        {
            Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();
            //Renderer[] renderers = transform.root.GetComponentsInChildren<Renderer>();
            if (renderers != null && renderers.Length > 0)
            {
                for (int i = 0; i < renderers.Length; i++)
                {
                    if (renderers[i] != null && renderers[i].sharedMaterials.Length > 0)
                    {
                        for (int j = 0; j < renderers[i].sharedMaterials.Length; j++)
                        {
                            if (isSeeThroughShaderMaterial(renderers[i].sharedMaterials[j]))
                            {
                                if(!materialList.Contains(renderers[i].sharedMaterials[j]))
                                {
                                    materialList.Add(renderers[i].sharedMaterials[j]);
                                }

                            }
                        }
                    }
                }
            }
        }




        public static void transitionEffectZones(float direction, Transform player, SeeThroughShaderZone seeThroughShaderZone)
        {
            Debug.Assert(Math.Abs(direction) == 1, "direction argument needs to be either 1 or -1");

            //ZoneDataStorage zoneDataStorage = ZoneDataStorage.Instance;

            float zoneID = seeThroughShaderZone.zoneData.id;
            //float zoneTValue = 0;

            int numOfPlayersInside = seeThroughShaderZone.numOfPlayersInside;
            if ((numOfPlayersInside + direction) >= 0)
            {
                numOfPlayersInside += (int)direction;
                seeThroughShaderZone.numOfPlayersInside = numOfPlayersInside;
            }
            else
            {
                seeThroughShaderZone.numOfPlayersInside = 0;
            }

            float duration = seeThroughShaderZone.transitionDuration;

            PlayersDataStorage playersDataStorage = PlayersDataStorage.Instance;
            PlayerData playerData = playersDataStorage.GetPlayerDataInstancedID(player.transform.GetInstanceID());
            TransitionData transitionData = playerData.GetZoneTransitionData((int)zoneID);

            transitionData.CalculateTValue();
            transitionData.direction = (int)direction;
            transitionData.tDuration = duration;
            playerData.AddOrUpdateZoneTransitionData(transitionData);

            playersDataStorage.AddOrUpdatePlayerData(playerData);
            playersDataStorage.UpdatePlayersDatasShaderFloatArray();

            //Debug.Log("zoneID: " + zoneID + ", zoneTValue: " + transitionData.tValue + ", zoneDirection: " + direction  + ", time: " + Time.timeSinceLevelLoad);

        }


        public static void transitionEffectZones(float direction, SeeThroughShaderZone seeThroughShaderZone)
        {
            Debug.Assert(Math.Abs(direction) == 1, "direction argument needs to be either 1 or -1");

            ZoneDataStorage zoneDataStorage = ZoneDataStorage.Instance;

            int numOfPlayersInside = seeThroughShaderZone.numOfPlayersInside;
            if ((numOfPlayersInside + direction) >= 0)
            {
                numOfPlayersInside += (int)direction;
                seeThroughShaderZone.numOfPlayersInside = numOfPlayersInside;
            }
            else
            {
                seeThroughShaderZone.numOfPlayersInside = 0;
            }

            float duration = seeThroughShaderZone.transitionDuration;

            TransitionData transitionData = seeThroughShaderZone.zoneData.transitionData;


            transitionData.CalculateTValue();
            transitionData.direction = (int)direction;
            transitionData.tDuration = duration;

            seeThroughShaderZone.UpdateZoneData();

            //Debug.Log("zoneID: " + zoneID + ", zoneTValue: " + transitionData.tValue + ", zoneDirection: " + direction  + ", time: " + Time.timeSinceLevelLoad);

        }

        private void transitionEffect(List<Material> materialList, float direction, Transform player = null)
        {
            //foreach (Material item in materialList)
            //{
            //    Debug.Log(item.name);
            //}
            if (materialList.Count > 0)
            {            
                float id = GeneralUtils.getFirstPropertyValueFoundInMaterialList(materialList, "_id");

                PlayersDataStorage playersDataStorage = null;
                PlayerData playerData = null;

                if (player != null)
                {
                    playersDataStorage = PlayersDataStorage.Instance;
                    playerData = playersDataStorage.GetPlayerDataInstancedID(player.transform.GetInstanceID());
                }

                float generatedId = id;
                //float tValue = 0;
                float maxDuration = 0;

                TransitionData transitionData = null;
                foreach (Material mat in materialList)
                {
                    if (!mat.HasProperty("_id") || mat.GetFloat("_id") == 0)
                    {
                        if (generatedId == 0)
                        {
                            generatedId = IdGenerator.Instance.Id;
                        }
                        mat.SetFloat("_id", generatedId);
                    }
                    else
                    {
                        if (generatedId == 0)
                        {
                            generatedId = mat.GetFloat("_id");
                        }
                        else
                        {
                            Debug.Assert(generatedId == mat.GetFloat("_id"), "Materials have different Ids! Bug?");
                        }

                    }

                    float numOfPlayersInside = 0;
                    if (mat.HasProperty("_numOfPlayersInside"))
                    {
                        numOfPlayersInside = mat.GetFloat("_numOfPlayersInside");
                    }
                    if ((numOfPlayersInside + direction) >= 0)
                    {
                        numOfPlayersInside += direction;
                        mat.SetFloat("_numOfPlayersInside", numOfPlayersInside);
                    }
                    else
                    {
                        mat.SetFloat("_numOfPlayersInside", 0);
                    }

                    float duration;
                    if (mat.GetFloat("_IsReplacementShader") == 1)
                    {
                        duration = Shader.GetGlobalFloat("_TransitionDurationGlobal");
                    }
                    else
                    {
                        duration = mat.GetFloat("_TransitionDuration");
                    }
                    maxDuration = Math.Max(duration, maxDuration);

                    Debug.Assert(Math.Abs(direction) == 1, "direction argument needs to be either 1 or -1");

                    if (player != null && playerData != null)
                    {
                        if (transitionData == null)
                        {
                            transitionData = playerData.GetTransitionData((int)generatedId);
                            transitionData.CalculateTValue();
                            transitionData.direction = (int)direction;
                            transitionData.tDuration = maxDuration;
                            playerData.AddOrUpdateTransitionData(transitionData);
                        }

                        if (transitionData != null)
                        {
                            mat.SetFloat("_tValue", transitionData.tValue);
                            mat.SetFloat("_tDirection", direction);
                        }
                    }
                    else
                    {
                        float tValue = mat.GetFloat("_tValue");
                        //if (tValue != 0 && Time.timeSinceLevelLoad - tValue < maxDuration)
                        //{
                        //    float lastTValue = tValue;
                        //    tValue = 2 * Time.timeSinceLevelLoad - lastTValue - maxDuration;
                        //}
                        //else
                        //{
                        //    tValue = Time.timeSinceLevelLoad;
                        //}

                        tValue = TransitionUtils.CalculateTValue(tValue, duration);
                        mat.SetFloat("_tValue", tValue);
                        mat.SetFloat("_tDirection", direction);
                    }
                }

                if (player != null && playerData != null && playersDataStorage != null)
                {
                    playersDataStorage.AddOrUpdatePlayerData(playerData);
                    playersDataStorage.UpdatePlayersDatasShaderFloatArray();
                }

            }
        }

    }
}