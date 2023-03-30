using UnityEngine;
using UnityEngine.AI;

namespace Blobcreate.ProjectileToolkit.Demo
{
	public class WASDude : MonoBehaviour
	{
		public float moveSpeed = 8f;
		public float jumpForce;
		public float protectedJumpTime = 0.5f;
		public LayerMask groundMask;

		Vector3 forwardVector;
		Vector3 rightVector;
		Vector3 moveVec;
		NavMeshAgent agent;
		Rigidbody rigid;
		bool isJumping;
		float jumpTimer;
		Vector3 airMoveVec;
		Vector3 airMove;

		void Start()
		{
			var camf = Camera.main.transform.forward;
			forwardVector = Vector3.Normalize(new Vector3(camf.x, 0f, camf.z));
			rightVector = Vector3.Cross(Vector3.up, forwardVector);

			agent = GetComponent<NavMeshAgent>();
			rigid = GetComponent<Rigidbody>();
		}

		void Update()
		{
			moveVec = default;

			if (Input.GetKey(KeyCode.W))
				moveVec += forwardVector;
			else if (Input.GetKey(KeyCode.S))
				moveVec += -forwardVector;

			if (Input.GetKey(KeyCode.A))
				moveVec += -rightVector;
			else if (Input.GetKey(KeyCode.D))
				moveVec += rightVector;

			moveVec.Normalize();

			if (!isJumping && Input.GetKeyDown(KeyCode.Space))
			{
				agent.enabled = false;
				rigid.isKinematic = false;
				rigid.velocity = 0.6f * moveSpeed * moveVec;
				rigid.AddForce(jumpForce * Vector3.up, ForceMode.VelocityChange);
				isJumping = true;
			}

			if (isJumping)
			{
				jumpTimer += Time.deltaTime;
				airMoveVec = moveVec;
				moveVec = default;

				if (jumpTimer > protectedJumpTime && IsGrounded)
				{
					jumpTimer = 0f;
					//agent.Warp(transform.position);
					agent.enabled = true;
					isJumping = false;
					rigid.isKinematic = true;
					airMove = Vector3.zero;
				}
			}

			if (agent.enabled)
				agent.Move(moveSpeed * Time.deltaTime * moveVec);
		}

		void FixedUpdate()
		{
			if (isJumping)
			{
				rigid.velocity -= airMove;
				airMove = 0.4f * moveSpeed * airMoveVec;
				rigid.velocity += airMove;
			}
		}

		public bool IsGrounded
		{
			get
			{
				return Physics.Raycast(transform.position, Vector3.down,
					agent.height * 0.5f + 0.06f, groundMask);
			}
		}
	}
}