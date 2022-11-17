using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddPlayerAtStartHelper : MonoBehaviour
{
    public List<GameObject> playerPrefabs;
    public GameObject playerParentGO;
    
    // Start is called before the first frame update
    void Start()
    {
        if (playerPrefabs.Count > 0 && playerParentGO != null)
        {
            foreach (GameObject item in playerPrefabs)
            {
                Instantiate(item, playerParentGO.transform.position,playerParentGO.transform.rotation,playerParentGO.transform);
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
