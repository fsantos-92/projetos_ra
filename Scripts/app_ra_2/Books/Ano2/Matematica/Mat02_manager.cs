using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mat02_manager : BaseBookManager, IClickInteraction
{
    private GameObject GirlHeight;
    private List<GameObject> _mattresses;
    private List<GameObject> _blueTennisKids;

    public override void StartBook()
    {
        base.StartBook();

        foreach (Transform element in transform.GetComponentsInChildren<Transform>())
        {
            if (element.name == ("Garota_Altura"))
                GirlHeight = element.gameObject;
        }
    }

    protected override void InitializeVariables()
    {
        base.InitializeVariables();
        _mattresses = new List<GameObject>();
        _blueTennisKids = new List<GameObject>();
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

        ClearMission(mission, !IsFinished());
        Invoke("ResetAnim", 2);
    }

    public void ClickInteraction(GameObject obj)
    {
        if (_isAnimating)
            return;
        
        Animator objAnim = obj.GetComponent<Animator>();
        string objName = obj.name;

        // Se o objeto nao estiver sendo animado
        if (objAnim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            if (objName.Contains("Azul") && activityUnlocked[0] )
            {
                _isAnimating = true;
                objAnim.SetTrigger("Play");
                obj.GetComponent<AudioSource>().Play();

                GameObject[] auxArray = _blueTennisKids.ToArray();
                if (!IsInArray(obj, auxArray))
                {
                    _blueTennisKids.Add(obj);
                }
                Debug.Log("count == " + _blueTennisKids.Count);

                if (_blueTennisKids.Count >= 3)
                    ClearMission(1, !IsFinished());
                Invoke("ResetAnim", 2);
            }
            else if (obj == GirlHeight && activityUnlocked[1] )
            {
                AnimateObj(obj, objAnim, 2);
            }
            else  if (objName.Contains("Tapete") && activityUnlocked[2] )
            {
                _isAnimating = true;
                objAnim.SetTrigger("Play");
                obj.GetComponent<AudioSource>().Play();

                GameObject[] auxArray = _mattresses.ToArray();
                if (!IsInArray(obj, auxArray))
                {
                    _mattresses.Add(obj);
                }

                if (_mattresses.Count >= 3)
                    ClearMission(3, !IsFinished());
                Invoke("ResetAnim", 2);
            }
        }
    }
    
    protected override void ResetProgress()
    {
        base.ResetProgress();
        
        _mattresses = new List<GameObject>();
        _blueTennisKids = new List<GameObject>();
    }
}
