using System.Collections;
using System.Collections.Generic;
using App.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Vuforia;

public class FTDTrackableEventHandler : MonoBehaviour, ITrackableEventHandler
{
    [SerializeField] GameObject Wall;
    [SerializeField] GameObject ShowDoor;
    [SerializeField] GameObject OpenDoor;
    [SerializeField] GameObject DoorToAnim;
    [SerializeField] GameObject ARCameraTarget;

    [SerializeField] GameObject CanvasGroup; // UI com interações mais específicas de cada capa
    [SerializeField] GameObject Scene_manager; // precisa ter o mesmo nome do script

    private GameObject _groupBtnMissions;
    private GameObject _textMissions;
    private Transform _childrenContent;

    private Transform _doorTransform;

    Vector3 OriginalWallScale;

    Vector3 OriginalWallPosition;
    Quaternion OriginalDoorTransform;

    TrackableBehaviour _trackableBehaviour;

    [SerializeField] Button BtnStart;
    [SerializeField] Button BtnBack;

    [SerializeField] Material[] _matLogos;
    [SerializeField] Material[] _matBgs;

    [SerializeField] GameObject[] _wallLogos;
    [SerializeField] GameObject[] _wallBgs;

    private bool _canTrack;

    void Awake()
    {
        OriginalDoorTransform = DoorToAnim.transform.localRotation;
        OriginalWallScale = Wall.transform.lossyScale;
        OriginalWallPosition = Wall.transform.localPosition;

        _canTrack = false;

        _doorTransform = Wall.transform;

        GameObject[] rootGameObjs = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
        foreach (GameObject element in rootGameObjs)
        {
            if (element.name == "Canvas")
            {
                _groupBtnMissions = element.transform.Find("ScreenGame/Group_Btn_Missions").gameObject;
                _textMissions = element.transform.Find("ScreenGame/Mission_Text").gameObject;
            }
        }
        _childrenContent = this.gameObject.transform.GetChild(0);

        BtnStart.onClick.AddListener(delegate { SetTrackInteraction(true); });
        BtnBack.onClick.AddListener(delegate { SetTrackInteraction(false); });
    }

    void SetTrackInteraction(bool canTrack)
    {
        _canTrack = canTrack;
    }


