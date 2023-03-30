using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShaderCrew.SeeThroughShader
{
    public class STSCameraMode : MonoBehaviour
    {
        // Minimum Y, GO won't go below this point
        public float minY;
        // Max distance between camera and GO
        public float camDistance = 20;

        public bool noMaxRestriction = false;
        //public bool dynamicScaledCollider = true;


        private float currentZ;
        //BoxCollider boxCollider;
        //private float origCenterZ;
        //private float origLocalScaleZ;
        public bool keepEnteredWithinDist = false;
        public GameObject virtualPlayerTemplate;
        //public LayerMask MovementMask;
        //public float raycastLength = 100;
        public float keepEnteredDistance = 50;
        //public List<GameObject> listBuildings;
        private PlayersPositionManager playerPosMgr;
        public GameObject playerPosMgrGO;
        private List<GameObject> listTempPlayerGO = new List<GameObject>();
        private Dictionary<GameObject, GameObject> listCamCubesAttached = new Dictionary<GameObject, GameObject>();


        void Start()
        {
            transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, camDistance));
            currentZ = camDistance;
            if (playerPosMgrGO != null && playerPosMgrGO.GetComponent<PlayersPositionManager>() != null)
            {
                playerPosMgr = playerPosMgrGO.GetComponent<PlayersPositionManager>();
            }

            if (keepEnteredWithinDist && playerPosMgrGO != null && virtualPlayerTemplate != null)
            {

                for (int i = 0; i < 99; i++)
                {
                    if (this.transform != null)
                    {
                        GameObject tempGO = Instantiate(virtualPlayerTemplate, playerPosMgrGO.transform);
                        //PlayerPosMgr.playableCharacters.Add(tempGO);
                        //PlayerPosMgr.init();
                        playerPosMgr.AddPlayerAtRuntime(tempGO);
                        listTempPlayerGO.Add(tempGO);
                    }
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
            Vector3 camGO = Camera.main.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, camDistance));
            if (Vector3.Dot(Vector3.down, camGO) > 0)
            {
                //this.gameObject.GetComponent<Renderer>().material.color = Color.red;
            }
            else
            {
                float z = Camera.main.transform.position.y / (Vector3.Dot(Vector3.down, camGO) / (Vector3.Magnitude(Vector3.down) * Vector3.Magnitude(camGO)));
                z *= -1;
                if (!noMaxRestriction)
                {
                    z = Mathf.Min(z, camDistance);
                }

                currentZ = z;
            }
            transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, currentZ));

            //if(dynamicScaledCollider)
            //{
            //    boxCollider.size = new Vector3(transform.localScale.x, transform.localScale.y, currentZ);
            //    boxCollider.center = new Vector3(boxCollider.center.x, boxCollider.center.y, -currentZ / 2);
            //} else
            //{
            //    boxCollider.size = new Vector3(transform.localScale.x, transform.localScale.y, origLocalScaleZ);
            //    boxCollider.center = new Vector3(boxCollider.center.x, boxCollider.center.y, origCenterZ);
            //}

            if (keepEnteredWithinDist)
            {


                GameObject removeGO = null;
                GameObject removeCamCubeGO = null;

                foreach (GameObject item in listCamCubesAttached.Keys)
                {


                    if (listCamCubesAttached.ContainsKey(item))
                    {
                        if (Vector3.Distance(Camera.main.transform.position, listCamCubesAttached[item].transform.position) > keepEnteredDistance)
                        {
                            removeGO = item;
                            removeCamCubeGO = listCamCubesAttached[item];
                        }
                    }

                }

                if (removeGO != null && removeCamCubeGO != null)
                {

                    removeCamCubeGO.transform.position = Vector3.zero;
                    removeCamCubeGO.transform.parent = playerPosMgrGO.transform;
                    listTempPlayerGO.Add(removeCamCubeGO);
                    listCamCubesAttached.Remove(removeGO);
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (keepEnteredWithinDist)
            {


                if (other.gameObject.GetComponent<TriggerByParent>() != null)
                {


                    if (other.transform.gameObject.GetComponent<TriggerByParent>() != null && other.transform.gameObject.GetComponent<TriggerByParent>().isDedicatedEnterExitTrigger == false)
                    {

                        if (Vector3.Distance(Camera.main.transform.position, other.transform.gameObject.GetComponent<Collider>().bounds.center) <= keepEnteredDistance)
                        {
                            if (listCamCubesAttached.ContainsKey(other.transform.gameObject) == false)
                            {
                                GameObject newCamCube = listTempPlayerGO[0];
                                newCamCube.transform.position = other.transform.gameObject.GetComponent<Collider>().bounds.center;
                                newCamCube.transform.parent = other.transform;
                                listTempPlayerGO.Remove(listTempPlayerGO[0]);
                                listCamCubesAttached.Add(other.transform.gameObject, newCamCube);
                            }

                        }
                    }


                    GameObject removeGO = null;
                    GameObject removeCamCubeGO = null;

                    foreach (GameObject item in listCamCubesAttached.Keys)
                    {


                        if (listCamCubesAttached.ContainsKey(item))
                        {
                            if (Vector3.Distance(Camera.main.transform.position, listCamCubesAttached[item].transform.position) > keepEnteredDistance)
                            {
                                removeGO = item;
                                removeCamCubeGO = listCamCubesAttached[item];
                            }
                        }

                    }

                    if (removeGO != null && removeCamCubeGO != null)
                    {

                        removeCamCubeGO.transform.position = Vector3.zero;
                        removeCamCubeGO.transform.parent = playerPosMgrGO.transform;
                        listTempPlayerGO.Add(removeCamCubeGO);
                        listCamCubesAttached.Remove(removeGO);
                    }
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (keepEnteredWithinDist)
            {


                GameObject removeGO = null;
                GameObject removeCamCubeGO = null;

                foreach (GameObject item in listCamCubesAttached.Keys)
                {


                    if (listCamCubesAttached.ContainsKey(item))
                    {
                        if (Vector3.Distance(Camera.main.transform.position, listCamCubesAttached[item].transform.position) > keepEnteredDistance)
                        {
                            removeGO = item;
                            removeCamCubeGO = listCamCubesAttached[item];
                        }
                    }

                }

                if (removeGO != null && removeCamCubeGO != null)
                {

                    removeCamCubeGO.transform.position = virtualPlayerTemplate.transform.position;
                    removeCamCubeGO.transform.parent = playerPosMgrGO.transform;
                    listTempPlayerGO.Add(removeCamCubeGO);
                    listCamCubesAttached.Remove(removeGO);
                }
            }
        }
    }
}