
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShaderCrew.SeeThroughShader
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(MeshCollider))]
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class MeshCombine : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            ///Use to clear deactivate all meshes if needed
            //for (int a = 0; a < transform.childCount; a++)
            // DestroyImmediate(transform.GetChild(a).gameObject.GetComponent<Collider>());
            //transform.GetChild(a).gameObject.SetActive(false);

            Quaternion OldRot = transform.rotation;
            Vector3 OldPos = transform.position;

            transform.rotation = Quaternion.identity;
            transform.position = Vector3.zero;



            MeshFilter[] filters = GetComponentsInChildren<MeshFilter>();

            Debug.Log(name + "is combining" + filters.Length + "meshes!");

            Mesh finalMesh = new Mesh();
            GetComponent<MeshCollider>().sharedMesh = null;
            GetComponent<MeshCollider>().sharedMesh = finalMesh;



            CombineInstance[] combiners = new CombineInstance[filters.Length];


            for (int a = 0; a < filters.Length; a++)
            {
                if (filters[a].transform == transform)
                    continue;


                combiners[a].subMeshIndex = 0;
                combiners[a].mesh = filters[a].sharedMesh;
                combiners[a].transform = filters[a].transform.localToWorldMatrix;
            }

            finalMesh.CombineMeshes(combiners);


            GetComponent<MeshFilter>().sharedMesh = finalMesh;



            transform.rotation = OldRot;
            transform.position = OldPos;
            transform.GetComponent<MeshRenderer>().enabled = false;


        }

    }
}