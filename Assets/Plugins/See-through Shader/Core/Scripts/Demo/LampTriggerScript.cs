using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShaderCrew.SeeThroughShader
{
    public class LampTriggerScript : MonoBehaviour
    {
        public Light lightBulb;
        // Start is called before the first frame update
        void Start()
        {
            lightBulb = GetComponent<Light>();
            lightBulb.enabled = false;
        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnTriggerEnter(Collider other)
        {
            if (other.name == "Player")
            {
                lightBulb.enabled = true;
            }
            else if (other.name == "Player")
            {
                lightBulb.enabled = false;
            }
        }
    }
}