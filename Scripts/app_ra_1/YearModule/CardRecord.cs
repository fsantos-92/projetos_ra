using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Android;
using System;

namespace App.Cards
{
    public class CardRecord : MonoBehaviour
    {
        private const int RECORD_TIME = 20;
        [SerializeField]
        AudioClip recordingAudioClip;
        private float startRecordingTime;
        [SerializeField]
        Text textRecordTime;
        
        Color weakColor;
        [SerializeField]
        Color strongColor;
        [SerializeField]
        Button BtnRecord;
        [SerializeField]
        Button BtnPlay;
        [SerializeField]
        Button BtnPause;
        [SerializeField]
        Button BtnDelete;
        [SerializeField]
        Image recordingMicBG;
        [SerializeField]
        Image bgImage;

        Image bgBorderImage;

        [SerializeField]
        Button BtnConfirmDelete;
        [SerializeField]
        Button BtnCancelDelete;
        bool DeleteRequested;
        public GameObject DeleteScreen;

        CanvasGroup deleteScreenCanvasGroup;

        [SerializeField]
        CanvasGroup recordingInfoCanvasGroup;

        RectTransform BtnRecordRectTransform;

        bool isRecording = false;

        float recordingTime;

        bool isPlaying;

        const float BtnRecordPositionEnabled = 372.76f;
        const float BtnRecordPositionDisabled = -94f;

        public static bool IsRecordingAllowed = true;

        // Start is called before the first frame update
        void Start()
        {
            this.DeleteScreen.SetActive(false);
            this.BtnPlay.onClick.AddListener(BtnPlayClick);
            this.BtnDelete.onClick.AddListener(BtnDeleteClick);
            this.BtnConfirmDelete.onClick.AddListener(BtnConfirmDeleteClick);
            this.BtnCancelDelete.onClick.AddListener(BtnCancelDeleteClick);
            this.BtnPause.onClick.AddListener(BtnPauseClick);

            
            BtnDelete.interactable = false;

            LeanTween.alphaCanvas(recordingInfoCanvasGroup, 0, 0);

            BtnRecordRectTransform = BtnRecord.GetComponent<RectTransform>();

            bgBorderImage = GetComponent<Image>();

            weakColor = new Color(strongColor.r, strongColor.g, strongColor.b, 0.357f);

            if (!deleteScreenCanvasGroup)
                deleteScreenCanvasGroup = DeleteScreen.GetComponent<CanvasGroup>();

            deleteScreenCanvasGroup.alpha = 0;

        }

        void Update()
        {
            if (isRecording) {
                recordingTime = Time.time - startRecordingTime;

                TimeSpan t = TimeSpan.FromSeconds(recordingTime);

                string prettyTime = string.Format("{0:D2}:{1:D2}",
                                t.Minutes,
                                t.Seconds);
                textRecordTime.text = prettyTime;
            }

            BtnDelete.interactable = recordingAudioClip != null && !isRecording && !isPlaying;

            BtnRecord.interactable = recordingAudioClip == null || isRecording;

            if (recordingAudioClip != null)
            {
                bool hasSameClip = SoundManager.Instance.GetRecordingTipAudioClip()?.name == recordingAudioClip.name;

                isPlaying = hasSameClip && SoundManager.Instance.IsPlayingRecordingTip();

                BtnPlay.gameObject.SetActive(!isPlaying && !isRecording);
                BtnPause.gameObject.SetActive(isPlaying && !isRecording);

                if (!isRecording)
                {
                    bgImage.color = Color.Lerp(bgImage.color, new Color(0.85f, 0.85f, 0.85f, 1f), Time.deltaTime * 3);
                    recordingMicBG.color = Color.Lerp(recordingMicBG.color, Color.clear, Time.deltaTime * 5);
                    bgBorderImage.color = Color.Lerp(bgBorderImage.color, strongColor, Time.deltaTime * 3);
                }
                
            }
            else
            {
                BtnPlay.gameObject.SetActive(false);
                BtnPause.gameObject.SetActive(false);

                bgImage.color = Color.Lerp(bgImage.color, new Color(1f, 1f, 1f, 1f), Time.deltaTime * 3);
                recordingMicBG.color = Color.Lerp(recordingMicBG.color, weakColor, Time.deltaTime * 5);
                bgBorderImage.color = Color.Lerp(bgBorderImage.color, new Color(0.566f, 0.566f, 0.566f, 1f), Time.deltaTime * 3);
            }

            if(!BtnRecord.interactable)
            {
                if (Mathf.Abs(BtnRecordRectTransform.anchoredPosition.x - BtnRecordPositionEnabled) <= 0)
                    LeanTween.move(BtnRecordRectTransform, new Vector3(BtnRecordPositionDisabled, BtnRecordRectTransform.anchoredPosition.y, 0), 0.35f);
            }
            else
            {
                if (Mathf.Abs(BtnRecordRectTransform.anchoredPosition.x - BtnRecordPositionDisabled) <= 0)
                    LeanTween.move(BtnRecordRectTransform, new Vector3(BtnRecordPositionEnabled, BtnRecordRectTransform.anchoredPosition.y, 0), 0.35f);
            }
   
        }

