using System.Collections;
using System.Collections.Generic;
using App.Core;
// using App.Interactions;
// using App.Screens;
using App.Utils;
using UnityEngine;
using Vuforia;

namespace App.Managers {

    public class GameManager : BaseManager<GameManager> {

        private InputHandler _input;

        Swipe _swipe;
        public Swipe Swipe { get => _swipe; }

        VuforiaBehaviour _cam;
        public VuforiaBehaviour Cam { get => _cam; }

        [SerializeField]
        GameObject[] _systemPrefabs;
        [SerializeField]
        GameObject _augReality;

        bool _isARFound = false;
        public bool IsARFound {
            get { return _isARFound; }
        }

        List<GameObject> _initializedSystemPrefabList;


        // Start is called before the first frame update
        void Start() {
            this._input = new InputHandler();
            this._swipe = new Swipe();

            this._cam = FindObjectOfType<VuforiaBehaviour>();
            
            InitializeSystemPrefabs();            
        }

        // Update is called once per frame
        void Update() {
            this._input?.Handle()?.Execute();
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

        public void ChangeARPosition(GameObject busPoint) {
            _isARFound = true;
            _augReality.transform.parent = busPoint.transform;
            _augReality.transform.localPosition = new Vector3(0, 0, 0);
            _augReality.transform.localRotation = Quaternion.identity;
            _augReality.transform.localScale = new Vector3(1, 1, 1);
        }

        public void OnARLost() {
            _isARFound = false;
        }
        #endregion
    }
}