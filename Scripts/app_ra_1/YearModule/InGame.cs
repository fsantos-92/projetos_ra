using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using App.Cards;
using UnityEngine.UI;
using System;

namespace App.Game
{
    public class InGame : MonoBehaviour
    {
        public VuforiaBehaviour ARCamera;
        public GameObject CountDownScreen;
        public GameObject CongratsScreen;
        [SerializeField]
        GameObject[] CardsAudioclips;
        [SerializeField]
        Sprite[] SpritesCards;
        [SerializeField]
        Color[] ColorsCards;

        [SerializeField]
        GameObject[] TargetsYearOne;
        [SerializeField]
        GameObject[] TargetsYearTwo;
        [SerializeField]
        GameObject[] TargetsYearThree;
        [SerializeField]
        GameObject[] TargetsYearFour;
        [SerializeField]
        GameObject[] TargetsYearFive;

        [SerializeField]
        Button BtnToQuestion;

        [SerializeField]
        GameObject[] ChildrenScreens;

        List<GameObject> CurrentTargets = new List<GameObject>();

        List<int> MissedTargets = new List<int>();

        bool HasSkippedCard = false;


        int[] Order = { 0, 1, 2, 3, 4 };

        string[] CardsDescriptions = {  "Cards de audição podem ser escondidos em ambientes com música ou sons reconhecíveis",
                                        "Cards de olfato podem ser escondidos em ambientes com cheiros reconhecíveis",
                                        "Cards de paladar, podem ser escondidos em ambientes que possuem alimentos e/ou bebidas",
                                        "Cards de tato podem ser escondidos em plantas e vasos, ou outros tipos de texturas",
                                        "Cards de visão podem ser escondidos em janelas ou ambientes com apelo visual e cores" };

        int CurrentCard = 0;

        CanvasGroup canvasGroup;

        void Start()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            if (ARCamera.enabled)
                ARCamera.enabled = false;
        }
        void OnEnable()
        {

            if (!canvasGroup)
                canvasGroup = GetComponent<CanvasGroup>();

            this.ResetScreen();
            BgManager.Instance.SetBgColor(BgColorsEnum.RED);
            BgManager.Instance.SetAllIconsWhiteColor(true);
            BgManager.Instance.SetIconsCanvasAlpha(0.32f);

            LeanTween.alphaCanvas(canvasGroup, 1, 0.35f).setDelay(0.3f);
        }

        void ResetScreen()
        {
            this.HasSkippedCard = false;
            this.CurrentCard = 0;
            foreach (GameObject element in this.ChildrenScreens)
            {
                element.SetActive(false);
            }
            // this.ARCamera.gameObject.SetActive(true);
            this.BtnToQuestion.gameObject.SetActive(false);
            this.ShuffleOrder(this.Order);
            this.InstanceTargetPrefabs();
            this.StartGame();
        }

        void ShuffleOrder(int[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                int rnd = UnityEngine.Random.Range(0, array.Length);
                int tempGO = array[rnd];
                array[rnd] = array[i];
                array[i] = tempGO;
            }
        }

        void InstanceTargetPrefabs()
        {
            this.CurrentTargets = new List<GameObject>();
            this.MissedTargets = new List<int>();
            int index = 0;

            switch (PlayerPrefs.GetInt("Year"))
            {
                case 1:
                    foreach (GameObject element in this.TargetsYearOne)
                    {
                        this.CurrentTargets.Add(this.TargetsYearOne[index]);
                        index++;
                    }
                    break;
                case 2:
                    foreach (GameObject element in this.TargetsYearTwo)
                    {
                        this.CurrentTargets.Add(this.TargetsYearTwo[index]);
                        index++;
                    }
                    break;
                case 3:
                    foreach (GameObject element in this.TargetsYearThree)
                    {
                        this.CurrentTargets.Add(this.TargetsYearThree[index]);
                        index++;
                    }
                    break;
                case 4:
                    foreach (GameObject element in this.TargetsYearFour)
                    {
                        this.CurrentTargets.Add(this.TargetsYearFour[index]);
                        index++;
                    }
                    break;
                case 5:
                    foreach (GameObject element in this.TargetsYearFive)
                    {
                        this.CurrentTargets.Add(this.TargetsYearFive[index]);
                        index++;
                    }
                    break;
            }
        }

        public string GetCurrentCardName()
        {
            return this.CurrentTargets[Order[CurrentCard]].name;
        }

        public void StartGame()
        {
            this.CountDownScreen.SetActive(true);
        }
        void EndGame()
        {
            if (ARCamera.enabled)
                ARCamera.enabled = false;
            this.CongratsScreen.SetActive(true);
        }

        public void CheckCompletion()
        {
            if (this.CurrentCard < this.TargetsYearOne.Length - 1)
            {
                this.CurrentCard = this.CurrentCard + 1;
                this.StartGame();
            }
            else
            {
                this.EndGame();
            }
        }

        public Sprite GetCardSprite()
        {
            return this.SpritesCards[this.Order[this.CurrentCard]];
        }
        public Color GetCardColor()
        {
            return this.ColorsCards[this.Order[this.CurrentCard]];
        }
        public string GetCardDescription()
        {
            return this.CardsDescriptions[this.Order[this.CurrentCard]];
        }

        public void SkipCard()
        {
            this.HasSkippedCard = true;
        }

        public bool HasSkipped()
        {
            return this.HasSkippedCard;
        }

        public int GetCurrentCard()
        {
            return this.CurrentCard;
        }

        public void FadeThis()
        {
            LeanTween.alphaCanvas(canvasGroup, 0, 0.3f).setOnComplete(() => gameObject.SetActive(false));
        }
        public void PlayCardTip()
        {
            AudioClip tip = null;
            if (this.CardsAudioclips[this.Order[this.CurrentCard]].GetComponent<CardRecord>().GetCardTip())
            {
                tip = this.CardsAudioclips[this.Order[this.CurrentCard]].GetComponent<CardRecord>().GetCardTip();
            }
            if (tip != null)
            {
                // this.gameObject.GetComponent<AudioSource>().PlayOneShot(tip);
                SoundManager.Instance.PlayRecordingTip(tip);
            }
        }
        public bool HasCardTip()
        {
            bool tip = false;
            if (this.CardsAudioclips[this.Order[this.CurrentCard]].GetComponent<CardRecord>().GetCardTip())
            {
                tip = true;
            }
            return tip;
        }
    }
}
