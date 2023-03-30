using UnityEngine;
using UnityEngine.UI;

namespace Blobcreate.ProjectileToolkit.Demo
{
	public class UIHandler : MonoBehaviour
	{
		public CannonLike launcher;
		public CannonLike launcher2;

		public Canvas byAngle;
		public Canvas bySpeed;
		public Canvas angleMode;
		public Canvas angleMode2;
		public Text angleText;
		public Text speedText;
		public Text index1;
		public Text index2;
		public Text index3;

		Canvas currentCanvas;
		Text currentIndexText;
		bool isDoubleLauncher;

		void Update()
		{
			if (Input.GetKeyDown(KeyCode.Alpha1))
			{
				launcher.type = CannonLike.LaunchType.VelocityByAngle;
				launcher2.type = CannonLike.LaunchType.VelocityByAngle;
				ChangeCanvas(byAngle, index1);
			}
			else if (Input.GetKeyDown(KeyCode.Alpha2))
			{
				launcher.type = CannonLike.LaunchType.AnglesBySpeed;
				launcher2.type = CannonLike.LaunchType.AnglesBySpeed;
				ChangeCanvas(bySpeed, index2);
			}
			else if (Input.GetKeyDown(KeyCode.Alpha3))
			{
				launcher.type = CannonLike.LaunchType.VelocitiesBySpeed;
				launcher2.type = CannonLike.LaunchType.VelocitiesBySpeed;
				ChangeCanvas(bySpeed, index3);
			}

			if (Input.GetKeyDown(KeyCode.D))
			{
				isDoubleLauncher = !isDoubleLauncher;
				launcher2.gameObject.SetActive(isDoubleLauncher);
				ChangeCanvas(currentCanvas, currentIndexText);
			}
		}

		void ChangeCanvas(Canvas c, Text t)
		{
			if (currentIndexText != null)
				currentIndexText.enabled = false;
			currentIndexText = t;
			currentIndexText.enabled = true;

			if (currentCanvas != null)
				currentCanvas.enabled = false;
			currentCanvas = c;
			currentCanvas.enabled = true;

			if (currentCanvas == bySpeed)
			{
				angleMode.enabled = true;
				angleMode2.enabled = isDoubleLauncher;
			}
			else
			{
				angleMode.enabled = false;
				angleMode2.enabled = false;
			}
		}

		public void ChangeLaunchAngle(float angle)
		{
			launcher.launchAngle = angle;
			launcher2.launchAngle = angle;
			angleText.text = "Launch Angle: " + angle.ToString() + "°";
		}

		public void ChangeLaunchSpeed(float speed)
		{
			launcher.launchSpeed = speed;
			launcher2.launchSpeed = speed;
			speedText.text = "Launch Speed: " + speed.ToString() + " m/s";
		}

		public void ChooseAngleMode(bool useHighAngle)
		{
			launcher.useHighAngle = useHighAngle;
			launcher.useLowAngle = !useHighAngle;
		}

		public void ChooseAngleMode2(bool useHighAngle)
		{
			launcher2.useHighAngle = useHighAngle;
			launcher2.useLowAngle = !useHighAngle;
		}
	}
}