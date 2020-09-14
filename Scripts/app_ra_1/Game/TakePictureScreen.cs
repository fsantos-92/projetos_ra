using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Vuforia;
using UnityEngine.UI;
#if UNITY_ANDROID
using UnityEngine.Android;
#elif UNITY_IOS
using UnityEngine.iOS;
#endif
using System;

namespace App.Game
{

    public class TakePictureScreen : MonoBehaviour
    {
        [SerializeField]
        VuforiaBehaviour ARCamera;
        [SerializeField]
        Button BtnTakePicture;
        [SerializeField]
        Button BtnBack;

        Texture2D m_Texture;

        Sprite Screenshot;

        int screenshotCount = 0;
        [SerializeField]
        GameObject PictureTakenScreen;
        [SerializeField]
        GameObject ImgHeader;
        [SerializeField]
        GameObject CongratsScreen;
        [SerializeField]
        AudioClip ClipButton;

        [SerializeField]
        Button BtnSwitchCamera;
        [SerializeField]
        GameObject frontCameraController;

        bool IsFrontCamera = false;

        //WebCamTexture CameraTexture;

        [SerializeField]
        UnityEngine.UI.Image ImgPicture;
        [SerializeField]
        UnityEngine.UI.Image ImgBorder;

        [SerializeField]
        GameObject CameraPosStart;

        [SerializeField]
        GameObject CameraPosEnd;

        [SerializeField]
        GameObject ImgBorderAlpha;

        bool FirstEnable = false;

        string FilePath;
        // Start is called before the first frame update
        void Awake()
        {
            float height = Screen.height;
            float width = (height / 4) * 3;
            
            this.BtnTakePicture.onClick.AddListener(BtnTakePictureClick);
            this.BtnBack.onClick.AddListener(BtnBackClick);
            this.BtnSwitchCamera.onClick.AddListener(BtnSwitchCameraClick);
            m_Texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, true);
        }

        void Update()
        {
#if UNITY_ANDROID
            if (Input.GetMouseButtonDown(0) && !IsFrontCamera)
            {
                CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_TRIGGERAUTO);
            }
#endif
        }

        private void BtnSwitchCameraClick()
        {
            this.IsFrontCamera = !this.IsFrontCamera;

            if (IsFrontCamera)
            {
                ARCamera.enabled = false;
                frontCameraController.SetActive(true);
            }
            else
            {
                frontCameraController.SetActive(false);
                ARCamera.enabled = true;
            }

        }


        bool CamPermission()
        {
#if UNITY_ANDROID
            if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
            {
                Permission.RequestUserPermission(Permission.Camera);
                return false;
            }
            else return true;
#elif UNITY_IOS
            if (!Application.HasUserAuthorization(UserAuthorization.WebCam))
            {
                Application.RequestUserAuthorization(UserAuthorization.WebCam);
                return false;
            }
            else return true;
#endif
            return true;
        }

        private void BtnBackClick()
        {
            IsFrontCamera = false;
            frontCameraController.SetActive(false);
            SoundManager.Instance.PlayClip(ClipButton);
            HideCurrentScreen();
            this.CongratsScreen.SetActive(true);
            Destroy(this.m_Texture);
        }

        private void BtnTakePictureClick()
        {
            SoundManager.Instance.PlayClip(ClipButton);
            StartCoroutine(TakePicture());
        }

        IEnumerator TakePicture()
        {
            float startPosX = this.CameraPosStart.GetComponent<RectTransform>().transform.position.x;
            float startPosY = this.CameraPosStart.GetComponent<RectTransform>().transform.position.y;
            float endPosX = this.CameraPosEnd.GetComponent<RectTransform>().transform.position.x;
            float endPosY = this.CameraPosEnd.GetComponent<RectTransform>().transform.position.y;
            float width = endPosX - startPosX;
            float height = endPosY - startPosY;
            this.ImgHeader.SetActive(false);
            this.BtnTakePicture.gameObject.SetActive(false);
            this.BtnSwitchCamera.gameObject.SetActive(false);
            this.ImgBorderAlpha.gameObject.SetActive(false);
            yield return new WaitForEndOfFrame();
            Texture2D texture;
            //Create a new texture with the width and height of the screen
            texture = new Texture2D((int)width, (int)height, TextureFormat.RGB24, false);
            //Read the pixels in the Rect starting at 0,0 and ending at the screen's width and height
            texture.ReadPixels(new Rect(startPosX, startPosY, width, height), 0, 0, false);
            texture.Apply();

            this.ImgPicture.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100f);

            float WidthToShow = this.CameraPosEnd.GetComponent<RectTransform>().transform.position.x - this.CameraPosStart.GetComponent<RectTransform>().transform.position.x;
            float heightToShow = this.CameraPosEnd.GetComponent<RectTransform>().transform.position.y - this.CameraPosStart.GetComponent<RectTransform>().transform.position.y;
            this.ImgPicture.preserveAspect = true;
            this.ImgBorder.gameObject.SetActive(true);

            if (IsFrontCamera)
            {
                frontCameraController.SetActive(false);
                IsFrontCamera = false;
            }
            int PosX = 0;
            int PosY = (int)((Screen.height * 0.5f) - (Screen.width * 0.5f));
            Debug.Log(PosY);
            int picWidth = Screen.width;
            yield return new WaitForEndOfFrame();
            this.m_Texture = new Texture2D(picWidth, picWidth, TextureFormat.RGB24, false);
            this.m_Texture.ReadPixels(new Rect(PosX, PosY, picWidth, picWidth), 0, 0, false);
            this.m_Texture.Apply();

            byte[] bytes = m_Texture.EncodeToJPG();
            string fileIndex = "SavedScreen" + screenshotCount + ".jpg";
            var fileName = Path.Combine(Application.persistentDataPath, fileIndex);
            this.FilePath = fileName;
            File.WriteAllBytes(fileName, bytes);

            this.Screenshot = Sprite.Create(m_Texture, new Rect(0, 0, m_Texture.width, m_Texture.height), new Vector2(0.5f, 0.5f), 100f);

            screenshotCount++;

            HideCurrentScreen();
            this.PictureTakenScreen.SetActive(true);
            Destroy(texture);
        }

        public string GetFilePath()
        {
            return this.FilePath;
        }

        public Sprite GetPicture()
        {
            return Screenshot;
        }
        void OnEnable()
        {
            if (!FirstEnable)
            {
                FirstEnable = true;
                Firebase.Analytics.FirebaseAnalytics.LogEvent("Tela_Atingida", "Tela", "Tirar_Foto");
            }

            this.ImgBorder.gameObject.SetActive(false);

            this.ImgBorderAlpha.gameObject.SetActive(true);
            this.IsFrontCamera = false;
            this.ARCamera.enabled = true;
            this.ImgHeader.SetActive(true);
            LeanTween.alphaCanvas(GetComponent<CanvasGroup>(), 1, .3f);
            this.BtnTakePicture.gameObject.SetActive(true);
            this.BtnSwitchCamera.gameObject.SetActive(true);

            BgManager.Instance.SetBgTransparent(true);

#if UNITY_IOS
            CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
            //this.FrontCameraRender.GetComponent<RectTransform>().transform.localRotation = new Quaternion(0, 0, 0, 1);
#endif
        }
        void OnDisable()
        {
            if (ARCamera.enabled)
                ARCamera.enabled = false;

            IsFrontCamera = false;
        }
        void HideCurrentScreen()
        {

            LeanTween.alphaCanvas(this.gameObject.GetComponent<CanvasGroup>(), 0, 0.3f).setOnComplete(
                () => gameObject.SetActive(false));
        }

    }
}
