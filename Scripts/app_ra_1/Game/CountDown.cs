using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace App.Game
{
    public class CountDown : MonoBehaviour
    {
        public TMPro.TMP_Text text;
        public Text TextYear;
        public Text TextCurrentCard;

        public GameObject InGame;
        public Image Card;

        public GameObject NextScreen;
        bool FirstEnable = false;
        // Start is called before the first frame update
        void OnEnable()
        {
            if(!FirstEnable)
            {
                FirstEnable = true;
                Firebase.Analytics.FirebaseAnalytics.LogEvent("Tela_Atingida", "Tela","Contagem_Regressiva");
            }

            BgManager.Instance.SetBgColor(BgColorsEnum.RED);
            BgManager.Instance.SetAllIconsWhiteColor(true);
            BgManager.Instance.SetIconsCanvasAlpha(0.32f);

            this.Card.sprite = this.InGame.GetComponent<InGame>().GetCardSprite();
            this.TextYear.color = this.InGame.GetComponent<InGame>().GetCardColor();
            this.TextCurrentCard.gameObject.SetActive(false);
            TextYear.text = PlayerPrefs.GetInt("Year") + "º";
            this.Card.gameObject.SetActive(false);
            this.TextCurrentCard.text = CurrentCardText();
            StartCoroutine(count());
        }

        string CurrentCardText()
        {
            switch (this.InGame.GetComponent<InGame>().GetCurrentCard())
            {
                case 0:
                    return "Primeira Carta";
                case 1:
                    return "Segunda Carta";
                case 2:
                    return "Terceira Carta";
                case 3:
                    return "Quarta Carta";
                case 4:
                    return "Quinta Carta";
                default: return null;
            }
        }

        IEnumerator count()
        {
            CanvasGroup txtCanvas = text.GetComponent<CanvasGroup>();

            yield return new WaitForSeconds(1f);

            for (int i = 3; i >= 0; i--)
            {
                text.rectTransform.localScale = Vector3.one;
                txtCanvas.alpha = 1;
                text.text = i <= 0 ? "Vai!!" : i.ToString();
                LeanTween.scale(text.rectTransform, Vector3.one * 5, 1);
                LeanTween.alphaCanvas(txtCanvas, 0, 1);
                yield return new WaitForSeconds(1f);
            }

            this.TextCurrentCard.gameObject.SetActive(true);
            this.Card.gameObject.SetActive(true);
            yield return new WaitForSeconds(2f);
            BgManager.Instance.SetBgTransparent(true);
            this.NextScreen.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }

}