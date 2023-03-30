using UnityEngine;

namespace Blobcreate.ProjectileToolkit.Demo
{
	public class JumpTester : MonoBehaviour
	{
		public Vector3 target;
		public float heightFromEnd = 2f;
		public float halfHeight = 0.5f;

		Rigidbody rigid;
		bool targetHasChanged;

		public bool TargetHasChanged { set => targetHasChanged = value; }


		public bool IsGrounded
		{
			get
			{
				return Physics.Raycast(transform.position, Vector3.down,
					halfHeight + 0.02f);
			}
		}

		void Jump(Vector3 targetPos)
		{
			var hFromX = heightFromEnd;

			rigid.velocity = default;
			rigid.AddForce(
				Projectile.VelocityByHeight(transform.position, targetPos, hFromX),
				ForceMode.VelocityChange);
		}


		void Start()
		{
			rigid = GetComponent<Rigidbody>();
			Jump(target);
		}

		void Update()
		{
			if (targetHasChanged)
			{
				if (IsGrounded)
				{
					Jump(target);
					targetHasChanged = false;
				}
			}
		}
	}
}