        private void BtnCancelDeleteClick()
        {
            this.DeleteRequested = false;
            LeanTween.alphaCanvas(deleteScreenCanvasGroup, 0, 0.3f).setOnComplete(() => this.DeleteScreen.SetActive(false));
        }

        private void BtnConfirmDeleteClick()
        {
            if (!this.DeleteRequested)
                return;

            this.recordingAudioClip = null;

            LeanTween.alphaCanvas(deleteScreenCanvasGroup, 0, 0.3f).setOnComplete(()=> this.DeleteScreen.SetActive(false));

            this.DeleteRequested = false;
        }

        private void BtnDeleteClick()
        {
            
            this.DeleteRequested = true;
            this.DeleteScreen.SetActive(true);
            LeanTween.alphaCanvas(deleteScreenCanvasGroup, 1, 0.3f);
        }

        private void BtnPlayClick()
        {
            
            isPlaying = true;

            SoundManager.Instance.PlayRecordingTip(this.recordingAudioClip);
            
        }

        private void BtnPauseClick()
        {
            SoundManager.Instance.PauseRecordingTip();  
        }

        public AudioClip GetCardTip()
        {
            return IsRecordingAllowed ? this.recordingAudioClip : null;
        }

        public void SetAudioTipNull()
        {
            this.recordingAudioClip = null;

        }

        /// <summary>
        /// Chamado pela UI
        /// </summary>
        public void BtnPressed()
        {
            if (recordingAudioClip)
                return;

            if (!MicPermission())
                return;

            StopAllCoroutines();
            StartCoroutine(CountToRecord());
        }

        /// <summary>
        /// Chamado Pela UI
        /// </summary>
        public void BtnReleased()
        {
            StopAllCoroutines();
            StopRecording();
        }

        IEnumerator CountToRecord()
        {
            yield return new WaitForSeconds(0.5f);
            StartRecord();
        }

        public void StartRecord()
        {
            SoundManager.Instance.disableMusic();

            LeanTween.alphaCanvas(recordingInfoCanvasGroup, 1, 0.3f);

            if (!MicPermission())
                return;

            //Get the max frequency of a microphone, if it's less than 44100 record at the max frequency, else record at 44100
            int minFreq;
            int maxFreq;
            int freq = 44100;
            Microphone.GetDeviceCaps("", out minFreq, out maxFreq);
            if (maxFreq < 44100 && minFreq > 0 && maxFreq > 0)
                freq = maxFreq;

            //Start the recording
            recordingAudioClip = Microphone.Start("", false, RECORD_TIME, freq);
            startRecordingTime = Time.time;

            this.isRecording = true;
        }
        public void StopRecording()
        {
          
            if (!isRecording)
                return;

            LeanTween.alphaCanvas(recordingInfoCanvasGroup, 0, 0.3f);

            //End the recording when the mouse comes back up
            Microphone.End("");

            //Trim the audioclip by the length of the recording
            AudioClip recordingNew = AudioClip.Create(gameObject.name, (int)((Time.time - startRecordingTime) * recordingAudioClip.frequency), recordingAudioClip.channels, recordingAudioClip.frequency, false);
            float[] data = new float[(int)((Time.time - startRecordingTime) * recordingAudioClip.frequency)];
            recordingAudioClip.GetData(data, 0);
            recordingNew.SetData(data, 0);
            this.recordingAudioClip = recordingNew;

            
            this.isRecording = false;
            SoundManager.Instance.enableMusic();
        }
        bool MicPermission()
        {
#if UNITY_ANDROID
            if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
            {
                Permission.RequestUserPermission(Permission.Microphone);
                return false;
            }
            else return true;
#elif UNITY_IOS
            if (!Application.HasUserAuthorization(UserAuthorization.Microphone))
            {
                Application.RequestUserAuthorization(UserAuthorization.Microphone);
                return false;
            } 
			else return true;
#endif
            return true;
        }
    }
}

