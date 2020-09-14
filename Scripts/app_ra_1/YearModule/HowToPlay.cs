using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

namespace App.YearModule
{
    public class HowToPlay : MonoBehaviour
    {
        public GameObject playScreen;
        public GameObject welcomeScreen;

        [SerializeField]
        HorizontalScrollSnap horizontalScrollSnap;

        [SerializeField]
        Button BtnStart, BtnBack, btnOpenCardsURL;

        [SerializeField]
        GameObject howToPlayTextAndroid, howToPlayTextIOS;

        [SerializeField]
        AudioClip ClipButton;
        bool FirstEnable;

        CanvasGroup canvas;

        public const string CardsURL = "https://www.ftd.com.br/caca-aos-sentidos/marcadores";

        private void Awake()
        {
            
            this.BtnStart.onClick.AddListener(BtnStartClick);
            this.BtnBack.onClick.AddListener(BtnBackClick);
#if UNITY_IOS
            btnOpenCardsURL.onClick.AddListener(BtnOpenURLClick);
            howToPlayTextIOS.SetActive(true);
            howToPlayTextAndroid.SetActive(false);

            howToPlayTextIOS.GetComponent<Text>().text += CardsURL;

#else
            
            howToPlayTextIOS.SetActive(false);
            howToPlayTextAndroid.SetActive(true);
            
#endif

        }

        private void BtnBackClick()
        {

            SoundManager.Instance.PlayClip(ClipButton);

            LeanTween.alphaCanvas(canvas, 0, 0.25f).setOnComplete(()=> {

                this.playScreen.SetActive(true);
                gameObject.SetActive(false);
                horizontalScrollSnap.GoToScreen(0);
            });
            
            BgManager.Instance.SetAllIconsWhiteColor(false);
        }

        void OnEnable()
        {
            if(!FirstEnable)
            {
                FirstEnable = true;
                Firebase.Analytics.FirebaseAnalytics.LogEvent("Tela_Atingida", "Tela","Como_Jogar");
            }
            if(!canvas)
                canvas = GetComponent<CanvasGroup>();

            LeanTween.alphaCanvas(canvas, 1, 0.3f);
            
            BgManager.Instance.SetIconsCanvasAlpha(0.1f);
        }

        void OnDisable()
        {
            canvas.alpha = 0;
        }

        private void BtnStartClick()
        {   
            // audioSource.PlayOneShot(ClipButton);
            SoundManager.Instance.PlayClip(ClipButton);

            LeanTween.alphaCanvas(canvas, 0, 0.25f).setOnComplete(() => {

                this.welcomeScreen.SetActive(true);
                gameObject.SetActive(false);
                horizontalScrollSnap.GoToScreen(0);
            });
        }

        void BtnOpenURLClick()
        {
            Application.OpenURL(CardsURL.Trim());
        }
    }
}
