#if UNITY_EDITOR

using UnityEngine.Rendering;
using UnityEngine;

namespace Blobcreate.ProjectileToolkit.Demo
{
	[ExecuteInEditMode]
	public class RPTester : MonoBehaviour
	{
		public Material testMat;
		public float linearSunIntensity = 3f;
		public float GammaSunIntensity = 1.5f;

		const string message = "Materials are Built-in RP materials and have not been converted. See Manual.pdf > Explore the demos > In editor.";

		float timer = 3f;
		int rpType = 0;

		void Start()
		{
			var l = GameObject.Find("Directional Light").GetComponent<Light>();

			// Light in built-in RP use gamma intensity by default, so change the intensity to fit.
			if (!GraphicsSettings.lightsUseLinearIntensity)
			{
				l.intensity = GammaSunIntensity;
				l.shadowBias = 0.1f;
				l.shadowNormalBias = 0.2f;
			}
			else
			{
				l.intensity = linearSunIntensity;
			}
		}

		void OnGUI()
		{
			timer += Time.deltaTime;

			if (timer > 2f)
			{
				if (GraphicsSettings.currentRenderPipeline)
				{
					if (testMat.shader.name == "Standard")
					{
						var rp = GraphicsSettings.currentRenderPipeline.GetType().ToString();
						if (rp.Contains("Universal"))
							rpType = 1;
						else if (rp.Contains("HighDefinition"))
							rpType = 2;
						else
							rpType = 69;

						Debug.Log(message);
					}
					else
					{
						rpType = 0;
					}
				}

				timer = 0f;
			}

			GUI.skin.label.fontSize = 24;

			if (rpType != 0)
			{
				if (rpType == 1)
					GUI.Label(new Rect(Screen.width / 4, Screen.height / 4, Screen.width / 2, Screen.height / 2), message);
				else if (rpType == 2)
					GUI.Label(new Rect(Screen.width / 4, Screen.height / 4, Screen.width / 2, Screen.height / 2), message +
						"\n\nYou also need to replace the Skybox material with one suitable for HDRP manually.");
				else if (rpType == 69)
					GUI.Label(new Rect(Screen.width / 4, Screen.height / 4, Screen.width / 2, Screen.height / 2),
						"The materials are built-in RP materials. Please update them to the equivalent in your custom RP.");
			}

			if (Physics.gravity.y != -29.43f || LayerMask.LayerToName(17) != "Platform")
			{
				var t = "The physics or layer settings seem not set up correctly. See README.";
				GUI.Label(new Rect(Screen.width / 4, Screen.height / 2, Screen.width / 2, Screen.height / 2), t);
			}
		}

	}
}

#endif