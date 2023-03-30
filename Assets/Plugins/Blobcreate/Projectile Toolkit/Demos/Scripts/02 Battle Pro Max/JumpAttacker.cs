using Blobcreate.Universal;
using UnityEngine;
using UnityEngine.AI;

namespace Blobcreate.ProjectileToolkit.Demo
{
	public class JumpAttacker : MonoBehaviour, IHealth
	{
		public Transform attackTarget;
		public Transform healthPack;
		public float attackRadius = 10f;
		public float heightFromEnd = 1.5f;
		public float protectedJumpTime = 1f;
		public float jumpPrepareTime = 0.2f;
		public LayerMask groundMask;
		public LayerMask obstacleMask;
		public float jumpAttackA = -0.15f;
		public float jumpGain = 1f;
		public float attackForce = 10f;
		public int damage = 25;
		public int maxHealth = 800;
		public int lowHealth = 200;
		public Transform deadFX;

		NavMeshAgent agent;
		Rigidbody rigid;
		bool isJumping;
		bool isAttacking;
		float jumpTimer;
		float destinationUpdateTimer;
		int attackCount;
		Vector3 attackStartPoint;
		int currentHealth;
		Transform currentDestination;

		public bool IsGrounded
		{
			get
			{
				return Physics.Raycast(transform.position, Vector3.down,
					agent.height * 0.5f + 0.06f, groundMask);
			}
		}

		void Jump(Vector3 targetPos, float a = 0f)
		{
			rigid.isKinematic = false;
			isJumping = true;
			agent.enabled = false;
			rigid.velocity = default;
			rigid.angularVelocity = default;
			rigid.collisionDetectionMode = CollisionDetectionMode.Continuous;

			if (a >= 0f)
			{
				// OffMeshLink jump.
				var hFromX = transform.position.y > targetPos.y ?
					transform.position.y - targetPos.y + heightFromEnd :
					heightFromEnd;
				rigid.AddForce(
					Projectile.VelocityByHeight(transform.position, targetPos + agent.height * 0.5f * Vector3.up, hFromX),
					ForceMode.VelocityChange);
			}
			else
			{
				// Attack jump.
				var f = Projectile.VelocityByA(transform.position, targetPos, a);
				f.y = Mathf.Clamp(f.y, 0f, 20f);
				f = new Vector3(jumpGain * f.x, f.y, jumpGain * f.z);
				rigid.AddForce(f, ForceMode.VelocityChange);
			}
		}

		void Start()
		{
			rigid = GetComponent<Rigidbody>();
			agent = GetComponent<NavMeshAgent>();
			currentHealth = maxHealth;
			currentDestination = attackTarget;
		}

		void Update()
		{
			if (attackTarget == null)
				return;

			destinationUpdateTimer += Time.deltaTime;

			// Update the destination every 0.2 second.
			if (destinationUpdateTimer > 0.2f)
			{
				destinationUpdateTimer -= 0.2f;
				if (agent.enabled)
				{
					agent.destination = currentDestination.position;
				}
			}

			if (!isJumping && agent.isOnOffMeshLink)
			{
				// OffMeshLink met, jump.
				Jump(agent.currentOffMeshLinkData.endPos);
			}
			else if (currentHealth > lowHealth && !isJumping)
			{
				if (Vector3.Distance(transform.position, attackTarget.position) < attackRadius
					&& CanSee(attackTarget))
				{
					// Health is high and attackTarget met, attack.
					agent.enabled = false;
				}

				if (!agent.enabled)
				{
					jumpTimer += Time.deltaTime;

					if (jumpTimer > jumpPrepareTime)
					{
						jumpTimer = 0f;
						isAttacking = true;
						Jump(attackTarget.position, jumpAttackA);
						attackStartPoint = transform.position;
					}
				}
			}

			// Try landing.
			if (isJumping && !isAttacking)
			{
				jumpTimer += Time.deltaTime;

				if (jumpTimer > protectedJumpTime && IsGrounded)
				{
					jumpTimer = 0f;
					// Supress the collision type warning.
					rigid.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
					rigid.isKinematic = true;
					isJumping = false;
					agent.enabled = true;
				}
			}
		}

		void OnCollisionEnter(Collision collision)
		{
			if (attackTarget == null)
				return;

			if (isAttacking)
			{
				attackCount += 1;
				var obj = collision.gameObject;
				if (obj.layer == 14)
				{
					var v = attackForce * rigid.velocity.normalized;
					v.y = Mathf.Clamp(v.y, 3f, 20f);
					obj.GetComponent<CharacterMovement>().AddForce(v, true);
					obj.GetComponent<IHealth>().TakeDamage(damage);
					// Jump back to start position. (game feel)
					Jump(attackStartPoint, -0.5f);
					attackCount = 4;
				}

				if (currentHealth <= lowHealth || attackCount == 4)
				{
					attackCount = 0;
					isAttacking = false;
				}
				else
				{
					Jump(attackTarget.position, jumpAttackA);
				}
			}
		}

		public void TakeDamage(int damage)
		{
			if (currentHealth <= 0)
				return;

			if (damage >= 0)
				Helper.HitSFX.Play();

			rigid.isKinematic = false;
			rigid.AddForce(
				Projectile.VelocityByHeight(transform.position, transform.position + 0.3f * agent.velocity, 0.4f),
				ForceMode.VelocityChange);
			isJumping = true;
			agent.enabled = false;
			currentHealth -= damage;

			if (currentHealth <= 0)
			{
				Helper.DeadSFX.Play();
				Helper.Instance.EnemyKilled += 1;
				Destroy(gameObject);
				Instantiate(deadFX, transform.position, Quaternion.identity);
			}
			else if (currentHealth <= lowHealth)
			{
				currentDestination = healthPack;
			}
			else if (currentHealth <= maxHealth)
			{
				currentDestination = attackTarget;
			}
			
			if (currentHealth > maxHealth)
			{
				currentHealth = maxHealth;
			}
		}

		public bool CanSee(Transform obj)
		{
			var dir = obj.position - transform.position;
			return !Physics.Raycast(transform.position, dir, dir.magnitude, obstacleMask);
		}
	}
}