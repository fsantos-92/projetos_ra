using UnityEngine;
using UnityEngine.UI;

namespace App.Screens
{
    public class ScreenImageContent : BaseScreen
    {
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
        [Header("Content Title")]
        [SerializeField]
        GameObject txtTitle;
        [Header("3d Content")]
        [SerializeField]
        GameObject ModelParent;
        [SerializeField]
        GameObject ModelCamera;
        [SerializeField]
        GameObject ARCamera;
        //
        void Awake()
        {
            this.resetScreen();
            this.BindButtons();
        }
        protected override void OnEnable()
        {
            base.OnEnable();
            this.resetScreen();
        }
        void OnDisable()
        {
            if (this.ModelParent)
            {
                foreach (Transform element in ModelParent.transform)
                {
                    Debug.Log("Name: " + element.name);
                    Destroy(element.gameObject);
                }
                // this.ARCamera.SetActive(true);
                // this.ModelCamera.SetActive(false);
            }
        }
        //
        private void resetScreen()
        {
            this.SideMenu.GetComponent<RectTransform>().anchoredPosition = new Vector2(-this.SideMenu.GetComponent<RectTransform>().sizeDelta.x, 0);
            this.BtnCloseMenu.gameObject.SetActive(false);
            //
            if (this.ModelParent)
            {
                // this.ModelCamera.SetActive(true);
                // this.ARCamera.SetActive(false);
                if (this.ModelParent.GetComponent<Animation>())
                {
                    this.ModelParent.GetComponent<Animation>().enabled = false;
                }
                Vector3 pos = Vector3.zero;
                Vector3 scale = Vector3.one;
                this.ModelParent.transform.localScale = scale;
                this.ModelParent.transform.localPosition = pos;
            }
        }
        //
        void BindButtons()
        {
            this.BtnMenu.onClick.AddListener(BtnMenuClick);
            this.BtnCloseMenu.onClick.AddListener(BtnCloseMenuClick);
        }

        private void BtnCloseMenuClick()
        {
            this.BtnCloseMenu.gameObject.SetActive(false);
            this.AnimMenu(-1);
        }

        private void BtnMenuClick()
        {
            this.BtnCloseMenu.gameObject.SetActive(true);
            this.AnimMenu(1);
        }
        void AnimMenu(int sign)
        {
            RectTransform rect = this.SideMenu.GetComponent<RectTransform>();
            Vector2 endPos = new Vector2(sign > 0 ? 0 : -rect.sizeDelta.x, 0);
            AnimPos(rect, endPos);
            // StartCoroutine(AnimatePosition(rect, startPos, endPos));
        }
    }
}