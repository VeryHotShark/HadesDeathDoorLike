using System;
using Blobcreate.Universal;
using UnityEngine;

namespace Blobcreate.ProjectileToolkit.Demo
{
	public class SimpleExplosive : ProjectileBehaviour
	{
		public float radius = 4f;
		public float centerForce = 10f;
		public float forceUplit;
		public LayerMask scanMask;
		public int damage = 100;
		public bool isPlayerWeapon;
		public LayerMask selfMask;
		public float selfDamageRatio = 0.25f;

		Collider[] result = new Collider[16];

		protected override void OnLaunch()
		{
		}

		// Apply damage and force.
		protected override void Explosion(Collision collision)
		{
			var c = Physics.OverlapSphereNonAlloc(transform.position, radius, result, scanMask);
			var mul = isPlayerWeapon ? selfDamageRatio : 1f;

			for (int i = 0; i < c; i++)
			{
				if (result[i].TryGetComponent<IHealth>(out var h))
				{
					if ((1 << result[i].gameObject.layer & selfMask) != 0)
						// is player
						h.TakeDamage((int)(mul * damage));
					else
						// is not player
						h.TakeDamage((damage));
				}
				
				if (result[i].TryGetComponent<Rigidbody>(out var rb))
					rb.AddExplosionForce(centerForce, transform.position, radius, forceUplit, ForceMode.Impulse);
				else if (result[i].TryGetComponent<CharacterMovement>(out var cm))
					cm.AddExplosionForce(centerForce, transform.position, radius, forceUplit);

				result[i] = null;
			}

			base.Explosion(collision);
		}
	}

}