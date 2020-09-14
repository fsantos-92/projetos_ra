using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnOpen : MonoBehaviour
{
    [SerializeField]
    Button OpenBtn;
    [SerializeField]
    GameObject ScreenToOpen;

    void Start()
    {
        OpenBtn.onClick.AddListener(BtnOpenClick);
    }

    private void BtnOpenClick()
    {
        this.ScreenToOpen.SetActive(true);
    }
}
