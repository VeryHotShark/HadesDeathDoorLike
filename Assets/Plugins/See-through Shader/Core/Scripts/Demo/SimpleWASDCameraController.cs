using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleWASDCameraController : MonoBehaviour
{

    public float speed = 1.5F;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            Vector3 tmp = transform.position + transform.forward.normalized * speed;
            tmp.y = this.transform.position.y;
            this.transform.position = tmp;
        }
        if (Input.GetKey(KeyCode.A))
        {
            Vector3 tmp = transform.position + -transform.right.normalized * speed;
            tmp.y = this.transform.position.y;
            this.transform.position = tmp;
        }
        if (Input.GetKey(KeyCode.S))
        {
            Vector3 tmp = transform.position + -transform.forward.normalized * speed;
            tmp.y = this.transform.position.y;
            this.transform.position = tmp;
        }
        if (Input.GetKey(KeyCode.D))
        {
            Vector3 tmp = transform.position + transform.right.normalized * speed;
            tmp.y = this.transform.position.y;
            this.transform.position = tmp;
        }
        if (Input.GetKey(KeyCode.X))
        {
            Vector3 tmp = transform.position + transform.up.normalized * speed;
            tmp.x = this.transform.position.x;
            tmp.z = this.transform.position.z;
            this.transform.position = tmp;
        }
        if (Input.GetKey(KeyCode.Y))
        {
            Vector3 tmp = transform.position + -transform.up.normalized * speed;
            tmp.x = this.transform.position.x;
            tmp.z = this.transform.position.z;
            this.transform.position = tmp;
        }


    }
}
