using UnityEngine;
using UnityEngine.AI;

namespace Blobcreate.ProjectileToolkit.Demo
{
	public class NMJump : MonoBehaviour
	{
		public bool useUnityJump;
		public float heightFromEnd = 1.5f;
		public float protectedJumpTime = 1f;
		public LayerMask groundMask;
		public Transform targetObj;
		public float airAngularSpeed = 360f;

		NavMeshAgent agent;
		Rigidbody rigid;
		bool isJumping;
		float jumpTimer;
		float destinationUpdateTimer;

		void Start()
		{
			rigid = GetComponent<Rigidbody>();
			agent = GetComponent<NavMeshAgent>();
		}

		void Update()
		{
			if (Input.GetKeyDown(KeyCode.U))
			{
				useUnityJump = !useUnityJump;
			}

			destinationUpdateTimer += Time.deltaTime;

			if (destinationUpdateTimer > 0.2f)
			{
				destinationUpdateTimer -= 0.2f;
				if (agent.enabled)
				{
					agent.autoTraverseOffMeshLink = useUnityJump;
					agent.destination = targetObj.position;
				}
			}

			if (!isJumping && !useUnityJump && agent.isOnOffMeshLink)
			{
				Jump(agent.currentOffMeshLinkData.endPos);
				agent.enabled = false;
			}

			if (isJumping)
			{
				jumpTimer += Time.deltaTime;

				if (jumpTimer > protectedJumpTime && IsGrounded)
				{
					jumpTimer = 0f;
					isJumping = false;
					rigid.isKinematic = true;
					agent.enabled = true;
				}
			}
		}

		void FixedUpdate()
		{
			if (!isJumping)
				return;

			// Control rotation in air.
			var lookDir = new Vector3(rigid.velocity.x, 0f, rigid.velocity.z);
			var lookQua = Quaternion.LookRotation(lookDir);
			var angle = Quaternion.Angle(transform.rotation, lookQua);
			transform.rotation = Quaternion.Slerp(transform.rotation, lookQua,
				1f / angle * airAngularSpeed * Time.deltaTime);
		}

		public bool IsGrounded
		{
			get
			{
				return Physics.Raycast(transform.position, Vector3.down,
					agent.height * 0.5f + 0.06f, groundMask);
			}
		}

		void Jump(Vector3 targetPos)
		{
			var hFromX = transform.position.y > targetPos.y ?
				transform.position.y - targetPos.y + heightFromEnd :
				heightFromEnd;
			//var hFromX = heightFromEnd;

			rigid.isKinematic = false;
			rigid.velocity = default;
			rigid.AddForce(
				Projectile.VelocityByHeight(transform.position, targetPos + agent.height * 0.5f * Vector3.up, hFromX),
				ForceMode.VelocityChange);

			isJumping = true;
		}
	}
}