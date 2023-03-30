using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShaderCrew.SeeThroughShader
{
    public enum ZoneType
    {
        Box,
        Sphere,
        Cylinder,
        //etc.
    }
    public abstract class ZoneData : ScriptableObject
    {

        public Transform transform;
        protected TransformContainer transformContainer;
        public int id { get; set; }
        public int type { get; set; }

        public int zoneInstanceID { get; set; }

        public TransitionData transitionData { get; set; }

        public ZoneData()
        {
            //id = IdGenerator.Instance.Id;
            //if (id == 0)
            //{
            //    id = IdGenerator.Instance.Id;
            //}
            //this.transformContainer = new TransformContainer(this.transform);
        }

        public void Init(Transform transform)
        {
            this.transform = transform;
            this.transitionData = new TransitionData(0,0,0,0);
            //id = IdGenerator.Instance.Id;
            //if (id == 0)
            //{
            //    id = IdGenerator.Instance.Id;
            //}
            //this.transformContainer = new TransformContainer(this.transform);
        }


        public abstract float[] ToShaderFloatArraySegment();

        public bool UpdateNecessary()
        {
            if (this.transformContainer == null)
            {
                this.transformContainer = new TransformContainer(this.transform);
                return true;
            }
            if (this.transformContainer != null && this.transform != null)
            {
                return transformContainer.HasChanged(this.transform);
            }
            else
            {
                return false;
            }
        }




    }
}