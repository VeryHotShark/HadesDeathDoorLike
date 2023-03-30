using UnityEngine;
using UnityEngine.SceneManagement;

namespace Blobcreate.ProjectileToolkit.Demo
{
	[DefaultExecutionOrder(-200)]
	public class Helper : MonoBehaviour
	{
		[SerializeField] AudioSource hitSFX;
		[SerializeField] AudioSource deadSFX;
		[SerializeField] int totalEnemyCount;
		[SerializeField] Canvas winCanvas;
		[SerializeField] Canvas loseCanvas;

		public static int titleScreen = -1;
		public static AudioSource HitSFX;
		public static AudioSource DeadSFX;
		static Helper self;

		public static Helper Instance => self;

		int enemyKilled;

		public int EnemyKilled
		{
			get => enemyKilled;
			set
			{
				enemyKilled = value;
				if (enemyKilled >= totalEnemyCount)
				{
					winCanvas.enabled = true;
					Cursor.visible = true;
				}
			}
		}

		public void OnPlayerDead()
		{
			loseCanvas.enabled = true;
			Cursor.visible = true;
		}


		void Awake()
		{
			HitSFX = hitSFX;
			DeadSFX = deadSFX;
			self = this;
		}
	}
}