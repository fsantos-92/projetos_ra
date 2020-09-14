using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;

namespace App.Game
{

    public class PictureTakenScreen : MonoBehaviour
    {
        //
        [SerializeField]
        Button BtnBack;
        [SerializeField]
        Image Photo;
        [SerializeField]
        GameObject TakePictureScreen;
        [SerializeField]
        AudioClip ClipButton;

        [Header("Social Media Buttons")]
        [SerializeField]
        Button BtnFacebook;
        [SerializeField]
        Button BtnTwitter;
        [SerializeField]
        Button BtnInstagram;
        [SerializeField]
        Button BtnWhatsApp;

        bool FirstEnable = false;

        // Start is called before the first frame update
        void Start()
        {
            this.BtnBack.onClick.AddListener(BtnBackClick);
            this.BtnFacebook.onClick.AddListener(BtnShareClick);
            this.BtnTwitter.onClick.AddListener(BtnShareClick);
            this.BtnInstagram.onClick.AddListener(BtnShareClick);
#if UNITY_IOS
            this.BtnWhatsApp.onClick.AddListener(ShareWhatsappClickIos);
#else
            this.BtnWhatsApp.onClick.AddListener(BtnShareClick);
#endif
        }

        private void BtnShareClick()
        {
            string ShareText = "#AceleraFTD ";
            var filePath = this.TakePictureScreen.GetComponent<TakePictureScreen>().GetFilePath();
            new NativeShare().AddFile(filePath).SetText(ShareText).Share();
            //
            Firebase.Analytics.FirebaseAnalytics.LogEvent(Firebase.Analytics.FirebaseAnalytics.EventShare);
        }

        private void ShareWhatsappClickIos()
        {
            var filePath = this.TakePictureScreen.GetComponent<TakePictureScreen>().GetFilePath();
            new NativeShare().AddFile(filePath).Share();
            //
            Firebase.Analytics.FirebaseAnalytics.LogEvent(Firebase.Analytics.FirebaseAnalytics.EventShare);
        }

        private void BtnBackClick()
        {
            // audioSource.PlayOneShot(ClipButton);
            SoundManager.Instance.PlayClip(ClipButton);
            HideCurrentScreen();
            this.TakePictureScreen.SetActive(true);
        }
        void OnEnable()
        {
            if (!FirstEnable)
            {
                FirstEnable = true;
                Firebase.Analytics.FirebaseAnalytics.LogEvent("Tela_Atingida", "Tela", "Compartilhar_Foto");
            }
            this.GetComponent<CanvasGroup>().alpha = 1;
            this.Photo.sprite = this.TakePictureScreen.GetComponent<TakePictureScreen>().GetPicture();
            BgManager.Instance.SetAllIconsWhiteColor(true);
            BgManager.Instance.SetBgColor(BgColorsEnum.RED);

            // Texture2D PicTexture = this.TakePictureScreen.GetComponent<TakePictureScreen>().GetPicture();
            // PhotoTexture = Sprite.Create(PicTexture, new Rect(0, 0, PicTexture.width, PicTexture.height), new Vector2(0.5f, 0.5f));
            // this.Photo.sprite = PhotoTexture;
            this.Photo.preserveAspect = true;
        }

        void HideCurrentScreen()
        {
            LeanTween.alphaCanvas(gameObject.GetComponent<CanvasGroup>(), 0, 0.3f).setOnComplete(()=>gameObject.SetActive(false));
        }
    }
}
