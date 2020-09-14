using UnityEngine;
using UnityEngine.UI;
using Vuforia;
using TMPro;

namespace App.Screens
{
    public class MainScreen : BaseScreen
    {
        [Header("AR Camera")]
        [SerializeField]
        VuforiaBehaviour ARCamera;
        //
        [Header("Buttons")]
        [SerializeField]
        Button BtnMenu;
        [SerializeField]
        Button BtnCloseMenu;
        //
        [Header("SideMenu")]
        [SerializeField]
        GameObject SideMenu;
        [SerializeField]
        SimpleCloudHandler CloudHandler;
        [Header("SubScreens")]
        [SerializeField]
        CanvasGroup mainGroup;
        [SerializeField]
        CanvasGroup canvasSliderDownloadProgress;
        [SerializeField]
        CanvasGroup canvasDownloading;
        [SerializeField]
        CanvasGroup canvasTxtScan;

        [SerializeField]
        RectTransform laserLine;

        CanvasGroup laserLineCanvas;

        [SerializeField]
        TMP_Text text;

        Text t;

        void Awake()
        {
            laserLineCanvas = laserLine.GetComponent<CanvasGroup>();
            this.BindButtons();
            this.ResetScreen();
            LeanTween.alpha(laserLine, 0.3f, 0.1f).setLoopPingPong();
            LeanTween.move(laserLine, new Vector3(0, 393, 0), 0);
            LeanTween.move(laserLine, new Vector3(0, -393, 0), 0.7f).setEase(LeanTweenType.easeInOutCubic).setLoopPingPong();

            //LeanTween.alpha(text.gameObject, 0, 3.5f);
            //LeanTween.value(text.gameObject, a => text.color = a, Color.white, new Color(1,1,1,0), 3.5f).setOnComplete(()=>text.color = Color.white);

            //LeanTween.value(text.gameObject, a => text.text = a.ToString("F0"), 0, 100, 10f);

            //text.color = Color.clear;
            //LeanTween.color(text.gameObject, Color.clear, 3.5f);


        }

        protected override void OnEnable()
        {
            base.OnEnable();
            this.ResetScreen();
            if (this.CloudHandler)
                this.CloudHandler.Reset();
        }
        void OnDisable()
        {
            //if (this.ARCamera)
                //this.ARCamera.enabled = false;
        }

        public override void Disable()
        {
            LeanTween.alphaCanvas(mainGroup, 0, 0.3f);
            mainGroup.blocksRaycasts = false;
            base.Disable();
        }

        void ResetScreen()
        {
            
            this.ARCamera.enabled = true;
            this.BtnCloseMenu.gameObject.SetActive(false);
            this.SideMenu.GetComponent<RectTransform>().anchoredPosition = new Vector2(-this.SideMenu.GetComponent<RectTransform>().sizeDelta.x, this.SideMenu.GetComponent<RectTransform>().anchoredPosition.y);

            LeanTween.alphaCanvas(mainGroup, 1, 0.3f);
            mainGroup.blocksRaycasts = true;
            LeanTween.alphaCanvas(canvasSliderDownloadProgress, 0, 0f);
            LeanTween.cancel(canvasDownloading.gameObject);
            canvasDownloading.alpha = 0;
            LeanTween.alphaCanvas(canvasTxtScan, 1, 0.3f);
            LeanTween.alphaCanvas(laserLineCanvas, 1, 0.3f);

        }
        
        void BindButtons()
        {
            this.BtnMenu.onClick.AddListener(BtnMenuClick);
            this.BtnCloseMenu.onClick.AddListener(BtnCloseMenuClick);
        }

        private void BtnCloseMenuClick()
        {
            this.BtnCloseMenu.gameObject.SetActive(false);
            RectTransform rect = this.SideMenu.GetComponent<RectTransform>();
            Vector2 startPos = rect.position;
            Vector2 endPos = new Vector3(-rect.sizeDelta.x, 0);
            AnimPos(rect, endPos);
        }

        private void BtnMenuClick()
        {
            this.BtnCloseMenu.gameObject.SetActive(true);
            RectTransform rect = this.SideMenu.GetComponent<RectTransform>();
            Vector2 startPos = rect.position;
            Vector2 endPos = new Vector3(0, 0);
            AnimPos(rect, endPos);
        }
        
        public void ShowLoadScreen()
        {

            LeanTween.alphaCanvas(canvasSliderDownloadProgress, 1, 0.3f);
            LeanTween.alphaCanvas(canvasTxtScan, 0, 0.3f);
            canvasDownloading.alpha = 0.3f;
            LeanTween.alphaCanvas(canvasDownloading, 1, 0.3f).setLoopPingPong();
            LeanTween.alphaCanvas(laserLineCanvas, 0, 0.3f);
        }
        public void HideLoadScreen()
        {
            
            LeanTween.alphaCanvas(canvasSliderDownloadProgress, 0, 0.3f);
            LeanTween.alphaCanvas(canvasTxtScan, 1, 0.3f);
            LeanTween.alphaCanvas(laserLineCanvas, 1, 0.3f);
            LeanTween.cancel(canvasDownloading.gameObject);
            LeanTween.alphaCanvas(canvasDownloading, 0, 0.3f);
            if (this.CloudHandler)
                this.CloudHandler.Reset();
        }
        
    }
}
