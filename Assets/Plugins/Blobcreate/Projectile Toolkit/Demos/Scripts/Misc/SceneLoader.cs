using UnityEngine;
using UnityEngine.SceneManagement;

namespace Blobcreate.ProjectileToolkit.Demo
{
	public class SceneLoader : MonoBehaviour
	{
		public void LoadScene(int index)
		{
			var current = SceneManager.GetActiveScene();
			if (current.name.Contains("Title Screen"))
				Helper.titleScreen = current.buildIndex;

			SceneManager.LoadScene(index);
		}

		public void Back()
		{
			if (Helper.titleScreen != -1)
			{
				SceneManager.LoadScene(Helper.titleScreen);
				Cursor.visible = true;
			}
			else
			{
#if UNITY_EDITOR
				UnityEditor.EditorApplication.isPlaying = false;
#endif
			}
		}

		public void Reload()
		{
			var i = SceneManager.GetActiveScene().buildIndex;
			LoadScene(i);
		}
	}
}