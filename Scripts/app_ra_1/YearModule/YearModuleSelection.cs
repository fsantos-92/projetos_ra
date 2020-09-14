using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace App.YearModule
{

    public class YearModuleSelection : MonoBehaviour
    {
        public Button[] btnYears;
        public Color[] colorBorderYears;
        public Color[] colorFillYears;
        public Button[] btnModules;
        public Color[] colorBorderModule;
        public Color[] colorFillModule;

        public Button btnBack;
        public Button btnNext;

        public GameObject scrollYear;
        public GameObject scrollModule;

        public Color OriginalTxtColor;

        public int CurrentYearSelection = -1;
        public int CurrentModuleSelection = -1;

        public GameObject previousScreen;
        public GameObject nextScreen;

        [SerializeField]
        AudioClip ClipButton;
        bool FirstEnable;

        CanvasGroup canvas;

        // Start is called before the first frame update
        void Awake()
        {
            canvas = GetComponent<CanvasGroup>();
            BindButtons();
            ResetScreen();
        }
        void OnEnable()
        {
            if(!FirstEnable)
            {
                FirstEnable = true;
                Firebase.Analytics.FirebaseAnalytics.LogEvent("Tela_Atingida", "Tela","Selecao_Ano_Modulo");
            }

            ResetScreen();

            LeanTween.alphaCanvas(canvas, 1, 0.25f).setDelay(0.25f);

            BgManager.Instance.SetBgColor(BgColorsEnum.WHITE);
            BgManager.Instance.SetIconsCanvasAlpha(0.1f);
            BgManager.Instance.SetAllIconsWhiteColor(false);
        }
        void OnDisable()
        {
            StopAllCoroutines();
            canvas.alpha = 0;
        }
        void ResetScreen()
        {
            this.CurrentYearSelection = -1;
            this.CurrentModuleSelection = -1;
            this.scrollYear.GetComponent<RectTransform>().transform.localPosition = new Vector3(0f, this.scrollYear.GetComponent<RectTransform>().transform.localPosition.y, this.scrollYear.GetComponent<RectTransform>().transform.localPosition.z);
            this.scrollModule.GetComponent<RectTransform>().transform.localPosition = new Vector3(0f, this.scrollModule.GetComponent<RectTransform>().transform.localPosition.y, this.scrollModule.GetComponent<RectTransform>().transform.localPosition.z);
            foreach (Button element in btnYears)
            {
                element.GetComponent<Image>().color = this.OriginalTxtColor;
                element.GetComponentInChildren<Text>().color = this.OriginalTxtColor;
                element.transform.parent.GetComponent<Image>().color = Color.white;
            }
            foreach (Button element in btnModules)
            {
                element.GetComponent<Image>().color = this.OriginalTxtColor;
                element.GetComponentInChildren<Text>().color = this.OriginalTxtColor;
                element.transform.parent.GetComponent<Image>().color = Color.white;
            }
            this.btnNext.gameObject.SetActive(false);
        }

        public Color GetBoxColor(string box)
        {
            switch (box)
            {
                case "YearBoxBorder":
                    return this.colorBorderYears[Math.Abs(this.CurrentYearSelection)];
                case "YearBoxFill":
                    return this.colorFillYears[Math.Abs(this.CurrentYearSelection)];
                case "ModuleBoxBorder":
                    return this.colorBorderModule[Math.Abs(this.CurrentModuleSelection)];
                case "ModuleBoxFill":
                    return this.colorFillModule[Math.Abs(this.CurrentModuleSelection)];
                default:
                    return Color.black;
            }
        }

        #region ButtonsFunctions
        void BindButtons()
        {
            this.btnBack.onClick.AddListener(OnBackButtonClick);
            this.btnNext.onClick.AddListener(OnNextButtonClick);
            foreach (Button element in btnYears)
            {
                element.onClick.AddListener(delegate { OnYearButtonClick(element); });
            }
            foreach (Button element in btnModules)
            {
                element.onClick.AddListener(delegate { OnModuleButtonClick(element); });
            }
        }

        void OnBackButtonClick()
        {
            StopAllCoroutines();
            SoundManager.Instance.PlayClip(ClipButton);

            LeanTween.alphaCanvas(canvas, 0, 0.25f).setOnComplete(()=> {
                gameObject.SetActive(false);
                previousScreen.SetActive(true);
            });

            
        }
        void OnNextButtonClick()
        {
            StopAllCoroutines();
            SoundManager.Instance.PlayClip(ClipButton);
            PlayerPrefs.SetInt("Year", this.CurrentYearSelection + 1);
            PlayerPrefs.SetInt("Module", this.CurrentModuleSelection + 1);
            LeanTween.alphaCanvas(canvas, 0, 0.25f).setOnComplete(() => {
                gameObject.SetActive(false);
                nextScreen.SetActive(true);
            });
            
        }
        void CheckNextButtonEnable()
        {
            if (this.CurrentModuleSelection != -1 && this.CurrentYearSelection != -1)
            {
                this.btnNext.gameObject.SetActive(true);
            }
        }

        void OnYearButtonClick(Button btn)
        {
            SoundManager.Instance.PlayClip(ClipButton);
            int index = 0;
            foreach (Button element in btnYears)
            {
                if (btn.gameObject.name == element.gameObject.name)
                {
                    element.GetComponent<Image>().color = this.colorBorderYears[index];
                    element.GetComponentInChildren<Text>().color = this.colorBorderYears[index];
                    element.transform.parent.GetComponent<Image>().color = this.colorFillYears[index];
                    this.CurrentYearSelection = index;
                }
                else
                {
                    element.GetComponent<Image>().color = this.OriginalTxtColor;
                    element.GetComponentInChildren<Text>().color = this.OriginalTxtColor;
                    element.transform.parent.GetComponent<Image>().color = Color.white;
                }
                index = index + 1;
            }
            this.CheckNextButtonEnable();
        }
        void OnModuleButtonClick(Button btn)
        {
            SoundManager.Instance.PlayClip(ClipButton);
            int index = 0;
            foreach (Button element in btnModules)
            {
                if (btn.gameObject.name == element.gameObject.name)
                {
                    element.GetComponent<Image>().color = this.colorBorderModule[index];
                    element.transform.parent.GetComponent<Image>().color = this.colorFillModule[index];
                    element.GetComponentInChildren<Text>().color = Color.white;
                    this.CurrentModuleSelection = index;
                }
                else
                {
                    element.GetComponent<Image>().color = this.OriginalTxtColor;
                    element.GetComponentInChildren<Text>().color = this.OriginalTxtColor;
                    element.transform.parent.GetComponent<Image>().color = Color.white;
                }
                index = index + 1;
            }
            this.CheckNextButtonEnable();
        }
        #endregion

    }
}
