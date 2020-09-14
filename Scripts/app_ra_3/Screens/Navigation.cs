using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

namespace App.Screens
{
    public class Navigation : MonoBehaviour
    {
        
        [Header("Buttons")]
        [SerializeField]
        Button BtnClose;

        [SerializeField]
        HorizontalScrollSnap horizontalScrollSnap;

        CanvasGroup canvasGroup;

        void Awake()
        {
            BtnClose.onClick.AddListener(BtnCloseClick);
            canvasGroup = GetComponent<CanvasGroup>();
            canvasGroup.alpha = 0;
        }

        void OnEnable()
        {
            if (!canvasGroup)
                canvasGroup = GetComponent<CanvasGroup>();

            LeanTween.alphaCanvas(canvasGroup, 1, 0.2f).setOnComplete(()=> canvasGroup.blocksRaycasts = true);
        }

        private void BtnCloseClick()
        {
            var cg = GetComponent<CanvasGroup>();
            cg.blocksRaycasts = false;
            LeanTween.alphaCanvas(canvasGroup, 0, 0.2f).setOnComplete(()=> {

                horizontalScrollSnap.ChangePage(0);

                canvasGroup.blocksRaycasts = false;

                gameObject.SetActive(false);

            });
            
        }
    }
}
