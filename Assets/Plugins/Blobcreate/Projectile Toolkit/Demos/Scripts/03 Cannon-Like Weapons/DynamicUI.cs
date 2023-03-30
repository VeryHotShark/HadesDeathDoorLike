using UnityEngine;
using UnityEngine.UI;

namespace Blobcreate.ProjectileToolkit.Demo
{
	public class DynamicUI : MonoBehaviour
	{
		public Text tofText;
		public Text rangeText;
		public AnimationCurve opacityCurve;

		float timer = 100;

		void Update()
		{
			var o = opacityCurve.Evaluate(timer);
			tofText.color = new Color(1, 1, 1, o);
			timer += Time.deltaTime;
		}

		public void SetTOFText(float tof)
		{
			if (!tofText.enabled)
				tofText.enabled = true;
			tofText.text = "Time of flight: " + tof.ToString("f2") + " s";
			timer = 0;
		}

		public void SetRangeText(float range)
		{
			if (!rangeText.enabled)
				rangeText.enabled = true;
			rangeText.text = "Max range: " + range.ToString("f2") + " m";
		}

		public void HideRangeText()
		{
			if (rangeText.enabled)
				rangeText.enabled = false;
		}
	}
}