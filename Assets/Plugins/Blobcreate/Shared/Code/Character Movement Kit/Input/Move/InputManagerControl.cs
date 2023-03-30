using UnityEngine;

namespace Blobcreate.Universal
{
	[DefaultExecutionOrder(-3)]
	public class InputManagerControl : MonoBehaviour
	{
		[SerializeField] CharacterMovement mover;

		void Update()
		{
			mover.Direction = GetInputDirection();
			mover.JumpInput = GetJumpInput();
		}

		Vector3 GetInputDirection()
		{
			return Vector3.Normalize(
				new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")));
		}

		float GetJumpInput()
		{
			if (mover.IsGrounded)
			{
				if (Input.GetButtonDown("Jump"))
					return mover.JumpSpeed;
			}

			return 0f;
		}
	}
}