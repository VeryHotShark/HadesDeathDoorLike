using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShaderCrew.SeeThroughShader
{
    public class STSCameraModeRaycast : MonoBehaviour
    {
        public GameObject cameraCube;
        public LayerMask MovementMask;
        public float raycastLength = 100;

        public bool keepOpenState = false;
        public float keepOpenStateMaxDist = 50;
        private PlayersPositionManager keepOpenStateSTSPosMgr;
        public GameObject keepOpenStateSTSPosGO;
        private List<GameObject> listTempPlayerGO = new List<GameObject>();
        private Dictionary<GameObject, GameObject> listCamCubesAttached = new Dictionary<GameObject, GameObject>();


        // Start is called before the first frame update
        void Start()
        {
            if (keepOpenState && keepOpenStateSTSPosGO != null)
            {
                keepOpenStateSTSPosMgr = keepOpenStateSTSPosGO.GetComponent<PlayersPositionManager>();
                if (keepOpenStateSTSPosMgr != null)
                {
                    for (int i = 0; i < 99; i++)
                    {
                        if (cameraCube != null)
                        {
                            GameObject tempGO = Instantiate(cameraCube, keepOpenStateSTSPosGO.transform);
                            //keepOpenStateSTSPosMgr.playableCharacters.Add(tempGO);
                            //keepOpenStateSTSPosMgr.init();
                            keepOpenStateSTSPosMgr.AddPlayerAtRuntime(tempGO);
                            listTempPlayerGO.Add(tempGO);
                        }
                    }
                }
            }
        }

        // Update is called once per frame
        void Update()
        {

            Ray m_Ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            m_Ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward * raycastLength);
            RaycastHit m_hit;
            if (Physics.Raycast(m_Ray, out m_hit, raycastLength, MovementMask))
            {


                if (keepOpenState && m_hit.transform.gameObject.GetComponent<TriggerByParent>() != null && m_hit.transform.gameObject.GetComponent<TriggerByParent>().isDedicatedEnterExitTrigger == false)
                {

                    if (Vector3.Distance(this.transform.position, m_hit.transform.gameObject.GetComponent<Collider>().bounds.center) <= keepOpenStateMaxDist)
                    {
                        if (keepOpenState)
                        {
                            if (listCamCubesAttached.ContainsKey(m_hit.transform.gameObject) == false)
                            {
                                GameObject newCamCube = listTempPlayerGO[0];
                                newCamCube.transform.position = m_hit.transform.gameObject.GetComponent<Collider>().bounds.center;
                                newCamCube.transform.parent = m_hit.transform;
                                listTempPlayerGO.Remove(listTempPlayerGO[0]);
                                listCamCubesAttached.Add(m_hit.transform.gameObject, newCamCube);
                            }
                        }

                    }

                }
                else
                {
                    cameraCube.transform.position = m_hit.point;
                }

            }



            if (keepOpenState)
            {

                GameObject removeGO = null;
                GameObject removeCamCubeGO = null;

                foreach (GameObject item in listCamCubesAttached.Keys)
                {


                    if (listCamCubesAttached.ContainsKey(item))
                    {
                        if (Vector3.Distance(this.transform.position, listCamCubesAttached[item].transform.position) > keepOpenStateMaxDist)
                        {
                            removeGO = item;
                            removeCamCubeGO = listCamCubesAttached[item];
                        }
                    }

                }

                if (removeGO != null && removeCamCubeGO != null && keepOpenStateSTSPosGO != null)
                {

                    if (removeGO.GetComponent<TriggerByParent>() != null)
                    {
                        ///removeGO.GetComponent<TriggerByParent>().ManualExitTrigger(removeCamCubeGO.transform);
                    }
                    removeCamCubeGO.transform.position = Vector3.zero;
                    removeCamCubeGO.transform.parent = keepOpenStateSTSPosGO.transform;
                    listTempPlayerGO.Add(removeCamCubeGO);
                    listCamCubesAttached.Remove(removeGO);

                }
            }


        }
    }
}