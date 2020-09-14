using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using App.Game;

public class FTDTrackableEventHandler : MonoBehaviour, ITrackableEventHandler
{

    TrackableBehaviour _trackableBehaviour;

    [SerializeField]
    GameObject BtnToQuestion;

    [SerializeField]
    GameObject ScreenTakePicture;
    // Start is called before the first frame update
    void Start()
    {
        _trackableBehaviour = GetComponent<TrackableBehaviour>();
        if (_trackableBehaviour)
        {
            _trackableBehaviour.RegisterTrackableEventHandler(this);
        }
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
        if (!this.ScreenTakePicture.activeInHierarchy)
            if (GameObject.FindWithTag("InGame").GetComponent<InGame>().GetCurrentCardName() != this.gameObject.name)
                return;


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

        SoundManager.Instance.PlayTrackingFound();
        StartCoroutine(ShowQuestionButton());
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
        this.BtnToQuestion.SetActive(false);
    }
    #endregion

    IEnumerator ShowQuestionButton()
    {
        yield return new WaitForSeconds(2f);
        this.BtnToQuestion.SetActive(true);
    }

    #region interface methods
    public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
    {
        if (newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED ||
            newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {
            Debug.Log("Trackable " + _trackableBehaviour.TrackableName + " found");
            OnTrackingFound();
        }
        else if (previousStatus == TrackableBehaviour.Status.TRACKED &&
                 newStatus == TrackableBehaviour.Status.NO_POSE)
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
