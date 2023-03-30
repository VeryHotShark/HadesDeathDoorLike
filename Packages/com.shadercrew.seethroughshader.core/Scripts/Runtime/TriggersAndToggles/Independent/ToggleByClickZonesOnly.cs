using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ShaderCrew.SeeThroughShader
{
    [AddComponentMenu(Strings.COMPONENTMENU_TOGGLE_BY_CLICK_ZONES_ONLY)]
    public class ToggleByClickZonesOnly : MonoBehaviour
    {
        //bool activated = false;


        RaycastHit[] hits = new RaycastHit[100];



        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                //int hitCount = Physics.RaycastNonAlloc(ray, hits);

                //for (int i = 0; i < hitCount; i++)
                //{
                //    if (hits[i].collider != null && hits[i].transform != null)
                //    {
                //        OnGameObjectClicked(hits[i].transform.gameObject);
                //    }
                //}

                //RaycastHit[] hit = Physics.RaycastAll(ray);
                //for (int i = 0; i < hit.Length; i++)
                //{
                //    if (hit[i].collider != null && hit[i].transform != null)
                //    {
                //        OnGameObjectClicked(hit[i].transform.gameObject);
                //    }
                //}

                RaycastHit raycastHit;
                if (Physics.Raycast(ray, out raycastHit, 100f))
                {
                    if (raycastHit.transform != null)
                    {
                        OnGameObjectClicked(raycastHit.transform.gameObject);
                    }
                }
            }
        }

        public void OnGameObjectClicked(GameObject gameObject)
        {
            if (gameObject.GetComponent<SeeThroughShaderZone>())
            {
                SeeThroughShaderZone seeThroughShaderZone = gameObject.GetComponent<SeeThroughShaderZone>();
                if (seeThroughShaderZone.enabled)
                {
                    seeThroughShaderZone.toggleZoneActivation();
                }
                
            }
        }




    }


}