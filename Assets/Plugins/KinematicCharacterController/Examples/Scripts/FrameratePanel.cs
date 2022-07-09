using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace KinematicCharacterController.Examples
{
    public class FrameratePanel : MonoBehaviour
    {
        [FormerlySerializedAs("PollingRate")] public float _pollingRate = 1f;
        [FormerlySerializedAs("PhysicsRate")] public Text _physicsRate;
        [FormerlySerializedAs("PhysicsFPS")] public Text _physicsFPS;
        [FormerlySerializedAs("AvgFPS")] public Text _avgFPS;
        [FormerlySerializedAs("AvgFPSMin")] public Text _avgFPSMin;
        [FormerlySerializedAs("AvgFPSMax")] public Text _avgFPSMax;

        public Action<float> OnPhysicsFPSReady;

        [FormerlySerializedAs("FramerateStrings")] public string[] _framerateStrings = new string[0];

        private bool _isFixedUpdateThisFrame = false;
        private bool _wasFixedUpdateLastFrame = false;
        private int _physFramesCount = 0;
        private float _physFramesDeltaSum = 0;

        private int _framesCount = 0;
        private float _framesDeltaSum = 0;
        private float _minDeltaTimeForAvg = Mathf.Infinity;
        private float _maxDeltaTimeForAvg = Mathf.NegativeInfinity;
        private float _timeOfLastPoll = 0;

        private void FixedUpdate()
        {
            _isFixedUpdateThisFrame = true;
        }

        void Update()
        {
            // Regular frames
            _framesCount++;
            _framesDeltaSum += Time.deltaTime;

            // Max and min
            if (Time.deltaTime < _minDeltaTimeForAvg)
            {
                _minDeltaTimeForAvg = Time.deltaTime;
            }
            if (Time.deltaTime > _maxDeltaTimeForAvg)
            {
                _maxDeltaTimeForAvg = Time.deltaTime;
            }

            // Fixed frames
            if (_wasFixedUpdateLastFrame)
            {
                _wasFixedUpdateLastFrame = false;

                _physFramesCount++;
                _physFramesDeltaSum += Time.deltaTime;
            }
            if (_isFixedUpdateThisFrame)
            {
                _wasFixedUpdateLastFrame = true;
                _isFixedUpdateThisFrame = false;
            }

            // Polling timer
            float timeSinceLastPoll = (Time.unscaledTime - _timeOfLastPoll);
            if (timeSinceLastPoll > _pollingRate)
            {
                float physicsFPS = 1f / (_physFramesDeltaSum / _physFramesCount);

                _avgFPS.text = GetNumberString(Mathf.RoundToInt(1f / (_framesDeltaSum / _framesCount)));
                _avgFPSMin.text = GetNumberString(Mathf.RoundToInt(1f / _maxDeltaTimeForAvg));
                _avgFPSMax.text = GetNumberString(Mathf.RoundToInt(1f / _minDeltaTimeForAvg));
                _physicsFPS.text = GetNumberString(Mathf.RoundToInt(physicsFPS));

                if(OnPhysicsFPSReady != null)
                {
                    OnPhysicsFPSReady(physicsFPS);
                }

                _physFramesDeltaSum = 0;
                _physFramesCount = 0;
                _framesDeltaSum = 0f;
                _framesCount = 0;
                _minDeltaTimeForAvg = Mathf.Infinity;
                _maxDeltaTimeForAvg = Mathf.NegativeInfinity;

                _timeOfLastPoll = Time.unscaledTime;
            }

            _physicsRate.text = GetNumberString(Mathf.RoundToInt(1f / Time.fixedDeltaTime));
        }

        public string GetNumberString(int fps)
        {
            if (fps < _framerateStrings.Length - 1 && fps >= 0)
            {
                return _framerateStrings[fps];
            }
            else
            {
                return _framerateStrings[_framerateStrings.Length - 1];
            }
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(FrameratePanel))]
    public class FrameratePanelEditor : Editor
    {
        private const int MaxFPS = 999;

        private void OnEnable()
        {
            InitStringsArray();
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("Init strings array"))
            {
                InitStringsArray();
            }
        }

        private void InitStringsArray()
        {
            FrameratePanel fp = target as FrameratePanel;
            fp._framerateStrings = new string[MaxFPS + 1];

            for (int i = 0; i < fp._framerateStrings.Length; i++)
            {
                if (i >= fp._framerateStrings.Length - 1)
                {
                    fp._framerateStrings[i] = i.ToString() + "+" + " (<" + (1000f / (float)i).ToString("F") + "ms)";
                }
                else
                {
                    fp._framerateStrings[i] = i.ToString() + " (" + (1000f/(float)i).ToString("F") + "ms)" ;
                }
            }
        }
    }
#endif
}