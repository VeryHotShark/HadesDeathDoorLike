using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace MoreMountains.Tools
{
	/// <summary>
	/// This component will let you have a UI object follow the mouse position
	/// </summary>
	public class MMUIFollowMouse : MonoBehaviour
	{
		#if ENABLE_INPUT_SYSTEM
        [Header("Input System")]
        public InputAction MousePositionAction;
		#endif
		public Canvas TargetCanvas { get; set; }
		protected Vector2 _newPosition;
		protected Vector2 _mousePosition;

		protected virtual void Start()
		{
			#if ENABLE_INPUT_SYSTEM
            MousePositionAction.Enable();
            MousePositionAction.performed += context => _mousePosition = context.ReadValue<Vector2>();
            MousePositionAction.canceled += context => _mousePosition = Vector2.zero;
			#endif
		}
        
		protected virtual void LateUpdate()
		{
			#if !ENABLE_INPUT_SYSTEM
			_mousePosition = Input.mousePosition;
			#endif
			RectTransformUtility.ScreenPointToLocalPointInRectangle(TargetCanvas.transform as RectTransform, _mousePosition, TargetCanvas.worldCamera, out _newPosition);
			transform.position = TargetCanvas.transform.TransformPoint(_newPosition);
		}
	}
}