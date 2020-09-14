using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace App.Game
{

    public class CongratsScreen : MonoBehaviour
    {
        [SerializeField]
        Button BtnRestart;
        [SerializeField]
        Button BtnTakePicture;
        [SerializeField]
        GameObject InGameScreen;
        [SerializeField]
        GameObject PlayScreen;
        [SerializeField]
        GameObject TakePictureScreen;
        [SerializeField]
        AudioClip ClipButton;

        [SerializeField]
        Text TxtCongrats;

        [SerializeField]
        Image ImgCongrats;
        [SerializeField]
        Sprite SpriteTrophy;
        [SerializeField]
        Sprite SpriteLupa;

        bool FirstEnable = false;
        // Start is called before the first frame update
        void Start()
        {
            this.BtnRestart.onClick.AddListener(BtnRestartClick);
            this.BtnTakePicture.onClick.AddListener(BtnTakePictureClick);
        }

        private void BtnTakePictureClick()
        {
            // audioSource.PlayOneShot(ClipButton);
            SoundManager.Instance.PlayClip(ClipButton);
            LeanTween.alphaCanvas(gameObject.GetComponent<CanvasGroup>(), 0, 0.3f).setOnComplete(()=> {
                this.gameObject.SetActive(false);
                this.TakePictureScreen.SetActive(true);
            });
            
        }

        void OnEnable()
        {
            if(!FirstEnable)
            {
                FirstEnable = true;
                Firebase.Analytics.FirebaseAnalytics.LogEvent("Tela_Atingida", "Tela","Tela_de_Parabens");
            }
            this.gameObject.GetComponent<CanvasGroup>().alpha = 1;
            if (this.InGameScreen.GetComponent<InGame>().HasSkipped())
            {
                this.TxtCongrats.text = "O caçador respondeu as 5 perguntas, mas não encontrou todas as cartas.";
                this.ImgCongrats.sprite = this.SpriteLupa;
            }
            else
            {
                this.TxtCongrats.text = "O caçador encontrou as 5 cartas e respondeu as 5 perguntas.";
                this.ImgCongrats.sprite = this.SpriteTrophy;
            }

            BgManager.Instance.SetBgColor(BgColorsEnum.WHITE);
            BgManager.Instance.SetAllIconsWhiteColor(false);
        }

        private void BtnRestartClick()
        {
            SoundManager.Instance.PlayClip(ClipButton);
            this.PlayScreen.SetActive(true);
            InGameScreen.GetComponent<InGame>().FadeThis();
            this.gameObject.SetActive(false);
        }
        
    }
}
