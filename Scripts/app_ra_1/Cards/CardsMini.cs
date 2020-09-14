using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace App.Cards
{

    public class CardsMini : MonoBehaviour

    {
        Text[] TxtYear;
        [SerializeField]
        RectTransform cardInfo;

        [SerializeField]
        Button BtnClose;

        Button Btn;

        [SerializeField]
        GameObject navigation;

        CanvasGroup cardInfoCanvas;

        CanvasGroup cardCanvasGroup;

        static readonly Vector2 cardInfoMinimumSize = new Vector2(0.324f, 0.324f);

        // Start is called before the first frame update
        void Start()
        {
            this.Btn = GetComponentInChildren<Button>();
            this.Btn.onClick.AddListener(BtnClick);
            this.BtnClose.onClick.AddListener(BtnCloseClick);

            cardInfo.localScale = cardInfoMinimumSize;
            cardInfo.anchoredPosition = transform.localPosition;
            cardInfoCanvas = cardInfo.GetComponent<CanvasGroup>();
            cardInfoCanvas.alpha = 0;
            cardCanvasGroup = GetComponent<CanvasGroup>();
            cardInfoCanvas.blocksRaycasts = false;

        }

        public void BtnCloseClick()
        {
            LeanTween.scale(cardInfo, cardInfoMinimumSize, 0.15f);
            LeanTween.move(cardInfo, transform.localPosition, 0.15f);
            LeanTween.alphaCanvas(cardInfoCanvas, 0, 0.15f);
            cardInfoCanvas.blocksRaycasts = false;
            LeanTween.alphaCanvas(cardCanvasGroup, 1, 0.15f).setDelay(0.1f);
        }

        private void BtnClick()
        {
            LeanTween.scale(cardInfo, Vector3.one, 0.15f);
            LeanTween.move(cardInfo, Vector3.zero, 0.15f);
            LeanTween.alphaCanvas(cardInfoCanvas, 1, 0.15f);
            cardInfoCanvas.blocksRaycasts = true;
            LeanTween.alphaCanvas(cardCanvasGroup, 0, 0.1f);
        }

        public void UpdateText()
        {
            TxtYear = GetComponentsInChildren<Text>();
            foreach (Text element in TxtYear)
            {
                element.text = PlayerPrefs.GetInt("Year") + "º";
            }
        }
    }
}
