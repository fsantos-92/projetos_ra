using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;

namespace App.Game
{

    public class GameScreen : MonoBehaviour
    {
        [SerializeField]
        VuforiaBehaviour ARCamera;
        public GameObject InGame;
        [SerializeField]
        UnityEngine.UI.Button BtnPause;
        [SerializeField]
        Button BtnShowTip;
        [SerializeField]
        Button BtnCloseTip;

        [SerializeField]
        Button BtnPlayTip;

        [SerializeField]
        UnityEngine.UI.Image CurrentCard;
        [SerializeField]
        Text TxtYear;

        [SerializeField]
        GameObject TipWindow;

        [SerializeField]
        Text TipDescription;

        [SerializeField]
        GameObject QuestionScreen;
        bool IsPaused = false;

        [SerializeField]
        AudioClip ClipButton;
        [SerializeField]
        GameObject ScreenConfig;

        [SerializeField]
        Button BtnToQuestion;

        [SerializeField]
        GameObject InGameScreen;

        [SerializeField]
        GameObject TxtTimeOut;

        [SerializeField]
        GameObject ImgLogo;

        [Header("Skip Question")]
        [SerializeField]
        GameObject ScreenSkipQuestion;
        [SerializeField]
        Button BtnSkipQuestion;
        [SerializeField]
        Button BtnCancelSkip;
        [SerializeField]
        Button BtnShowSkipScreen;

        bool FirstEnable = false;

        float GameTime = 0;

        // Start is called before the first frame update
        void Awake()
        {
            this.BtnPause.onClick.AddListener(BtnPauseClick);
            this.BtnShowTip.onClick.AddListener(BtnShowTipClick);
            this.BtnCloseTip.onClick.AddListener(BtnShowTipClick);
            this.BtnPlayTip.onClick.AddListener(BtnPlayTipClick);
            this.BtnToQuestion.onClick.AddListener(ObjectClicked);
            this.BtnShowSkipScreen.onClick.AddListener(BtnShowSkipScreenClick);
            this.BtnCancelSkip.onClick.AddListener(BtnCancelSkipClick);
            this.BtnSkipQuestion.onClick.AddListener(BtnSkipQuestionClick);
        }

        private void BtnSkipQuestionClick()
        {
            this.InGameScreen.GetComponent<InGame>().SkipCard();
            this.ObjectClicked();
        }

        private void BtnCancelSkipClick()
        {
            this.ScreenSkipQuestion.SetActive(false);
        }

        private void BtnShowSkipScreenClick()
        {
            this.ScreenSkipQuestion.SetActive(true);
        }

        private void BtnPlayTipClick()
        {
            // audioSource.PlayOneShot(ClipButton);
            SoundManager.Instance.PlayClip(ClipButton);
            InGame.GetComponent<InGame>().PlayCardTip();
        }

        private void BtnShowTipClick()
        {
            // audioSource.PlayOneShot(ClipButton);
            SoundManager.Instance.PlayClip(ClipButton);
            this.TipWindow.SetActive(!this.TipWindow.activeInHierarchy);
            this.ImgLogo.SetActive(!this.ImgLogo.activeInHierarchy);
        }

        private void BtnPauseClick()
        {
            // audioSource.PlayOneShot(ClipButton);
            SoundManager.Instance.PlayClip(ClipButton);
            this.IsPaused = !this.IsPaused;
            this.ScreenConfig.SetActive(true);
        }

        void OnEnable()
        {
            if(!FirstEnable)
            {
                FirstEnable = true;
                Firebase.Analytics.FirebaseAnalytics.LogEvent("Tela_Atingida", "Tela","Tela_de_Jogo");
            }
            #if UNITY_IOS
            CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
            #endif
            this.ResetScreen();
        }

        void ResetScreen()
        {
            this.CurrentCard.GetComponent<RectTransform>().localScale = new Vector3(1.8619f, 1.8619f, 1f);
            CurrentCard.rectTransform.anchoredPosition = new Vector2(0, -201);
            CurrentCard.rectTransform.localRotation = Quaternion.identity;


            LeanTween.move(CurrentCard.rectTransform, new Vector3(360, -768, 0), 0.3f).setDelay(1);
            LeanTween.rotate(CurrentCard.gameObject, new Vector3(0, 0, -8), 0.3f).setDelay(1);
            LeanTween.scale(CurrentCard.rectTransform, Vector3.one, 0.3f).setDelay(1);


            this.GameTime = 0;
            if (this.InGame.GetComponent<InGame>().HasCardTip())
                this.BtnPlayTip.gameObject.SetActive(true);
            else
                this.BtnPlayTip.gameObject.SetActive(false);
            this.ImgLogo.SetActive(false);
            TxtTimeOut.SetActive(false);
            this.IsPaused = false;
            this.BtnToQuestion.gameObject.SetActive(false);
            this.GetComponent<CanvasGroup>().alpha = 1;
            this.CurrentCard.sprite = this.InGame.GetComponent<InGame>().GetCardSprite();
            this.TxtYear.color = this.InGame.GetComponent<InGame>().GetCardColor();
            this.TxtYear.text = PlayerPrefs.GetInt("Year") + "º";
            this.TipDescription.text = this.InGame.GetComponent<InGame>().GetCardDescription();
            this.TipWindow.SetActive(false);
            this.ScreenSkipQuestion.SetActive(false);
            this.BtnShowSkipScreen.gameObject.SetActive(false);
            this.ARCamera.enabled = true;
        }

        void OnDisable()
        {
            if (!ARCamera)
                return;

            if (ARCamera.enabled)
                ARCamera.enabled = false;
        }

        // Update is called once per frame
        void Update()
        {

            if (!this.ScreenConfig.activeInHierarchy)
                this.GameTime += Time.deltaTime;

            if (this.GameTime > 60f)
            {
                this.BtnShowSkipScreen.gameObject.SetActive(true);
            }
            #if UNITY_ANDROID
            if (Input.GetMouseButtonDown(0))
            {
                CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_TRIGGERAUTO);
            }
            #endif
        }

        public void ObjectClicked()
        {
            this.IsPaused = true;
            StartCoroutine(HideCurrentScreen());
            this.QuestionScreen.SetActive(true);
        }
        IEnumerator HideCurrentScreen()
        {
            yield return new WaitForSeconds(1f);
            for (; Mathf.Abs(this.gameObject.GetComponent<CanvasGroup>().alpha) > 0;)
            {
                this.gameObject.GetComponent<CanvasGroup>().alpha = Mathf.MoveTowards(this.gameObject.GetComponent<CanvasGroup>().alpha, 0, Time.deltaTime * 2);
                yield return null;
            }
            this.gameObject.SetActive(false);
        }
    }

}