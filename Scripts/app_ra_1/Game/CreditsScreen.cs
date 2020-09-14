using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditsScreen : MonoBehaviour
{
    [SerializeField]
    Button BtnHistoryCredits;
    [SerializeField]
    Button BtnScienceCredits;
    [SerializeField]
    Button BtnGeographyCredits;

    [SerializeField]
    GameObject ScreenHistoryCredits;
    [SerializeField]
    GameObject ScreenScienceCredits;
    [SerializeField]
    GameObject ScreenGeographyCredits;

    [SerializeField]
    Button BtnBack;
    [SerializeField]
    Button BtnQuit;

    [SerializeField]
    GameObject ConfigScreen;
    // Start is called before the first frame update
    void Start()
    {
        this.BtnHistoryCredits.onClick.AddListener(BtnHistoryClick);
        this.BtnScienceCredits.onClick.AddListener(BtnScienceClick);
        this.BtnGeographyCredits.onClick.AddListener(BtnGeographyClick);
        this.BtnBack.onClick.AddListener(BtnBackClick);
    }

    private void BtnBackClick()
    {
        if (ScreenHistoryCredits.activeSelf)
            ScreenHistoryCredits.SetActive(false);
        else
            if (ScreenScienceCredits.activeSelf)
                ScreenScienceCredits.SetActive(false);
        else
            if (ScreenGeographyCredits.activeSelf)
                ScreenGeographyCredits.SetActive(false);
        else
            gameObject.SetActive(false);

    }

    private void BtnGeographyClick()
    {
        // this.Mask.SetActive(true);
        this.ScreenGeographyCredits.SetActive(true);
    }

    private void BtnScienceClick()
    {
        // this.Mask.SetActive(true);
        this.ScreenScienceCredits.SetActive(true);
    }

    private void BtnHistoryClick()
    {
        // this.Mask.SetActive(true);
        this.ScreenHistoryCredits.SetActive(true);
    }

    void OnEnable()
    {
        // this.Mask.SetActive(false);
        this.ScreenGeographyCredits.SetActive(false);
        this.ScreenHistoryCredits.SetActive(false);
        this.ScreenScienceCredits.SetActive(false);
    }

    void OnDisable()
    {
        // this.Mask.SetActive(false);
        this.ScreenGeographyCredits.SetActive(false);
        this.ScreenHistoryCredits.SetActive(false);
        this.ScreenScienceCredits.SetActive(false);
    }
}
