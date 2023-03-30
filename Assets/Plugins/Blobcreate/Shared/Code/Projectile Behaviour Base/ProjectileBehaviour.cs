using System;
using UnityEngine;

namespace Blobcreate.Universal
{
	public abstract class ProjectileBehaviour : MonoBehaviour
	{
		[SerializeField] protected Transform explosionFX;

		protected bool exploded = false;

		public Action<Collision> OnExplode;

		public Transform Target { get; set; }
		public Vector3 TargetPoint { get; set; }
		public virtual Transform ExplosionFX { get => explosionFX; set => explosionFX = value; }


		protected abstract void OnLaunch();

		protected virtual void Fly()
		{
			if (exploded)
				return;

			if (Target != null)
				TargetPoint = Target.position;

			// Do the movement here...
		}

		protected virtual void Explosion(Collision collision)
		{
			OnExplode?.Invoke(collision);

			if (explosionFX != null)
				Instantiate(explosionFX, transform.position, Quaternion.identity);
		}

		public void Launch(Transform target)
		{
			exploded = false;
			Target = target;
			TargetPoint = target.position;
			OnLaunch();
		}

		public void Launch(Vector3 targetPoint)
		{
			exploded = false;
			Target = null;
			TargetPoint = targetPoint;
			OnLaunch();
		}


		void Update()
		{
			Fly();
		}

		protected virtual void OnCollisionEnter(Collision collision)
		{
			if (exploded)
				return;

			exploded = true;
			Explosion(collision);
			// Unsubscribe all events and destroy self.
			OnExplode = null;
			Destroy(gameObject);
		}
	}
}