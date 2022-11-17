using UnityEngine;

namespace ShaderCrew.SeeThroughShader
{
    public class TransformContainer
    {

        public Vector3 position;
        public Quaternion rotation;
        public Vector3 localScale;

        public TransformContainer(Vector3 position, Quaternion rotation, Vector3 localScale)
        {
            this.position = position;
            this.rotation = rotation;
            this.localScale = localScale;
        }

        public TransformContainer(Transform transform)
        {
            if (transform != null)
            {
                CopyFrom(transform);
            }
            else
            {
                this.position = Vector3.zero;
                this.rotation = Quaternion.identity;
                this.localScale = Vector3.one;
            }

        }

        public bool HasChanged(Transform transform)
        {
            if (Vector3.Distance(transform.position, this.position) <= 0 &&
                Quaternion.Angle(transform.rotation, this.rotation) <= 0 &&
                Vector3.Distance(transform.localScale, this.localScale) <= 0)
            {
                return false;
            }
            else
            {
                CopyFrom(transform);
                return true;
            }
        }

        public void CopyFrom(Transform transform)
        {
            position = transform.position;
            rotation = transform.rotation;
            localScale = transform.localScale;
        }

        public void CopyTo(Transform transform)
        {
            transform.position = position;
            transform.rotation = rotation;
            transform.localScale = localScale;
        }

    }
}