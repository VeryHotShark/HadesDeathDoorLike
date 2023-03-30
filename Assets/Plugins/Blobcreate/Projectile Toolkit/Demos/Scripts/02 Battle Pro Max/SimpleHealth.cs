using Blobcreate.Universal;
using UnityEngine;

namespace Blobcreate.ProjectileToolkit.Demo
{
	public class SimpleHealth : MonoBehaviour, IHealth
	{
		[SerializeField] int maxHealth = 100;
		public Transform deadFX;
		public AudioSource customHitSound;
		public AudioSource customDeadSound;

		int currentHealth;
		
		public int MaxHealth
		{
			get => maxHealth;
			set
			{
				maxHealth = value;
				currentHealth = maxHealth < currentHealth ? maxHealth : currentHealth;
			}
		}

		public virtual void TakeDamage(int damage)
		{
			if (currentHealth <= 0)
				return;

			currentHealth -= damage;

			if (currentHealth <= 0)
			{
				if (customDeadSound)
					customDeadSound.Play();
				else
					Helper.DeadSFX.Play();

				if (deadFX != null)
					Instantiate(deadFX, transform.position, Quaternion.identity);

				if (gameObject.layer == LayerMask.NameToLayer("Enemy"))
					Helper.Instance.EnemyKilled += 1;
				else
					Helper.Instance.OnPlayerDead();

				Destroy(gameObject);
			}
			else
			{
				if (damage >= 0)
				{
					if (customHitSound)
						customHitSound.Play();
					else
						Helper.HitSFX.Play();
				}

				if (currentHealth > maxHealth)
				{
					currentHealth = maxHealth;
				}
			}
		}

		void Awake()
		{
			currentHealth = maxHealth;
		}
	}
}