using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace App.Game
{

    public class QuestionScreen : MonoBehaviour
    {
        [SerializeField]
        GameObject InGameScreen;

        [Header("Ano 1")]
        [SerializeField]
        GameObject[] QuestionsYearOneModOne;
        [SerializeField]
        GameObject[] QuestionsYearOneModTwo;
        [SerializeField]
        GameObject[] QuestionsYearOneModThree;
        [SerializeField]
        GameObject[] QuestionsYearOneModFour;

        //

        [Header("Ano 2")]
        [SerializeField]
        GameObject[] QuestionsYearTwoModOne;
        [SerializeField]
        GameObject[] QuestionsYearTwoModTwo;
        [SerializeField]
        GameObject[] QuestionsYearTwoModThree;
        [SerializeField]
        GameObject[] QuestionsYearTwoModFour;

        //

        [Header("Ano 3")]
        [SerializeField]
        GameObject[] QuestionsYearThreeModOne;
        [SerializeField]
        GameObject[] QuestionsYearThreeModTwo;
        [SerializeField]
        GameObject[] QuestionsYearThreeModThree;
        [SerializeField]
        GameObject[] QuestionsYearThreeModFour;

        //

        [Header("Ano 4")]
        [SerializeField]
        GameObject[] QuestionsYearFourModOne;
        [SerializeField]
        GameObject[] QuestionsYearFourModTwo;
        [SerializeField]
        GameObject[] QuestionsYearFourModThree;
        [SerializeField]
        GameObject[] QuestionsYearFourModFour;

        //

        [Header("Ano 5")]
        [SerializeField]
        GameObject[] QuestionsYearFiveModOne;
        [SerializeField]
        GameObject[] QuestionsYearFiveModTwo;
        [SerializeField]
        GameObject[] QuestionsYearFiveModThree;
        [SerializeField]
        GameObject[] QuestionsYearFiveModFour;

        //
        int[] Order;

        List<int> order = new List<int>();

        bool FirstEnable = false;


        // Start is called before the first frame update
        void Awake()
        {
            for (int i = 0; i < this.QuestionsYearOneModOne.Length; i++)
            {
                this.order.Add(i);
            }
            Order = this.order.ToArray();
            ShuffleSequence(this.Order);
        }

        public void ShuffleSequence(int[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                int rnd = UnityEngine.Random.Range(0, array.Length);
                int tempGO = array[rnd];
                array[rnd] = array[i];
                array[i] = tempGO;
            }
        }

        void OnEnable()
        {
            if(!FirstEnable)
            {
                FirstEnable = true;
                Firebase.Analytics.FirebaseAnalytics.LogEvent("Tela_Atingida", "Tela","Tela_de_Perguntas");
            }
            this.InstantiateQuestion();
        }

        void InstantiateQuestion()
        {
            int year = PlayerPrefs.GetInt("Year");
            int module = PlayerPrefs.GetInt("Module");
            int current = this.InGameScreen.GetComponent<InGame>().GetCurrentCard();

            switch (year)
            {
                case 1:
                    switch (module)
                    {
                        case 1:
                            this.QuestionsYearOneModOne[this.Order[current]].SetActive(true);
                            break;
                        case 2:
                            this.QuestionsYearOneModTwo[this.Order[current]].SetActive(true);
                            break;
                        case 3:
                            this.QuestionsYearOneModThree[this.Order[current]].SetActive(true);
                            break;
                        case 4:
                            this.QuestionsYearOneModFour[this.Order[current]].SetActive(true);
                            break;
                    }
                    break;
                case 2:
                    switch (module)
                    {
                        case 1:
                            this.QuestionsYearTwoModOne[this.Order[current]].SetActive(true);
                            break;
                        case 2:
                            this.QuestionsYearTwoModTwo[this.Order[current]].SetActive(true);
                            break;
                        case 3:
                            this.QuestionsYearTwoModThree[this.Order[current]].SetActive(true);
                            break;
                        case 4:
                            this.QuestionsYearTwoModFour[this.Order[current]].SetActive(true);
                            break;
                    }
                    break;
                case 3:
                    switch (module)
                    {
                        case 1:
                            this.QuestionsYearThreeModOne[this.Order[current]].SetActive(true);
                            break;
                        case 2:
                            this.QuestionsYearThreeModTwo[this.Order[current]].SetActive(true);
                            break;
                        case 3:
                            this.QuestionsYearThreeModThree[this.Order[current]].SetActive(true);
                            break;
                        case 4:
                            this.QuestionsYearThreeModFour[this.Order[current]].SetActive(true);
                            break;
                    }
                    break;
                case 4:
                    switch (module)
                    {
                        case 1:
                            this.QuestionsYearFourModOne[this.Order[current]].SetActive(true);
                            break;
                        case 2:
                            this.QuestionsYearFourModTwo[this.Order[current]].SetActive(true);
                            break;
                        case 3:
                            this.QuestionsYearFourModThree[this.Order[current]].SetActive(true);
                            break;
                        case 4:
                            this.QuestionsYearFourModFour[this.Order[current]].SetActive(true);
                            break;
                    }
                    break;
                case 5:
                    switch (module)
                    {
                        case 1:
                            this.QuestionsYearFiveModOne[this.Order[current]].SetActive(true);
                            break;
                        case 2:
                            this.QuestionsYearFiveModTwo[this.Order[current]].SetActive(true);
                            break;
                        case 3:
                            this.QuestionsYearFiveModThree[this.Order[current]].SetActive(true);
                            break;
                        case 4:
                            this.QuestionsYearFiveModFour[this.Order[current]].SetActive(true);
                            break;
                    }
                    break;
            }

        }
    }
}
