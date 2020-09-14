using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Cards;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
using Vuforia;
#if UNITY_ANDROID
using UnityEngine.Android;
#elif UNITY_IOS
using UnityEngine.iOS;
#endif

namespace App.YearModule
{

    public class HideCards : MonoBehaviour
    {
        public Button btnBack;
        public Button BtnStart;
        public Transform[] boxYearPositions;
        public Transform[] boxModulePositions;


        public GameObject previousScreen;

        [SerializeField]
        CanvasGroup canvasGroupCards;

        public GameObject GameScreen;


        private int CurrentGroupScreen;

        public GameObject[] MiniCards;
        public GameObject[] CardsRecord;

        [SerializeField]
        HorizontalScrollSnap horizontalScrollSnap;

        [SerializeField]
        Button BtnRecordYesOrNo;
        [SerializeField]
        Sprite SpriteBtnYes;
        [SerializeField]
        Sprite SpriteBtnNo;
        bool DoRecord = true;
        [SerializeField]
        CanvasGroup blockRecording;

        [SerializeField]
        Text TxtTitle;

        [SerializeField]
        Text yearAndModule;

        [Header("Colors")]
        [SerializeField]
        Color DarkBlue;
        [SerializeField]
        Color LightBlue;

        [SerializeField]
        AudioClip ClipButton;

        [SerializeField]
        GameObject[] CanvasGroups;
        [SerializeField]
        GameObject ScreenConfig;
        [SerializeField]
        Button BtnConfig;

        CanvasGroup canvasGroup;

        bool FirstEnable = false;
        // Start is called before the first frame update
        void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            btnBack.onClick.AddListener(OnBackButtonClick);
            BtnRecordYesOrNo.onClick.AddListener(BtnYesOrNoClick);
            BtnStart.onClick.AddListener(BtnStartClick);
            BtnConfig.onClick.AddListener(BtnConfigClick);
        }

        private void BtnConfigClick()
        {
            this.ScreenConfig.SetActive(true);
        }

        private void BtnStartClick()
        {
            SoundManager.Instance.PlayClip(ClipButton);

            LeanTween.alphaCanvas(canvasGroup, 0, 0.25f).setOnComplete(() => {
                gameObject.SetActive(false);
                this.GameScreen.SetActive(true);
                horizontalScrollSnap.GoToScreen(0);
            });
        }

        private void BtnYesOrNoClick()
        {
            SoundManager.Instance.PlayClip(ClipButton);
            DoRecord = !DoRecord;
            CardRecord.IsRecordingAllowed = DoRecord;
            if (DoRecord)
            {
                this.BtnRecordYesOrNo.GetComponent<UnityEngine.UI.Image>().sprite = SpriteBtnYes;
                LeanTween.alphaCanvas(blockRecording, 1, 0.3f);
                this.blockRecording.blocksRaycasts = true;
            }
            else
            {
                this.BtnRecordYesOrNo.GetComponent<UnityEngine.UI.Image>().sprite = SpriteBtnNo;

                LeanTween.alphaCanvas(blockRecording, 0.28f, 0.3f);
                this.blockRecording.blocksRaycasts = false;
            }
        }

        void ShowCards()
        {
            LeanTween.alphaCanvas(canvasGroupCards, 1, 0.3f);
        }

        // Update is called once per frame
        void Update()
        {

            canvasGroupCards.blocksRaycasts = canvasGroupCards.alpha >= 1;

        }
        void OnEnable()
        {
            if (!FirstEnable)
            {
                FirstEnable = true;
                Firebase.Analytics.FirebaseAnalytics.LogEvent("Tela_Atingida", "Tela", "Gravar_Dica");
            }

            this.ResetScreen();

            LeanTween.alphaCanvas(canvasGroup, 1, 0.25f).setDelay(0.25f).setOnComplete(()=> {
                ShowCards();
            });

            BgManager.Instance.SetBgColor(BgColorsEnum.WHITE);
            BgManager.Instance.SetIconsCanvasAlpha(0.1f);
            BgManager.Instance.SetAllIconsWhiteColor(false);
        }
        void OnDisable()
        {
            //this.navigation.SetActive(false);
            StopAllCoroutines();
        }
        void ResetScreen()
        {
            //this.navigation.SetActive(true);
            this.gameObject.GetComponent<CanvasGroup>().alpha = 0;
            //this.btnBack.enabled = false;
            canvasGroupCards.alpha = 0;
            yearAndModule.text = PlayerPrefs.GetInt("Year") + "º Ano - " + "Módulo 0" + PlayerPrefs.GetInt("Module");
            this.BtnRecordYesOrNo.GetComponent<UnityEngine.UI.Image>().sprite = SpriteBtnYes;
            this.blockRecording.alpha = 1;
            this.blockRecording.blocksRaycasts = true;
            this.TxtTitle.color = this.DarkBlue;
            this.btnBack.GetComponent<UnityEngine.UI.Image>().color = this.DarkBlue;

            //this.navigation.SetActive(false);
            foreach (GameObject element in this.MiniCards)
            {
                element.GetComponent<CardsMini>().UpdateText();
            }

            foreach (GameObject element in CardsRecord)
            {
                element.GetComponent<CardRecord>().SetAudioTipNull();
            }
        }

        private void OnBackButtonClick()
        {
            // audioSource.PlayOneShot(ClipButton);
            SoundManager.Instance.PlayClip(ClipButton);
            StopAllCoroutines();

            LeanTween.alphaCanvas(canvasGroup, 0, 0.25f).setOnComplete(() => {
                gameObject.SetActive(false);
                this.previousScreen.SetActive(true);
                horizontalScrollSnap.GoToScreen(0);
                foreach (var item in MiniCards)
                {
                    item.GetComponent<CardsMini>().BtnCloseClick();
                }
            });

        }

    }
}
