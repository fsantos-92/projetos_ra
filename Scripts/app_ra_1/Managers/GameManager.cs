using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : BaseManager<GameManager> {

    [SerializeField]
    GameObject[] _systemPrefabs;

    List<GameObject> _initializedSystemPrefabList;

    // Start is called before the first frame update
    void Start() {
        InitializeSystemPrefabs();
    }

    // Update is called once per frame
    void Update() {
        
    }

    void OnDestroy() {
        foreach (GameObject item in _initializedSystemPrefabList) {
            Destroy(item);
        }
        _initializedSystemPrefabList.Clear();
    }

    #region custom methods
    void InitializeSystemPrefabs() {
        _initializedSystemPrefabList = new List<GameObject>();
        GameObject systemPrefab;
        foreach (GameObject item in _systemPrefabs) {
            systemPrefab = Instantiate(item);
            _initializedSystemPrefabList.Add(systemPrefab);
        }
    }
    #endregion
}
