using UnityEngine;

namespace Blobcreate.Universal
{
	public class HealthPack : MonoBehaviour
	{
		[SerializeField] int value = 100;
		[SerializeField] AudioSource pickupSound;
		[SerializeField] GameObject healthPackModel;
		[SerializeField] bool respawn;
		[SerializeField] float respawnInterval;

		float timer;

		void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent<IHealth>(out var health))
			{
				health.TakeDamage(-value);
				healthPackModel.SetActive(false);
				pickupSound.Play();
			}
		}

		void Update()
		{
			if (!respawn)
				return;

			if (!healthPackModel.activeSelf)
			{
				timer += Time.deltaTime;
				if (timer > respawnInterval)
				{
					timer = 0f;
					healthPackModel.SetActive(true);
				}
			}
		}
	}
}