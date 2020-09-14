using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;
using Image = UnityEngine.UI.Image;
using App.YearModule;

namespace App.Game
{

    public class ConfigScreen : MonoBehaviour
    {
        [SerializeField]
        Button BtnConfig;
        [SerializeField]
        Button BtnMute;
        [SerializeField]
        Sprite SpriteBtnMute;
        [SerializeField]
        Sprite SpriteBtnUnmute;

        [SerializeField]
        GameObject[] ScreensToHide;
        [SerializeField]
        GameObject ScreenPlay;
        [SerializeField]
        Button BtnQuit;
        [SerializeField]
        Button BtnBack;

        [SerializeField]
        Button BtnConfirmQuit;
        [SerializeField]
        Button BtnCancelQuit;
        [SerializeField]
        GameObject ScreenConfirmQuit;

        [SerializeField]
        GameObject ScreenCredits;
        [SerializeField]
        Button BtnCredits;
        [SerializeField]
        Button btnCartas;

        [SerializeField]
        InGame inGame;

        CanvasGroup canvasGroup;

        bool IsMuted = false;
        // Start is called before the first frame update
        void Awake()
        {
            this.BtnConfig.onClick.AddListener(BtnConfigClick);
            this.BtnMute.onClick.AddListener(BtnMuteClick);
            this.BtnConfirmQuit.onClick.AddListener(BtnConfirmQuitClick);
            this.BtnBack.onClick.AddListener(BtnConfigClick);
            this.BtnQuit.onClick.AddListener(BtnQuitClick);
            this.BtnCancelQuit.onClick.AddListener(BtnCancelQuitClick);
            this.BtnCredits.onClick.AddListener(BtnCreditsClick);
            btnCartas.onClick.AddListener(BtnCardsClick);


            this.BtnMute.GetComponent<Image>().sprite = SpriteBtnMute;

            canvasGroup = GetComponent<CanvasGroup>();
            canvasGroup.alpha = 0;

#if !UNITY_IOS
            btnCartas.gameObject.SetActive(false);
#endif
        }

        private void BtnCreditsClick()
        {
            this.ScreenCredits.SetActive(true);
        }

        private void BtnCancelQuitClick()
        {
            this.ScreenConfirmQuit.SetActive(false);
        }

        private void BtnQuitClick()
        {
            this.ScreenConfirmQuit.SetActive(true);
        }

        private void BtnConfirmQuitClick()
        {
            StartCoroutine(QuitGame());
        }

        IEnumerator QuitGame()
        {

            foreach (GameObject element in ScreensToHide)
            {
                element.SetActive(false);
                yield return new WaitForEndOfFrame();
                yield return new WaitForEndOfFrame();
                yield return new WaitForEndOfFrame();
            }

            this.ScreenPlay.SetActive(true);

            HideCurrentScreen();

            yield return null;

        }

        void OnEnable()
        {
            if (!canvasGroup)
                canvasGroup = GetComponent<CanvasGroup>();
            this.ScreenConfirmQuit.SetActive(false);
            this.ScreenCredits.SetActive(false);
            BtnQuit.gameObject.SetActive(inGame.gameObject.activeSelf);
            LeanTween.alphaCanvas(canvasGroup, 1, 0.3f);
        }

        private void BtnMuteClick()
        {
            this.IsMuted = !this.IsMuted;
            SoundManager.Instance.SetSound(this.IsMuted);
            if (this.IsMuted)
            {
                this.BtnMute.GetComponent<Image>().sprite = this.SpriteBtnUnmute;
                PlayerPrefs.SetInt("IsMuted", 1);
            }
            else
            {
                this.BtnMute.GetComponent<Image>().sprite = this.SpriteBtnMute;
                PlayerPrefs.SetInt("IsMuted", 0);
            }
        }

        private void BtnConfigClick()
        {
            HideCurrentScreen();
        }

        void HideCurrentScreen()
        {
            LeanTween.alphaCanvas(canvasGroup, 0, 0.3f).setOnComplete(()=> gameObject.SetActive(false));
        }

        void BtnCardsClick()
        {
            Application.OpenURL(HowToPlay.CardsURL);
        }
    }
}
