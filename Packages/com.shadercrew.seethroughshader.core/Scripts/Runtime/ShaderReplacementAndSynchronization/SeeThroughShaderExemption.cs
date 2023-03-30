using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShaderCrew.SeeThroughShader
{
    [AddComponentMenu(Strings.COMPONENTMENU_SHADER_EXEMPTION)]
    public class SeeThroughShaderExemption : MonoBehaviour
    {
        Transform root;
        void Start()
        {
            root = this.gameObject.transform;
            if (root != null)
            {
                Renderer[] renderers = root.GetComponentsInChildren<Renderer>();
                for (int i = 0; i < renderers.Length; i++)
                {
                    renderers[i].material.SetFloat("_IsExempt", System.Convert.ToSingle(this.enabled));
                }
            }
        }

        void Update()
        {

        }

        //private void OnValidate()
        //{
        //    root = this.gameObject.transform.root;
        //    if (root != null)
        //    {
        //        Renderer[] renderers = root.GetComponentsInChildren<Renderer>();
        //        for (int i = 0; i < renderers.Length; i++)
        //        {
        //            // System.Convert.ToSingle(this.enabled)
        //            renderers[i].material.SetFloat("_IsExempt", System.Convert.ToSingle(this.enabled));
        //        }
        //    }
        //}

        void OnEnable()
        {
            if (root != null)
            {
                Renderer[] renderers = root.GetComponentsInChildren<Renderer>();
                for (int i = 0; i < renderers.Length; i++)
                {
                    renderers[i].material.SetFloat("_IsExempt", System.Convert.ToSingle(this.isActiveAndEnabled));
                }
            }

        }
        void OnDisable()
        {
            if (root != null)
            {
                Renderer[] renderers = root.GetComponentsInChildren<Renderer>();
                for (int i = 0; i < renderers.Length; i++)
                {
                    renderers[i].material.SetFloat("_IsExempt", System.Convert.ToSingle(this.isActiveAndEnabled));
                }
            }

        }
    }
}
