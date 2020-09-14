using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;

namespace App.Screens
{
    public class MenuScreen : BaseScreen
    {
        [SerializeField] VuforiaBehaviour ARCamera;
        [SerializeField] Button BtnStart;
        [SerializeField] Button BtnAbout;
        [SerializeField] Button BtnCloseAbout;
        [Header( "Screens" )] [SerializeField] GameObject ScreenGame;
        [SerializeField] RectTransform screenAbout;
        [SerializeField] RectTransform ImgFtdLogo;
        public RectTransform FtdLogoPosOriginal;
        public RectTransform FtdLogoPosSide;
        public AudioClip SfxButton;
        AudioSource audioSource;

        private List<GameObject> _managers = new List<GameObject>();

        // Start is called before the first frame update
        void Start()
        {
            // this.ARCamera.enabled = false;
            audioSource = GetComponent<AudioSource>();
            ImgFtdLogo.GetComponent<RectTransform>().transform.position =
                FtdLogoPosOriginal.GetComponent<RectTransform>().transform.position;
            BtnStart.onClick.AddListener( BtnStartClick );
            
            BtnAbout.onClick.AddListener( OpenAboutScreen );
            BtnCloseAbout.onClick.AddListener( CloseAboutScreen );
        }

        void OnEnable()
        {
            SetButtonsInteractions( true );
        }

        private void BtnStartClick()
        {
            ScreenGame.SetActive( true );
            ScreenGame.SendMessage( "StartGame" );
            ScreenGame.GetComponent<GameScreen>().StopCoroutines();
            ScreenGame.GetComponent<GameScreen>().SetButtonsInteractions( true );

            LeanTween.moveX(gameObject.GetComponent<RectTransform>(), - 1920, 1.2f).setDelay(0.2f).setEase(LeanTweenType.easeOutCirc);
            LeanTween.moveX(ScreenGame.GetComponent<RectTransform>(), 0, 1.2f).setDelay(0.2f).setEase(LeanTweenType.easeOutCirc);
            LeanTween.move(ImgFtdLogo, FtdLogoPosSide.localPosition, 1.2f).setDelay(0.2f).setEase(LeanTweenType.easeOutCirc);
            LeanTween.scale(ImgFtdLogo.GetComponent<RectTransform>(), Vector3.one * 0.3f, 1.2f).setDelay(0.2f).setEase(LeanTweenType.easeOutCirc);

            SetButtonsInteractions( false );
            audioSource.PlayOneShot( SfxButton );
            Invoke( "setCameraActive", 1 );
            
            screenAbout.gameObject.SetActive( false );
        }

        void resetManagers()
        {
            GameObject[] rootGameObjs = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
            foreach ( GameObject element in rootGameObjs )
            {
                if ( element.name.Contains( "ImageTarget" ) )
                {
                    Debug.Log( element.transform.GetChild( 0 ).transform.GetChild( 0 ).name );
                }
            }
        }

        private void OpenAboutScreen()
        {
            screenAbout.gameObject.SetActive( true );
            StopCoroutines();
            LeanTween.moveY(screenAbout.GetComponent<RectTransform>(), 0, 0.4f).setDelay(0.1f);
        }
        private void CloseAboutScreen()
        {
            StopCoroutines();
            LeanTween.moveY(screenAbout.GetComponent<RectTransform>(), -1080 -400, 0.4f).setDelay(0.1f);
        }

        void setCameraActive()
        {
            CameraDevice.Instance.SetFocusMode(
                CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO );
            // this.ARCamera.enabled = !this.ARCamera.isActiveAndEnabled;
        }
    }
}