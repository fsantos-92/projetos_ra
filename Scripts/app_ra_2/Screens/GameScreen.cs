using UnityEngine;
using UnityEngine.UI;
using Vuforia;
using System.Collections;

namespace App.Screens
{

    public class GameScreen : BaseScreen
    {
        [SerializeField] Button BtnBack;

        [SerializeField] GameObject ScreenMenu;

        public RectTransform ImgFtdLogo;
        public RectTransform FtdLogoPosSide;
        public RectTransform FtdLogoPosOriginal;

        [SerializeField]
        VuforiaBehaviour ARCamera;

        public AudioClip SfxButton;
        AudioSource audioSource;

        bool _inGame;

        [SerializeField] private GameObject[] _managers;


        // Start is called before the first frame update
        void Start()
        {
            _inGame = false;
            this.BtnBack.onClick.AddListener(BtnBackClick);
            this.audioSource = GetComponent<AudioSource>();
            this.gameObject.GetComponent<RectTransform>().transform.position = new Vector3(this.gameObject.GetComponent<RectTransform>().transform.position.x + Screen.width, this.gameObject.GetComponent<RectTransform>().transform.position.y, this.gameObject.GetComponent<RectTransform>().transform.position.z);
        }

        void Update()
        {
            if (_inGame && Input.GetKeyDown(KeyCode.Escape))
            {
                BtnBackClick();
            }
        }

        void OnEnable()
        {
            SetButtonsInteractions(true);
        }

        public void StartGame()
        {
            _inGame = true;
        }

        private void BtnBackClick()
        {
            this.ScreenMenu.SetActive(true);
            this.ScreenMenu.GetComponent<MenuScreen>().StopCoroutines();
            this.ScreenMenu.GetComponent<MenuScreen>().SetButtonsInteractions(true);

            LeanTween.moveX(gameObject.GetComponent<RectTransform>(), 1920, 0.8f).setDelay(0.2f).setEase(LeanTweenType.easeOutCirc);
            LeanTween.moveX(ScreenMenu.GetComponent<RectTransform>(), 0, 0.8f).setDelay(0.2f).setEase(LeanTweenType.easeOutCirc);
            LeanTween.move(ImgFtdLogo, FtdLogoPosOriginal.localPosition, 1.2f).setDelay(0.2f).setEase(LeanTweenType.easeOutCirc);
            LeanTween.scale(ImgFtdLogo, Vector3.one, 1.2f).setDelay(0.2f).setEase(LeanTweenType.easeOutCirc);

            Button[] btns = FindObjectsOfType<Button>();
            SetButtonsInteractions(false);
            this.audioSource.PlayOneShot(SfxButton);
            Invoke("setCameraActive", 1);
            _inGame = false;

            StartCoroutine(ResetManagers());
            //             foreach ( var manager in _managers )
            //             {
            // //                Debug.Log(manager +" --- "+ manager.name + " === " + manager.GetComponent( manager.name ) );
            //                 manager.SetActive( true );
            //                 manager.GetComponent( manager.name ).SendMessage( "ResetProgress" );
            //                 manager.SetActive( false );
            //             }

            // transform.Find("Group_Btn_Missions").gameObject.SetActive(false);
            // transform.Find("Mission_Text/Mission_BG").GetComponent<Animator>().SetTrigger("Reset");
        }

        IEnumerator ResetManagers()
        {
            foreach (var manager in _managers)
            {
                //                Debug.Log(manager +" --- "+ manager.name + " === " + manager.GetComponent( manager.name ) );
                manager.SetActive(true);
                manager.GetComponent(manager.name).SendMessage("ResetProgress");
                manager.SetActive(false);
            }

			transform.Find("Group_Btn_Missions").gameObject.SetActive(false);
            transform.Find("Mission_Text/Mission_BG").GetComponent<Animator>().SetTrigger("Reset");
            yield return null;
        }

        void setCameraActive()
        {
            // this.ARCamera.enabled = !this.ARCamera.isActiveAndEnabled;
        }
    }
}
