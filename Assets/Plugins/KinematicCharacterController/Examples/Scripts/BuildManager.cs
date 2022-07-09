using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class BuildManager : MonoBehaviour
{
    [FormerlySerializedAs("WebGLCanvas")] public GameObject _webGLCanvas;
    [FormerlySerializedAs("WarningPanel")] public GameObject _warningPanel;

    void Awake()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        WebGLCanvas.SetActive(true);
#endif
    }

    private void Update()
    {
        if(Keyboard.current.f1Key.wasPressedThisFrame)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }

#if UNITY_WEBGL && !UNITY_EDITOR
        if(Input.GetKeyDown(KeyCode.H))
        {
            WarningPanel.SetActive(!WarningPanel.activeSelf);
        }
#endif
    }
}
