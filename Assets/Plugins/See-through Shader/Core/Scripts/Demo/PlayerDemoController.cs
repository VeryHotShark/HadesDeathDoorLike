using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace ShaderCrew.SeeThroughShader
{
    public class PlayerDemoController : MonoBehaviour
    {
        // Start is called before the first frame update

        public Camera PlayerCamera;
        public LayerMask MovementMask;
        public NavMeshAgent myAIAgent;
        public GameObject selectionSphere;
        public GameObject currentPlayer;
        public GameObject rotateCube;
        public GameObject currentTargetGO;
        private Dictionary<GameObject, GameObject> playersInventory = new Dictionary<GameObject, GameObject>();

        

        Animator myAnimator;
        Rigidbody myRigidBody;

        void Start()
        {
            if (currentPlayer != null)
            {
                selectionSphere.transform.position = currentPlayer.transform.position;
                selectionSphere.transform.position += new Vector3(0, 2, 0);
                selectionSphere.transform.parent = currentPlayer.transform;
            }
        }

        // Update is called once per frame
        void Update()
        {

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {

                Ray m_Ray = PlayerCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit m_hit;
                if (Physics.Raycast(m_Ray, out m_hit, 5000.0f, MovementMask))
                {
                    if (m_hit.transform.gameObject.tag == "Player")
                    {
                        if (m_hit.transform.gameObject.GetComponent<NavMeshAgent>() != null)
                        {
                            if (currentPlayer != null && currentPlayer != m_hit.transform.gameObject)
                            {

                            }

                            myAIAgent = m_hit.transform.gameObject.GetComponent<NavMeshAgent>();
                            currentPlayer = m_hit.transform.gameObject;
                            selectionSphere.transform.position = currentPlayer.transform.position;
                            selectionSphere.transform.position += new Vector3(0, 2, 0);
                            selectionSphere.transform.parent = currentPlayer.transform;
                            rotateCube.transform.position = currentPlayer.transform.position;
                            rotateCube.transform.parent = currentPlayer.transform;


                        }
                    }
                    else if (m_hit.transform.gameObject.layer == LayerMask.NameToLayer("Item"))
                    {
                        currentTargetGO = m_hit.transform.gameObject;
                        foreach (Transform item in currentTargetGO.transform)
                        {
                            if (item.gameObject.name == "ItemTrigger")
                            {
                                myAIAgent.destination = item.transform.position;
                            }
                        }

                    }
                    else
                    {


                        if (myAIAgent != null)
                        {

                            myAIAgent.destination = m_hit.point;
                        }
                        else
                        {
                            Debug.Log("No Player selected ,please select a player first");
                        }

                    }


                }
            }

            if (currentPlayer != null)
            {
                if (currentPlayer.GetComponent<Animator>() != null)
                {
                    if (currentPlayer.GetComponent<NavMeshAgent>() != null)
                    {
                        currentPlayer.GetComponent<Animator>().SetFloat("Forward", currentPlayer.GetComponent<NavMeshAgent>().velocity.magnitude);

                    }
                }
            }

            if (currentTargetGO != null && myAIAgent.remainingDistance == 0)
            {
                foreach (Transform item in currentTargetGO.transform)
                {
                    if (item.gameObject.GetComponent<Collider>().bounds.Contains(currentPlayer.transform.position))
                    {
                        currentPlayer.transform.rotation = item.transform.rotation;
                        currentPlayer.GetComponent<Animator>().SetTrigger("Pickup");

                        playersInventory.Add(currentPlayer, currentTargetGO);
                        currentTargetGO.transform.position = Vector3.zero;
                        currentTargetGO = null;
                    }
                }
            }

        }

        

    }
}