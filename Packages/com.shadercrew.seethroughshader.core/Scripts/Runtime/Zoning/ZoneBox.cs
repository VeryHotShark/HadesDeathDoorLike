using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShaderCrew.SeeThroughShader
{
    public class ZoneBox : ZoneData
    {
        public ZoneBox(Transform transform)
        {
            this.transform = transform;
        }


        public static ZoneBox CreateInstance(Transform transform)
        {
            ZoneBox data = ScriptableObject.CreateInstance<ZoneBox>();
            data.Init(transform);
            return data;
        }

        //public override float[] ToShaderFloatArraySegment()
        //{
        //    if (id == 0)
        //    {
        //        id = IdGenerator.Instance.Id;
        //    }
        //    List<float> list = new List<float>();
        //    list.Add(id);
        //    list.Add(type); //instead array length?
        //    list.AddRange(computeShaderInformationCube());
        //    return list.ToArray();
        //}

        public override float[] ToShaderFloatArraySegment()
        {
            if (id == 0)
            {
                id = IdGenerator.Instance.Id;
            }
            List<float> list = new List<float>();
            list.Add(id);
            list.Add(type); //instead array length?
            list.AddRange(computeShaderInformationCube());
            if(transitionData == null)
            {
                transitionData = new TransitionData(0, 0, 0, 0);
            }
            list.AddRange(transitionData.ToShaderFloatArraySegmentZone());
            return list.ToArray();
        }

        private float[] computeShaderInformationCube()
        {
            Vector3 center = this.transform.position;
            Vector3 leftCornerBackward = new Vector3(-0.5f, -0.5f, 0.5f);
            Vector3 leftCornerForward = new Vector3(-0.5f, -0.5f, -0.5f);
            Vector3 rightCornerForward = new Vector3(0.5f, -0.5f, -0.5f);
            Vector3 topCornerForward = new Vector3(-0.5f, 0.5f, -0.5f);
            Vector3 xDirection = Vector3.Normalize(transform.TransformPoint(rightCornerForward) - transform.TransformPoint(leftCornerForward));
            Vector3 yDirection = Vector3.Normalize(transform.TransformPoint(topCornerForward) - transform.TransformPoint(leftCornerForward));
            Vector3 zDirection = Vector3.Normalize(transform.TransformPoint(leftCornerBackward) - transform.TransformPoint(leftCornerForward));
            Vector3 dimensionsBy2 = this.transform.lossyScale / 2;

            List<Vector3> data = new List<Vector3> { center, xDirection, yDirection, zDirection, dimensionsBy2 };
            List<float> shaderData = new List<float>();

            foreach (Vector3 vec3 in data)
            {
                shaderData.Add(vec3.x);
                shaderData.Add(vec3.y);
                shaderData.Add(vec3.z);
            }
            return shaderData.ToArray();

        }
    }
}