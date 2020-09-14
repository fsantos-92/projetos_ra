using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseManager<T> : MonoBehaviour where T : MonoBehaviour {

    static T _instance;

    public static T Instance {
        get { return _instance; }
        set {
            if (_instance == null) {
                _instance = value;
                DontDestroyOnLoad(_instance.gameObject);
            } else if (_instance != value) {
                Destroy(_instance.gameObject);
            }
        }
    }

    void Awake() {
        _instance = this as T;
    }
}