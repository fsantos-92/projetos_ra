using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnClose : MonoBehaviour
{
    [SerializeField]
    Button CloseBtn;
    // Start is called before the first frame update
    void Start()
    {
        CloseBtn.onClick.AddListener(BtnCloseClick);
    }

    private void BtnCloseClick()
    {
        this.gameObject.SetActive(false);
    }
}
