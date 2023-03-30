using UnityEngine;
using UnityEngine.UI;

namespace Blobcreate.ProjectileToolkit.Demo
{
	public class EscMenu : MonoBehaviour
	{
		public Canvas menuPrefab;
		[TextArea]
		public string description;

		Canvas menu;

		void Update()
		{
			if (Input.GetKeyDown(KeyCode.X))
			{
				if (menu != null)
				{
					menu.enabled = !menu.enabled;
				}
				else
				{
					menu = Instantiate(menuPrefab);
					var t = menu.GetComponentInChildren<Text>();
					t.alignment = TextAnchor.UpperLeft;
					t.fontSize = 24;
					t.text = description;
				}
			}
		}
	}
}