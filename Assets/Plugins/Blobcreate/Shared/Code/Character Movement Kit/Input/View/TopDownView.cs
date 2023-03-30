using UnityEngine;

namespace Blobcreate.Universal
{
	[DefaultExecutionOrder(-3)]
	public class TopDownView : MonoBehaviour
	{
		[SerializeField] protected Transform cameraMan;
		[SerializeField] Vector3 cameraOffset;
		[SerializeField] protected Transform body;
		[SerializeField] protected Transform head;
		[SerializeField] protected float bodyRotationSpeed = 0.1f;
		[SerializeField] protected float headRotationSpeed = 0.1f;
		[SerializeField] protected LayerMask groundMask;
		[SerializeField] Transform worldCursor;

		Camera cam;

		public Vector3 CameraOffset { get => cameraOffset; set => cameraOffset = value; }
		public Transform WorldCursor { get => worldCursor; set => worldCursor = value; }
		public Transform Body { get => body; set => body = value; }
		public Transform Head { get => head; set => head = value; }

		protected virtual void Start()
		{
			cam = Camera.main;
			Cursor.visible = false;
		}

		void Update()
		{
			SetLookTarget();
		}

		void LateUpdate()
		{
			SetCharacterRotation();
		}

		protected virtual void SetLookTarget()
		{
			var t = body.position + cameraOffset;
			cameraMan.transform.position = Vector3.Lerp(cameraMan.transform.position, t, 4f * Time.deltaTime);

			if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out var hit, 300f, groundMask))
			{
				worldCursor.position = hit.point;
				worldCursor.LookAt(worldCursor.position + hit.normal);
			}
		}

		protected virtual void SetCharacterRotation()
		{
			var pointer = worldCursor.position;
			var center = body.position;
			center.y = pointer.y;

			var r = Quaternion.LookRotation(pointer - center);
			body.rotation = Quaternion.Slerp(body.rotation, r, bodyRotationSpeed);

			if (head == null)
				return;

			// Limit the rotation angle.
			if ((pointer - center).sqrMagnitude < 4f)
				pointer = center + 2f * (pointer - center).normalized;

			var o = head.position.y > pointer.y ? Vector3.up : Vector3.down;
			r = Quaternion.LookRotation(pointer - (center + o));
			head.rotation = Quaternion.Slerp(head.rotation, r, headRotationSpeed);
		}
	}

}