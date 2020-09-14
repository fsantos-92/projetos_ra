using UnityEngine;
using UnityEngine.UI;

namespace App.Screens
{
    public class SideMenu : BaseScreen
    {
        [Header("Buttons")]
        [SerializeField]
        Button BtnHelp;
        [SerializeField]
        Button BtnAbout;
        [SerializeField]
        Button BtnClose;
        [SerializeField]
        Button BtnScan;
        [SerializeField]
        Button BtnCloseClickOut;

        [Header("Screens")]
        [SerializeField]
        GameObject ScreenParent;
        [SerializeField]
        GameObject ScreenHelp;
        [SerializeField]
        GameObject ScreenAbout;
        [SerializeField]
        GameObject ScreenMain;
        //
        void Awake()
        {
            this.BindButtons();
        }
        //
        protected override void OnEnable()
        {

        }
        void BindButtons()
        {
            this.BtnClose.onClick.AddListener(BtnCloseClick);
            this.BtnAbout.onClick.AddListener(BtnAboutClick);
            if (this.BtnHelp)
                this.BtnHelp.onClick.AddListener(BtnHelpClick);
            if (this.BtnScan)
                BtnScan.onClick.AddListener(BtnScanClick);
        }

        private void BtnScanClick()
        {
            this.ScreenParent.SendMessage("Disable");
            this.ScreenMain.SetActive(true);
            BtnCloseClick();
        }

        private void BtnAboutClick()
        {
            this.ScreenAbout.SetActive(true);
            BtnCloseClick();
        }

        private void BtnHelpClick()
        {
            this.ScreenHelp.SetActive(true);
            BtnCloseClick();
        }

        private void BtnCloseClick()
        {
            this.BtnCloseClickOut.gameObject.SetActive(false);
            RectTransform rect = GetComponent<RectTransform>();
            Vector2 startPos = rect.position;
            Vector2 endPos = new Vector2(-rect.sizeDelta.x, 0);
            AnimPos(rect, endPos);
        }
    }
}
