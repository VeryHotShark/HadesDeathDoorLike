using UnityEngine;

// Very simple smooth mouselook modifier for the MainCamera in Unity
// by Francis R. Griffiths-Keam - www.runningdimensions.com

namespace ShaderCrew.SeeThroughShader
{

    public class FreeLookNorbit : MonoBehaviour
    {
        private Vector2 _mouseAbsolute;
        private Vector2 _smoothMouse;
        private Vector2 mouseDelta;
        public bool ThirdPersonMode = true;
        public Vector2 clampInDegrees = new Vector2(360, 180);
        public Vector2 sensitivity = new Vector2(2, 2);
        public Vector2 smoothing = new Vector2(3, 3);
        private Vector2 targetDirection;
        private Vector2 targetCharacterDirection;
        private Vector2 startTargetDirection;
        public GameObject mainChar;
        private bool lookAtPlayer = false;


        // Assign this if there's a parent object controlling motion, such as a Character Controller.
        // Yaw rotation will affect this object instead of the camera if set.
        public GameObject characterBody;

        void Start()
        {
            if (mainChar == null)
            {
                mainChar = GameObject.FindGameObjectWithTag("Player");
            }

            // Set target direction to the camera's initial orientation.            
            targetDirection = transform.rotation.eulerAngles;
            startTargetDirection = transform.localRotation.eulerAngles;

            // Set target direction for the character body to its inital state.
            if (characterBody) targetCharacterDirection = characterBody.transform.localRotation.eulerAngles;
        }

        void Update()
        {

            if (Input.GetMouseButton(2) && ThirdPersonMode || !ThirdPersonMode || lookAtPlayer)
            {
                if (lookAtPlayer)
                {

                    Vector3 relativePos = mainChar.transform.position - transform.position;

                    // the second argument, upwards, defaults to Vector3.up
                    Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
                    transform.rotation = rotation;
                    targetDirection = transform.rotation.eulerAngles;
                    transform.LookAt(mainChar.transform.position);
                    lookAtPlayer = false;

                }
                // Allow the script to clamp based on a desired target value.                

                var targetOrientation = Quaternion.Euler(targetDirection);
                var targetCharacterOrientation = Quaternion.Euler(targetCharacterDirection);

                // Get raw mouse input for a cleaner reading on more sensitive mice.
                mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

                // Scale input against the sensitivity setting and multiply that against the smoothing value.
                mouseDelta = Vector2.Scale(mouseDelta, new Vector2(sensitivity.x * smoothing.x, sensitivity.y * smoothing.y));

                // Interpolate mouse movement over time to apply smoothing delta.
                _smoothMouse.x = Mathf.Lerp(_smoothMouse.x, mouseDelta.x, 1f / smoothing.x);
                _smoothMouse.y = Mathf.Lerp(_smoothMouse.y, mouseDelta.y, 1f / smoothing.y);

                // Find the absolute mouse movement value from point zero.
                _mouseAbsolute += _smoothMouse;

                // Clamp and apply the local x value first, so as not to be affected by world transforms.
                if (clampInDegrees.x < 360)
                    _mouseAbsolute.x = Mathf.Clamp(_mouseAbsolute.x, -clampInDegrees.x * 0.5f, clampInDegrees.x * 0.5f);

                // Then clamp and apply the global y value.
                if (clampInDegrees.y < 360)
                    _mouseAbsolute.y = Mathf.Clamp(_mouseAbsolute.y, -clampInDegrees.y * 0.5f, clampInDegrees.y * 0.5f);

                var xRotation = Quaternion.AngleAxis(-_mouseAbsolute.y, targetOrientation * Vector3.right);
                transform.localRotation = xRotation;


                transform.localRotation *= targetOrientation;


                // If there's a character body that acts as a parent to the camera

                var yRotation = Quaternion.AngleAxis(_mouseAbsolute.x, transform.InverseTransformDirection(Vector3.up));
                transform.localRotation *= yRotation;


            }
        }
    }

}