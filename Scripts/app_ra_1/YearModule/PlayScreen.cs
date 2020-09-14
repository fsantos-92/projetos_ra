using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Vuforia;

namespace App.YearModule
{

    public class PlayScreen : MonoBehaviour
    {
        public GameObject welcomeScreen;
        public GameObject howToPlayScreen;

        [SerializeField]
        Button BtnPlay;
        [SerializeField]
        Button BtnHowToPlay;
        [SerializeField]
        Button BtnConfig;
        [SerializeField]
        GameObject ScreenConfig;

        [SerializeField]
        VuforiaBehaviour ARCamera;

        [SerializeField]
        RectTransform[] elementsBg;

        public AudioClip ClipButton;

        CanvasGroup canvas;

        bool FirstEnable = false;
        // Start is called before the first frame update
        void Awake()
        {
            canvas = GetComponent<CanvasGroup>();
            this.BindButtons();
            canvas.alpha = 0;
        }

        void OnEnable()
        {
            if(!FirstEnable)
            {
                FirstEnable = true;
                Firebase.Analytics.FirebaseAnalytics.LogEvent("Tela_Atingida", "Tela","Tela_de_Jogar");
            }
            BgManager.Instance.SetBgColor(BgColorsEnum.WHITE);
            BgManager.Instance.SetAllIconsWhiteColor(false);
            BgManager.Instance.SetIconsCanvasAlpha(1);
            LeanTween.alphaCanvas(canvas, 1, 0.3f);

            if (ARCamera.enabled)
                ARCamera.enabled = false;
        }

        #region Buttons
        void BindButtons()
        {
            this.BtnPlay.onClick.AddListener(BtnPlayClick);
            this.BtnHowToPlay.onClick.AddListener(BtnHowToPlayClick);
            this.BtnConfig.onClick.AddListener(BtnConfigClick);
        }

        private void BtnConfigClick()
        {
            this.ScreenConfig.SetActive(true);
        }

        private void BtnPlayClick()
        {
            // audioSource.PlayOneShot(ClipButton);
            SoundManager.Instance.PlayClip(ClipButton);
            LeanTween.alphaCanvas(canvas, 0, 0.25f).setOnComplete(()=>
            {
                gameObject.SetActive(false);
                this.welcomeScreen.SetActive(true);

            });
        }
        private void BtnHowToPlayClick()
        {
            // audioSource.PlayOneShot(ClipButton);
            SoundManager.Instance.PlayClip(ClipButton);
            LeanTween.alphaCanvas(canvas, 0, 0.25f).setOnComplete(() =>
            {
                gameObject.SetActive(false);
                this.howToPlayScreen.SetActive(true);
            });


            BgManager.Instance.SetBgColor(BgColorsEnum.WHITE);
            BgManager.Instance.SetAllIconsWhiteColor(false);
        }
        #endregion

    }

}