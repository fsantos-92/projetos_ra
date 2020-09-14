using System.Collections;
using UnityEngine;
#if UNITY_ANDROID
using UnityEngine.Android;
#elif UNITY_IOS
using UnityEngine.iOS;
#endif
using Vuforia;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace App.Game
{
    public class Loader : MonoBehaviour
    {

        [SerializeField] Text txtTitle;
        [SerializeField] Text txtMessage;
        [SerializeField] RectTransform Warning;
        [SerializeField] Button btnNext;

        void Start()
        {
            Warning.transform.localScale = Vector3.zero;
            btnNext.interactable = false;
            txtTitle.text = "ATENÇÃO!";
            txtMessage.text = "O aplicativo necessita da permissão da câmera para funcionar!";
            btnNext.onClick.AddListener(BtnNextClick);
            StartCoroutine(delayLoad());
        }

        private void BtnNextClick()
        {
            btnNext.interactable = false;
            // esconder caixa de aviso e carregar o jogo
            LeanTween.scale(Warning, Vector3.zero, 0.5f).setEase(LeanTweenType.easeInOutBack).setOnComplete(() =>
            {
                txtTitle.text = "Carregando...";
                txtMessage.text = "Isso pode levar alguns segundos";
                // this.ScreenWarning.SetActive(false);
                VuforiaRuntime.Instance.InitVuforia();

                SceneManager.LoadSceneAsync(2);
            });
        }

        IEnumerator delayLoad()
        {
            // this.ScreenWarning.SetActive(true);
            yield return new WaitForSeconds(0.2f);

            if (!CamPermission()) StartCoroutine(delayLoad());
            else
            {
                // Mostrar caixa de aviso
                LeanTween.scale(Warning, Vector3.one, 0.5f).setDelay(0.5f).setEase(LeanTweenType.easeInOutBack).setOnComplete(() => { btnNext.interactable = true; });
            }
        }
        bool CamPermission()
        {
#if UNITY_ANDROID
            if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
            {
                Permission.RequestUserPermission(Permission.Camera);
                return false;
            }
            else return true;
#elif UNITY_IOS
            if (!Application.HasUserAuthorization(UserAuthorization.WebCam))
            {
                Application.RequestUserAuthorization(UserAuthorization.WebCam);
                return false;
            } 
			else return true;
#endif
            return true;
        }
    }

}