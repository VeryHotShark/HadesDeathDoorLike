using Blobcreate.Universal;
using UnityEngine;

namespace Blobcreate.ProjectileToolkit.Demo
{
	public class ProjectileLauncher : MonoBehaviour
	{
		public Transform launchPoint;
		public Rigidbody bulletPrefab;
		public LayerMask groundMask;
		public float torqueForce = 5f;
		public float smallA = -0.1f;
		public float bigA = -0.01f;
		public float lerpSpeed = 5f;
		public float reloadTime = 1f;
		public TrajectoryPredictor trajectory;
		public bool drawLine;

		float currentA;
		float currentTorque;
		bool isReloading;
		float reloadTimer;

		void Start()
		{
			currentA = smallA;
		}

		void LateUpdate()
		{
			if (Input.GetMouseButtonDown(0))
			{
				OnFireButtonDown();
			}
			else if (Input.GetMouseButton(0))
			{
				OnFireButton();
			}

			if (Input.GetMouseButtonUp(0))
			{
				OnFireButtonUp();
			}

			if (isReloading)
			{
				reloadTimer += Time.deltaTime;

				if (reloadTimer > reloadTime)
				{
					reloadTimer = 0f;
					isReloading = false;
				}
			}

			if (drawLine)
			{
				if (!isReloading)
				{
					Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),
						out var hit, 300f, groundMask);
					RenderLaunch(launchPoint.position, hit.point);
					trajectory.enabled = true;
				}
				else
				{
					trajectory.enabled = false;
				}
			}
		}

		public void RenderLaunch(Vector3 origin, Vector3 target)
		{
			var v = Projectile.VelocityByA(origin, target, currentA);
			trajectory.Render(origin, v, target, 16);
		}

		public void Fire(Vector3 target)
		{
			var b = Instantiate(bulletPrefab, launchPoint.position, launchPoint.rotation);
			b.GetComponent<ProjectileBehaviour>().Launch(target);

			// Magic happens!
			var f = Projectile.VelocityByA(b.position, target, currentA);
			b.AddForce(f, ForceMode.VelocityChange);

			// Add some torque, not necessary, but interesting.
			var t = Vector3.Lerp(torqueForce * Random.onUnitSphere,
				torqueForce * (target - launchPoint.position).normalized, currentTorque);
			b.AddTorque(t, ForceMode.VelocityChange);
		}

		void OnFireButtonDown()
		{
			currentA = smallA;
			currentTorque = 0f;
		}

		void OnFireButton()
		{
			currentA = Mathf.Lerp(currentA, bigA, lerpSpeed * Time.deltaTime);
			currentTorque = Mathf.Lerp(currentTorque, 1f, lerpSpeed * Time.deltaTime);
		}

		void OnFireButtonUp()
		{
			if (isReloading)
				return;

			Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hit, 300f, groundMask);
			Fire(hit.point);
			isReloading = true;
			currentA = smallA;
			currentTorque = 0f;
		}
	}
}