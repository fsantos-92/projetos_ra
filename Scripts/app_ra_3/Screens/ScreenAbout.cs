using UnityEngine;
using UnityEngine.UI;

namespace App.Screens
{
    public class ScreenAbout : BaseScreen
    {
        [SerializeField]
        Button BtnClose;

        void Awake()
        {
            this.BtnClose.onClick.AddListener(BtnCloseClick);
        }

        private void BtnCloseClick()
        {
            fade(gameObject.GetComponent<CanvasGroup>(), 1, 0, true);
        }
    }
}
