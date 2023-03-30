using UnityEngine;

namespace Blobcreate.ProjectileToolkit.Demo
{
	public class WeaponPack : MonoBehaviour
	{
		public Transform gunPrefab;
		public GameObject enemy;
		public AudioSource pickupSound;

		void OnTriggerEnter(Collider other)
		{
			var gunPoint = other.transform.GetChild(0).Find("GunPoint");

			if (gunPoint != null)
			{
				Instantiate(gunPrefab, gunPoint);
				gameObject.SetActive(false);
				enemy.SetActive(true);
				pickupSound.Play();
			}
		}
	}
}