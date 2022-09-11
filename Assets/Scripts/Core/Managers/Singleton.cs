using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {
    private static T _instance;
    private static bool _applicationQuit;

    private void Awake() {
        if (_instance) {
            Destroy(gameObject);
            return;
        }
        
        _instance = this as T;
        DontDestroyOnLoad(gameObject);   
        OnAwake();
    }
    
    protected static T Instance => _instance || _applicationQuit ? _instance : new GameObject(typeof(T).Name, typeof(T)).GetComponent<T>();

    protected virtual void OnAwake() {}
    protected virtual void OnApplicationQuit() => _applicationQuit = true;
}
