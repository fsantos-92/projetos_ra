using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public class Cie03_manager : BaseBookManager, IClickInteraction
{
    GameObject Shadow;
    [SerializeField] GameObject questionBox;
    [SerializeField] AudioClip ShowBox;
    [SerializeField] AudioClip HideBox;
    [SerializeField] Button[] btnAnswers;
    GameObject Master;
    GameObject[] stars;
    [SerializeField] AudioClip starsAppear;

    public override void StartBook()
    {
        base.StartBook();

        List<GameObject> _stars = new List<GameObject>();

        foreach (Transform element in transform.GetComponentsInChildren<Transform>())
        {
            if (element.name == ("TrapezistaSombra"))
                Shadow = element.gameObject;
            if (element.name == ("Mestre"))
                Master = element.gameObject;
            if (element.name.Contains("Estrelas"))
                _stars.Add(element.gameObject);
        }
        stars = _stars.ToArray();
        StarsColliderState(false);
    }

    bool[] _canPlayFeedback;

    protected override void BindButtonEvents()
    {
        base.BindButtonEvents();
        btnAnswers[0].onClick.AddListener(BtnCircusClick);
    }

    private void BtnCircusClick()
    {
        btnAnswers[0].gameObject.GetComponent<Animator>().SetTrigger("Right");
        btnAnswers[0].gameObject.GetComponent<AudioSource>().Play();
        ClearMission(2, _canPlayFeedback[1]);
        _canPlayFeedback[1] = false;
        StartCoroutine(HideQuestionBox(2f));
    }

    protected override void InitializeVariables()
    {
        base.InitializeVariables();
        _canPlayFeedback = new bool[3] { true, true, true };
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        foreach (Button element in btnAnswers)
        {
            if (element.gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("IdleShown"))
                element.gameObject.GetComponent<Animator>().SetTrigger("Hide");
            element.gameObject.SetActive(false);
        }
        if (questionBox.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("IdleShown"))
            questionBox.GetComponent<Animator>().SetTrigger("Hide");
        questionBox.SetActive(false);
    }

    protected override void PlayInstructionAudio(int type)
    {
        base.PlayInstructionAudio(type);

        if (type == 2)
        {
            if (questionBox.activeInHierarchy)
                StartCoroutine(HideQuestionBox());
            else
                StartCoroutine(ShowQuestionBox());
        }
        else
        {
            if (questionBox.activeInHierarchy)
                StartCoroutine(HideQuestionBox());
        }
    }

    IEnumerator ShowQuestionBox()
    {
        _audioSource.PlayOneShot(ShowBox);
        questionBox.SetActive(true);
        questionBox.GetComponent<Animator>().SetTrigger("Show");
        yield return new WaitForSeconds(0.5f);
        foreach (Button element in btnAnswers)
        {
            element.gameObject.SetActive(true);
            element.gameObject.GetComponent<Animator>().SetTrigger("Show");
            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator HideQuestionBox(float delay = 0)
    {
        yield return new WaitForSeconds(delay);
        foreach (Button element in btnAnswers)
        {
            element.gameObject.GetComponent<Animator>().SetTrigger("Hide");
            yield return new WaitForSeconds(0.5f);
            element.gameObject.SetActive(false);
        }
        questionBox.GetComponent<Animator>().SetTrigger("Hide");
        _audioSource.PlayOneShot(HideBox);
        yield return new WaitForSeconds(0.5f);
        questionBox.SetActive(false);
    }

    void StarsColliderState(bool isEnabled)
    {
        foreach (GameObject element in stars)
        {
            BoxCollider[] cols = element.GetComponents<BoxCollider>();
            foreach (BoxCollider component in cols)
            {
                component.enabled = isEnabled;
            }
        }
        if (isEnabled)
            _audioSource.PlayOneShot(starsAppear);
    }

    void ResetAnim()
    {
        _isAnimating = false;

    }

    void AnimateObj(GameObject obj, Animator objAnim, int mission)
    {
        _isAnimating = true;
        objAnim.SetTrigger("Play");
        obj.GetComponent<AudioSource>().Play();

        ClearMission(mission, _canPlayFeedback[mission - 1]);
        _canPlayFeedback[mission - 1] = false;
        Invoke("ResetAnim", 2);
    }

    public void ClickInteraction(GameObject obj)
    {
        if (_isAnimating)
            return;
        Animator objAnim = obj.GetComponent<Animator>();
        string objName = obj.name;

        if (objAnim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            if (obj == Shadow  && activityUnlocked[0])
            {
                AnimateObj(obj, objAnim, 1);
                obj.GetComponent<BoxCollider>().enabled = false;
            }
            else if (obj == Master && activityUnlocked[2] )
            {
                if (_canPlayFeedback[2])
                {
                    foreach (GameObject element in stars)
                    {
                        element.GetComponent<Animator>().SetTrigger("Show");
                    }
                    StarsColliderState(true);
                }
                AnimateObj(obj, objAnim, 3);
            }
            else if (objName.Contains("Estrelas"))
            {
                foreach (GameObject element in stars)
                {
                    element.GetComponent<Animator>().SetTrigger("Play");
                    element.GetComponent<AudioSource>().Play();
                }
            }
        }
    }
    
    
    protected override void ResetProgress()
    {
        base.ResetProgress();

        InitializeVariables();
        questionBox.transform.parent.gameObject.SetActive( false );
    }
    
}
