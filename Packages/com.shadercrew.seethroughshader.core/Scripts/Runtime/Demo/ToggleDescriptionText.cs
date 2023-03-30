using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleDescriptionText : MonoBehaviour
{
    public GameObject description;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (description.activeSelf)
            {
                description.SetActive(false);
            }
            else
            {
                description.SetActive(true);
            }
        }
    }
}
