using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace App.Game
{

    public class QuestionTypeTwo : MonoBehaviour
    {
        [SerializeField]
        GameObject InGameScreen;
        [SerializeField]
        GameObject ScreenQuestion;
        [Header("Texts")]
        [SerializeField]
        Text TxtQuestion;
        [Header("Buttons")]
        [SerializeField]
        Button BtnPlayQuestion;
        [SerializeField]
        Button[] BtnAnswers;
        [SerializeField]
        Button BtnCheckAnswer;

        [Header("Feedbacks")]
        [SerializeField]
        GameObject ImgPositiveFeedback;
        [SerializeField]
        GameObject ImgNegativeFeedback;

        [SerializeField]
        Color DarkBlue;
        [SerializeField]
        Color Yellow;

        [Header("SoundClips")]
        [SerializeField]
        AudioClip ClipQuestion;

        [SerializeField]
        AudioClip ClipButton;

        [SerializeField]
        AudioClip ClipNegFeedback;
        [SerializeField]
        AudioClip ClipPosFeedback;

        //
        [Header("Question")]
        [SerializeField]
        string StringQuestion;
        [SerializeField]
        Sprite RightAnswer;
        [SerializeField]

        Sprite[] Answers;

        string SelectedAnswer;

        Button CurrentSelectedButton;

        int[] Order;

        List<int> order = new List<int>();

        // Start is called before the first frame update
        void Awake()
        {
            for (int i = 0; i < this.Answers.Length; i++)
            {
                this.order.Add(i);
            }
            Order = this.order.ToArray();
            foreach (Button element in BtnAnswers)
            {
                element.onClick.AddListener(delegate { BtnAnswerClick(element); });
            }
            this.BtnCheckAnswer.onClick.AddListener(BtnCheckAnswerClick);
            if(BtnPlayQuestion != null)
            this.BtnPlayQuestion.onClick.AddListener(BtnPlayQuestionClick);
        }

        private void BtnPlayQuestionClick()
        {
            if (this.ClipQuestion != null)
                SoundManager.Instance.PlayRecordingTip(ClipQuestion);
        }

        private void BtnAnswerClick(Button btn)
        {
            this.BtnCheckAnswer.gameObject.SetActive(true);
            this.SelectedAnswer = btn.transform.GetChild(0).GetComponent<Image>().sprite.name;
            this.CurrentSelectedButton = btn;
            foreach (Button element in BtnAnswers)
            {
                if (element.name == btn.name)
                {
                    element.GetComponent<Image>().color = this.Yellow;
                }
                else
                {
                    element.GetComponent<Image>().color = this.DarkBlue;
                }
            }
            SoundManager.Instance.PlayClip(ClipButton);
        }

        void BtnCheckAnswerClick()
        {
            SoundManager.Instance.Stop();
            SoundManager.Instance.PlayClip(ClipButton);
            string rightAnswer = this.RightAnswer.name;
            if (this.SelectedAnswer == rightAnswer)
            {
                StartCoroutine(this.ShowFeedback(this.ImgPositiveFeedback, true));
                SoundManager.Instance.PlayClip(ClipPosFeedback);
            }
            else
            {
                StartCoroutine(this.ShowFeedback(this.ImgNegativeFeedback));
                SoundManager.Instance.PlayClip(ClipNegFeedback);
                this.CurrentSelectedButton.interactable = false;
            }
        }
        void OnEnable()
        {
            this.gameObject.GetComponent<CanvasGroup>().alpha = 1;
            this.BtnCheckAnswer.gameObject.SetActive(false);
            this.UnlockButtons();
            this.ShuffleOrder(this.Order);
            this.SetAnswers();
        }

        private void UnlockButtons()
        {
            foreach (Button element in this.BtnAnswers)
            {
                element.interactable = true;
                element.GetComponent<Image>().color = this.DarkBlue;
            }
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
        void SetAnswers()
        {
            this.SelectedAnswer = "";
            this.TxtQuestion.text = this.StringQuestion;
            for (int i = 0; i < this.BtnAnswers.Length; i++)
            {
                BtnAnswers[i].transform.GetChild(0).GetComponent<Image>().sprite = this.Answers[this.Order[i]];
                BtnAnswers[i].transform.GetChild(0).GetComponent<Image>().preserveAspect = true;
            }
        }

        IEnumerator ShowFeedback(GameObject Feedback, bool IsRight = false)
        {
            Feedback.SetActive(true);
            yield return new WaitForSeconds(2f);
            Feedback.SetActive(false);
            if (IsRight)
            {
                this.InGameScreen.GetComponent<InGame>().CheckCompletion();
                this.ScreenQuestion.SetActive(false);
                this.gameObject.SetActive(false);
            }
        }
    }
}
