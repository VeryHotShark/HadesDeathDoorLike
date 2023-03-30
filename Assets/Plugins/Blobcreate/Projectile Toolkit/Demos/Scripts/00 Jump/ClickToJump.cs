using UnityEngine;

namespace Blobcreate.ProjectileToolkit.Demo
{
	public class ClickToJump : MonoBehaviour
	{
		public JumpTester cube1;
		public JumpTester cube2;

		JumpTester currentCube;

		void Start()
		{
			currentCube = cube1;
		}

		void Update()
		{
			if (Input.GetKeyDown(KeyCode.Alpha1))
				currentCube = cube1;
			else if (Input.GetKeyDown(KeyCode.Alpha2))
				currentCube = cube2;

			if (Input.GetMouseButtonDown(0))
			{
				if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var mouseHitInfo, 200f))
				{
					currentCube.TargetHasChanged = true;
					currentCube.target = mouseHitInfo.point + new Vector3(0f, currentCube.halfHeight, 0f);
				}
			}
		}
	}
}