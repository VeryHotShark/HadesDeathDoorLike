using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {
    [SerializeField] private bool _persistent = false;
    
    private static T _instance;
    private static bool _applicationQuit;

    private void Awake() {
        if (_instance) {
            Destroy(gameObject);
            return;
        }
        
        _instance = this as T;
        
        if(_persistent)
            DontDestroyOnLoad(gameObject);
        
        OnAwake();
    }
    
    protected static T Instance {
        get {
            if (_applicationQuit)
                return _instance;
            
            return _instance ? _instance : new GameObject(typeof(T).Name, typeof(T)).GetComponent<T>();
        }
    }

    protected virtual void OnAwake() {}
    private void OnApplicationQuit() => _applicationQuit = true;
}