    // Start is called before the first frame update
    void Start()
    {
        _trackableBehaviour = GetComponent<TrackableBehaviour>();
        if (_trackableBehaviour)
        {
            _trackableBehaviour.RegisterTrackableEventHandler(this);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnDestroy()
    {
        if (_trackableBehaviour)
        {
            _trackableBehaviour.UnregisterTrackableEventHandler(this);
        }
    }

    #region custom methods

    void OnTrackingFound()
    {
        if (!_canTrack) return;

        if (Scene_manager != null)
        {
            Scene_manager?.SetActive(true);
        }

        Wall.transform.parent = _childrenContent;
        Wall.transform.localPosition = OriginalWallPosition;
        Wall.transform.localEulerAngles = Vector3.zero;
        Wall.transform.localScale = OriginalWallScale;

        TreatWallColor();

        var rendererComponents = GetComponentsInChildren<Renderer>(true);
        var colliderComponents = GetComponentsInChildren<Collider>(true);
        var canvasComponents = GetComponentsInChildren<Canvas>(true);

        // Enable rendering:
        foreach (var component in rendererComponents)
            component.enabled = true;

        // Enable colliders:
        foreach (var component in colliderComponents)
            component.enabled = true;

        // Enable canvas':
        foreach (var component in canvasComponents)
            component.enabled = true;

        //Scene_manager.transform.GetChild(0).gameObject.SetActive(false);

        ShowDoor.GetComponent<Animator>().SetTrigger("ShowDoor");
        Invoke("ChangeDoors", 2);

        

        if (CanvasGroup != null)
        {
            CanvasGroup?.SetActive(true);
        }

        if (_groupBtnMissions != null)
        {
            _groupBtnMissions?.SetActive(true);
        }

        if (_textMissions != null)
        {
            _textMissions?.SetActive(true);
        }

        Invoke("OpenBook", 5);
    }

    void TreatWallColor()
    {
        string name = this.gameObject.name;
        Material matLogo;
        Material matBG;
        if (name.Contains("Ciencias"))
        {
            matLogo = _matLogos[0];
            matBG = _matBgs[0];
        }
        else if (name.Contains("Geografia"))
        {
            matLogo = _matLogos[1];
            matBG = _matBgs[1];
        }
        else if (name.Contains("Historia"))
        {
            matLogo = _matLogos[2];
            matBG = _matBgs[2];
        }
        else if (name.Contains("Matematica"))
        {
            matLogo = _matLogos[3];
            matBG = _matBgs[3];
        }
        else
        {
            matLogo = _matLogos[4];
            matBG = _matBgs[4];
        }

        foreach(GameObject element in _wallLogos)
        {
            element.GetComponent<Renderer>().material = matLogo;
        }
        foreach(GameObject element in _wallBgs)
        {
            element.GetComponent<Renderer>().material = matBG;
        }
    }

    private void OpenBook()
    {
        if ( Scene_manager.activeInHierarchy )
        {
            Scene_manager.GetComponent(Scene_manager.name).SendMessage("StartBook");
        }
    }

    public void ChangeDoors()
    {
        OpenDoor.SetActive(true);
        ShowDoor.GetComponent<Animator>().SetTrigger("Reset");
        ShowDoor.SetActive(false);

        if ( Scene_manager.transform.childCount > 0  )
        {
            Scene_manager.transform.GetChild(0).gameObject.SetActive(true);
            StartCoroutine(AnimDoor());
        }
    }

    void OnTrackingLost()
    {
        var rendererComponents = GetComponentsInChildren<Renderer>(true);
        var colliderComponents = GetComponentsInChildren<Collider>(true);
        var canvasComponents = GetComponentsInChildren<Canvas>(true);

        // Disable rendering:
        foreach (var component in rendererComponents)
            component.enabled = false;

        // Disable colliders:
        foreach (var component in colliderComponents)
            component.enabled = false;

        // Disable canvas':
        foreach (var component in canvasComponents)
            component.enabled = false;

        StopAllCoroutines();
        DoorToAnim.GetComponent<Animator>().SetTrigger("Close");
        OpenDoor.SetActive(false);
        ShowDoor.SetActive(true);
        DoorToAnim.transform.localRotation = OriginalDoorTransform;
        Wall.SetActive(true);
        Wall.transform.localPosition = OriginalWallPosition;

        if (Scene_manager != null)
        {
            Scene_manager?.SetActive(false);
        }

        if (CanvasGroup != null)
        {
            CanvasGroup?.SetActive(false);
        }

        if (_textMissions != null)
        {
            _textMissions?.SetActive(false);
        }

        if (_groupBtnMissions != null)
        {
            _groupBtnMissions?.SetActive(false);
            foreach (Transform child in _groupBtnMissions.transform)
            {
                if (!child.name.Contains("Blocker"))
                    child.GetComponent<Animator>().SetTrigger("Reset");
            }
        }
    }

    #endregion

    #region Coroutines

    IEnumerator AnimDoor()
    {
        DoorToAnim.GetComponent<Animator>().SetTrigger("Play");
        yield return new WaitForSeconds(2);
        for (; Vector3.Distance(Wall.transform.position, ARCameraTarget.transform.position) > 0.4f;)
        {
            Wall.transform.position = Vector3.Lerp(Wall.transform.position, ARCameraTarget.transform.position,
                3f * Time.deltaTime);
            yield return null;
        }

        Wall.SetActive(false);
    }

    #endregion

    #region interface methods

    public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
    {
        if (newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED)
        {
            Debug.Log("Trackable " + _trackableBehaviour.TrackableName + " found");
            OnTrackingFound();
        }
        else if ((previousStatus == TrackableBehaviour.Status.TRACKED &&
                 newStatus == TrackableBehaviour.Status.NO_POSE)||
                newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {
            Debug.Log("Trackable " + _trackableBehaviour.TrackableName + " lost");
            OnTrackingLost();
        }
        else
        {
            // For combo of previousStatus=UNKNOWN + newStatus=UNKNOWN|NOT_FOUND
            // Vuforia is starting, but tracking has not been lost or found yet
            // Call OnTrackingLost() to hide the augmentations
            OnTrackingLost();
        }
    }

    #endregion
}