using System.Collections;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using System;

namespace App.Tools
{
    public class CustomVideoPlayer : MonoBehaviour
    {
        //
        [SerializeField]
        VideoPlayer videoPlayer;
        AudioSource audiosource;
        //
        [SerializeField]
        Button BtnPlay;
        [SerializeField]
        Button BtnFullScreen;
        //
        bool IsFullscreen;
        bool IsSliding;
        bool isSeeking;
        //
        [SerializeField]
        RawImage imgTexture;
        [SerializeField]
        Texture videoPlaceholderTexture;
        //
        [SerializeField]
        Slider sliderVideo;
        //
        [SerializeField]
        GameObject ToolBar;
        //
        [SerializeField]
        RectTransform PlayerBG;
        [SerializeField]
        RectTransform VideoRect;
        //
        [SerializeField]
        GameObject ContentContainer;
        //
        [Header("Button Sprites")]
        [SerializeField]
        Sprite SpriteBtnPlay;
        [SerializeField]
        Sprite SpriteBtnPause;
        [SerializeField]
        Sprite SpriteBtnFullScreenMax;
        [SerializeField]
        Sprite SpriteBtnFullScreenMin;

        [SerializeField]
        GameObject ImgErrorMessage;

        Coroutine prepareVideoRoutine;

        const float ScreenWidth = 1080;
        const float ScreenHeight = 1920;
        void Awake()
        {
            this.VideoPlayerSetup();
            this.BindButtons();
            ImgErrorMessage.gameObject.SetActive(false);
            imgTexture.texture = videoPlaceholderTexture;
            videoPlayer.loopPointReached += LoopReached;
            videoPlayer.seekCompleted += SeekCompleted;

        }

        private void SeekCompleted(VideoPlayer source)
        {
            LeanTween.value(0, 1, 0.3f).setOnComplete(() => isSeeking = false);
        }

        private void LoopReached(VideoPlayer source)
        {
            BtnPlayClick();
        }

        void Update()
        {
            if (!this.IsSliding && !isSeeking)
                this.sliderVideo.value = this.videoPlayer.frame / (float)this.videoPlayer.frameCount;
            if (videoPlayer.isPlaying)
                imgTexture.texture = videoPlayer.texture;
            else
            if(!videoPlayer.isPrepared)
                imgTexture.texture = videoPlaceholderTexture;
        }
        void OnEnable()
        {
            this.ImgErrorMessage.SetActive(false);
        }
        IEnumerator PrepareVideo()
        {
            imgTexture.texture = videoPlaceholderTexture;
            this.BtnPlay.gameObject.SetActive(false);
            this.videoPlayer.errorReceived += VideoPlayer_errorReceived;
            this.videoPlayer.Prepare();
            while (!this.videoPlayer.isPrepared)
            {
                yield return new WaitForEndOfFrame();
            }
            Debug.Log("Video ready");
            this.BtnPlay.gameObject.SetActive(true);
            BtnFullScreenClick();
            BtnPlayClick();

            yield return null;
        }

        public void SetURL(string url)
        {
            this.videoPlayer.url = url;
            if(prepareVideoRoutine != null)
                StopCoroutine(prepareVideoRoutine);

            prepareVideoRoutine = StartCoroutine(PrepareVideo());

        }

        void ScaleVideo()
        {
            float aspect;
            float width;
            float height;
            if (this.videoPlayer.texture.width > this.videoPlayer.texture.height)
            {
                aspect = (float)this.videoPlayer.texture.width / (float)this.videoPlayer.texture.height;
                float playerAspect = this.PlayerBG.sizeDelta.x / this.PlayerBG.sizeDelta.y;
                if (playerAspect > aspect)
                {
                    aspect = (float)this.videoPlayer.texture.width / (float)this.videoPlayer.texture.height;
                    height = this.PlayerBG.sizeDelta.y;
                    width = height * aspect;
                }
                else
                {
                    width = this.PlayerBG.sizeDelta.x;
                    height = width * (1 / aspect);
                }

            }
            else if (this.videoPlayer.texture.width < this.videoPlayer.texture.height)
            {
                aspect = (float)this.videoPlayer.texture.width / (float)this.videoPlayer.texture.height;
                height = this.PlayerBG.sizeDelta.y;
                width = height * aspect;
            }
            else
            {
                aspect = 1;
                height = this.PlayerBG.sizeDelta.y;
                width = height;
            }
            // Debug.Log(aspect);
            this.VideoRect.sizeDelta = new Vector2(width, height);
        }
        //
        public void OnSliderBeginDrag()
        {
            this.IsSliding = true;
            isSeeking = true;
        }
        public void OnSliderEndDrag()
        {
            this.videoPlayer.frame = (long)(this.sliderVideo.value * this.videoPlayer.frameCount);
            this.IsSliding = false;
            isSeeking = true;
        }
        //
        #region Video player setup
        void VideoPlayerSetup()
        {
            this.videoPlayer.SetTargetAudioSource(0, this.audiosource);
            this.videoPlayer.aspectRatio = VideoAspectRatio.FitInside;
        }
        #endregion
        //
        #region Buttons Functions
        void BindButtons()
        {
            this.BtnPlay.onClick.AddListener(BtnPlayClick);
            this.BtnFullScreen.onClick.AddListener(BtnFullScreenClick);
        }
        //
        private void BtnPlayClick()
        {
            if (this.videoPlayer.isPrepared)
            {
                if (this.videoPlayer.isPlaying)
                {
                    this.videoPlayer.Pause();
                    this.BtnPlay.GetComponent<Image>().sprite = this.SpriteBtnPlay;
                }
                else
                {
                    this.videoPlayer.Play();
                    this.BtnPlay.GetComponent<Image>().sprite = this.SpriteBtnPause;
                }
            }
        }
        //
        private void BtnFullScreenClick()
        {
            this.IsFullscreen = !this.IsFullscreen;
            if (this.IsFullscreen)
            {
                float posY = 0;
                this.ToolBar.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, posY);
                PlayerBG.transform.localEulerAngles = new Vector3(PlayerBG.transform.localEulerAngles.x, PlayerBG.transform.localEulerAngles.y, 90);
                PlayerBG.sizeDelta = new Vector2(ScreenHeight, ScreenWidth);
                this.BtnFullScreen.GetComponent<Image>().sprite = this.SpriteBtnFullScreenMin;
                this.ContentContainer.SetActive(false);
            }
            else
            {
                float posY = -this.ToolBar.GetComponent<RectTransform>().sizeDelta.y;
                this.ToolBar.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -this.ToolBar.GetComponent<RectTransform>().sizeDelta.y);
                PlayerBG.transform.localEulerAngles = new Vector3(PlayerBG.transform.localEulerAngles.x, PlayerBG.transform.localEulerAngles.y, 0);
                PlayerBG.sizeDelta = new Vector2(ScreenWidth, 700);
                this.BtnFullScreen.GetComponent<Image>().sprite = this.SpriteBtnFullScreenMax;
                this.ContentContainer.SetActive(true);
            }
            //this.ScaleVideo();
        }
        #endregion

        private void VideoPlayer_errorReceived(VideoPlayer source, string message)
        {
            this.ImgErrorMessage.SetActive(true);
            if(prepareVideoRoutine != null) {
                StopCoroutine(prepareVideoRoutine);
                prepareVideoRoutine = null;
            }
            this.videoPlayer.errorReceived -= VideoPlayer_errorReceived;//Unregister to avoid memory leaks
        }

    }
}
