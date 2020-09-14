using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace App.YearModule
{

    public class WelcomeScreen : MonoBehaviour
    {
        public Text txtTitle;
        public Button btnBack;
        public Button btnNext;
        public GameObject nextScreen;
        public GameObject previousScreen;

        [SerializeField]
        AudioClip ClipButton;

        CanvasGroup canvas;

        bool FirstEnable;

        // Start is called before the first frame update
        void Start()
        {
            this.btnBack.onClick.AddListener(OnBackButtonClick);
            this.btnNext.onClick.AddListener(OnNextButtonClick);
        }

        void OnEnable()
        {
            if(!FirstEnable)
            {
                FirstEnable = true;
                Firebase.Analytics.FirebaseAnalytics.LogEvent("Tela_Atingida", "Tela","Tela_de_Bem_Vindo");
            }
			canvas = GetComponent<CanvasGroup>();
			canvas.alpha = 0;

            LeanTween.alphaCanvas(canvas, 1, 0.3f);
            BgManager.Instance.SetBgColor(BgColorsEnum.WHITE);
            BgManager.Instance.SetIconsCanvasAlpha(0.1f);
        }
        
        void OnBackButtonClick()
        {
            SoundManager.Instance.PlayClip(ClipButton);
            HideCurrentScreen(previousScreen);
        }
        void OnNextButtonClick()
        {
            SoundManager.Instance.PlayClip(ClipButton);
            HideCurrentScreen(nextScreen);
        }

        void HideCurrentScreen(GameObject screenToActivate)
        {

            LeanTween.alphaCanvas(canvas, 0, 0.3f).setOnComplete(()=> {
                this.gameObject.SetActive(false);
                if(screenToActivate)
                    screenToActivate.SetActive(true);
            });

        }

    }
}
