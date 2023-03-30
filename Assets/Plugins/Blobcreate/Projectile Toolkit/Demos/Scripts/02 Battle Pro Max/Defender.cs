using Blobcreate.Universal;
using UnityEngine;

namespace Blobcreate.ProjectileToolkit.Demo
{
	public class Defender : MonoBehaviour
	{
		public Transform attackTarget;
		public float perceptionRadius = 20f;
		public float perceptionInterval = 1f;
		public float timerOffset;
		public Transform bombLaunchPoint;
		public Rigidbody bombPrefab;
		public float bombFlyTime = 1f;
		[Tooltip("Smaller values bring higher accuracy, but more likely to be tricked by tiny movements.")]
		public float recordInterval = 0.1f;

		float timer;
		float estimationTimer;
		float lastRecordTime;
		Vector3 lastTargetPosition;

		void Start()
		{
			lastTargetPosition = attackTarget.position;
			timer = timerOffset;
		}

		void Update()
		{
			if (attackTarget == null)
				return;

			timer += Time.deltaTime;

			if (timer > perceptionInterval)
			{
				timer -= perceptionInterval;

				if (Vector3.SqrMagnitude(attackTarget.position - transform.position) <
					perceptionRadius * perceptionRadius)
				{
					// Predict the position of the target after time bombFlyTime.
					var predictedPos = attackTarget.position + bombFlyTime * EstimateVelocity();

					var b = Instantiate(bombPrefab, bombLaunchPoint.position, Quaternion.identity);
					var f = Projectile.VelocityByTime(b.position, predictedPos, bombFlyTime);
					b.AddForce(f, ForceMode.VelocityChange);

					// Initialize the ProjectileBehaviour.
					b.GetComponent<ProjectileBehaviour>().Launch(predictedPos);
				}
			}

			estimationTimer += Time.deltaTime;

			// Record the position of the target every recordInterval second.
			if (estimationTimer > recordInterval)
			{
				estimationTimer -= recordInterval;
				lastRecordTime = Time.time;
				lastTargetPosition = attackTarget.position;
			}

			var lookPoint = attackTarget.position;
			lookPoint.y = transform.position.y;
			transform.LookAt(lookPoint);
		}

		Vector3 EstimateVelocity()
		{
			var v = (attackTarget.position - lastTargetPosition) / (Time.time - lastRecordTime);
			v.y = 0f;
			return v;
		}
	}
}
