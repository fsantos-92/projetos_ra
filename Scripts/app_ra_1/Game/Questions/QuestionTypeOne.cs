using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace App.Game
{

    public class QuestionTypeOne : MonoBehaviour
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
        string RightAnswer;
        [SerializeField]

        string[] Answers;

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
            if (BtnPlayQuestion != null)
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
            this.SelectedAnswer = btn.GetComponentInChildren<Text>().text;
            this.CurrentSelectedButton = btn;
            foreach (Button element in BtnAnswers)
            {
                if (element.name == btn.name)
                {
                    element.GetComponent<Image>().color = this.Yellow;
                    element.GetComponentInChildren<Text>().color = this.Yellow;
                }
                else
                {
                    element.GetComponent<Image>().color = this.DarkBlue;
                    element.GetComponentInChildren<Text>().color = this.DarkBlue;
                }
            }
            SoundManager.Instance.PlayClip(ClipButton);
        }

        void BtnCheckAnswerClick()
        {
            SoundManager.Instance.Stop();
            SoundManager.Instance.PlayClip(ClipButton);
            if (this.SelectedAnswer.ToLower().Trim() == this.RightAnswer.ToLower().Trim())
            {
                StartCoroutine(this.ShowFeedback(this.ImgPositiveFeedback, true));
                SoundManager.Instance.PlayClip(ClipPosFeedback);
            }
            else
            {
                StartCoroutine(this.ShowFeedback(this.ImgNegativeFeedback));
                // this.gameObject.GetComponent<AudioSource>().PlayOneShot(ClipNegFeedback);
                SoundManager.Instance.PlayClip(ClipNegFeedback);
                this.CurrentSelectedButton.interactable = false;
                this.CurrentSelectedButton.GetComponentInChildren<Text>().color = Color.grey;
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
                element.GetComponentInChildren<Text>().color = this.DarkBlue;
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
                //Troca o <br> por uma quebra de linha na string
                BtnAnswers[i].GetComponentInChildren<Text>().text = this.Answers[this.Order[i]].Replace("<br>", "\n");
            }
            this.RightAnswer = this.RightAnswer.Replace("<br>", "\n");
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
