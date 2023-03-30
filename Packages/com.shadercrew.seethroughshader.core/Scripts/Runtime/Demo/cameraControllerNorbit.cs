
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShaderCrew.SeeThroughShader
{
    public class cameraControllerNorbit : MonoBehaviour
    {
        public GameObject player;
        private Vector3 wayPoint;
        public GameObject rotateCube;
        public float distance = 10;
        public float distanceSpeed;
        public float orbitSpeed;
        public float followSpeed;
        private Vector3 posTemp;
        public float cameraHeight = 5;
        public float sensitivityX = 5;
        public float sensitivityY = 5;
        public bool followMode = false;
        public float minCamHeight;
        public float maxCamHeight;
        public float minPlayerDistance;
        public float maxPlayerDistance;
        public float maxFreeCamDistance;
        public GameObject myCanvas;

        // Start is called before the first frame update
        void Start()
        {

            transform.LookAt(player.transform);


        }

        // Update is called once per frame
        void Update()
        {
            // Decrease radius to player
            if (Input.GetKey(KeyCode.W))
            {

                if (followMode)
                {
                    if (distance >= minPlayerDistance)
                    {

                        distance -= 1;
                    }
                }
                else
                {
                    if (Vector3.Distance(player.transform.position, transform.position) < maxFreeCamDistance)

                    {

                        Vector3 tempFwd = transform.forward.normalized * distanceSpeed;
                        tempFwd.y = 0;
                        transform.position += tempFwd;
                    }
                    else
                    {
                        Vector3 tempVect = transform.position + transform.forward.normalized * -5.5F;
                        tempVect.y = transform.position.y;
                        Vector3 tempPos = player.transform.forward * -15F;
                        tempPos.y = 5F;
                        transform.position = player.transform.position + tempPos;
                        transform.LookAt(player.transform.position);
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.I))
            {
                if (myCanvas.activeSelf)
                {
                    myCanvas.SetActive(false);
                }
                else
                {
                    myCanvas.SetActive(true);
                }
            }



            //Increase radius to player
            if (Input.GetKey(KeyCode.S))
            {

                if (followMode)
                {
                    if (distance <= maxPlayerDistance)
                    {

                        distance += 1;
                    }
                }
                else
                {
                    if (Vector3.Distance(player.transform.position, transform.position) < maxFreeCamDistance)
                    {


                        Vector3 tempFwd = transform.forward.normalized * distanceSpeed;
                        tempFwd.y = 0;
                        transform.position -= tempFwd;
                    }
                    else
                    {
                        Vector3 tempVect = transform.position + transform.forward.normalized * 5.5F;
                        tempVect.y = transform.position.y;
                        Vector3 tempPos = player.transform.forward * -15F;
                        tempPos.y = 5F;
                        transform.position = player.transform.position + tempPos;
                        transform.LookAt(player.transform.position);

                    }
                }

            }


            // Orbit right
            if (Input.GetKey(KeyCode.D))
            {


                if (followMode)
                {
                    followMode = false;
                }
                else
                {
                    if (Vector3.Distance(player.transform.position, transform.position) <= maxFreeCamDistance)
                    {


                        Vector3 tempFwd = transform.right.normalized * distanceSpeed;
                        tempFwd.y = 0;
                        transform.position += tempFwd;
                    }
                    else
                    {
                        Vector3 tempPos = player.transform.forward * -15F;
                        tempPos.y = 5F;
                        transform.position = player.transform.position + tempPos;
                        transform.LookAt(player.transform.position);
                    }
                }

            }


            // Orbit left

            if (Input.GetKey(KeyCode.Q))
            {

                transform.RotateAround(rotateCube.transform.position, Vector3.up, orbitSpeed * Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.E))
            {

                transform.RotateAround(rotateCube.transform.position, Vector3.up, -orbitSpeed * Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.A))
            {

                if (followMode)
                {

                    followMode = false;
                }
                else
                {
                    if (Vector3.Distance(player.transform.position, transform.position) <= maxFreeCamDistance)
                    {


                        Vector3 tempFwd = transform.right.normalized * distanceSpeed;
                        tempFwd.y = 0;
                        transform.position -= tempFwd;
                    }
                    else
                    {
                        Vector3 tempPos = player.transform.forward * -15F;
                        tempPos.y = 5F;
                        transform.position = player.transform.position + tempPos;
                        transform.LookAt(player.transform.position);
                    }
                }
            }


            if (Input.GetKey(KeyCode.Y))
            {
                cameraHeight -= 0.5F;
                Vector3 tempFwd = Vector3.up.normalized * distanceSpeed;
                if (transform.position.y >= (player.transform.position.y + minCamHeight))
                {

                    transform.position -= tempFwd;
                }

            }

            if (Input.GetKey(KeyCode.X))
            {
                cameraHeight += 0.5F;
                Vector3 tempFwd = Vector3.up.normalized * distanceSpeed;
                if (transform.position.y <= (player.transform.position.y + maxCamHeight))
                {

                    transform.position += tempFwd;
                }
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                if (followMode == false)
                {
                    Vector3 tempPos = player.transform.forward * -15F;
                    tempPos.y += 5F;
                    transform.position += tempPos;
                    transform.LookAt(player.transform.position);
                    distance = 10;
                    followMode = true;

                }
                else
                {
                    followMode = false;
                }
            }



            RaycastHit hit;
            if (Physics.Raycast(transform.position, -Vector3.up, out hit, 1000))
            {
                var distanceToGround = hit.distance;

                if (hit.transform.GetComponent<Terrain>() != null || hit.transform.gameObject.layer == 22)
                {
                    Vector3 tempPos;

                    if (true)
                    {

                        tempPos = transform.position;


                    }
                }

            }
            else if (Physics.Raycast(transform.position + new Vector3(0, 50, 0), -Vector3.up, out hit, 1000))
            {
                var distanceToGround = hit.distance;

                if (hit.transform.GetComponent<Terrain>() != null)
                {
                    Vector3 tempPos;
                    if (true)
                    {

                        tempPos = transform.position;

                    }
                }

            }

        }
        private void LateUpdate()
        {
            if (followMode)
            {
                Vector3 playerToCam = player.transform.position - transform.position;
                playerToCam.y = 0;
                var normalized = Vector3.Normalize(playerToCam);
                playerToCam = player.transform.position - (distance * normalized);
                playerToCam = new Vector3(playerToCam.x, cameraHeight, playerToCam.z);

                transform.position = Vector3.Lerp(transform.position, playerToCam, followSpeed * Time.deltaTime);

            }
        }
    }

}