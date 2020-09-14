using System.Collections;
using UnityEngine;
#if UNITY_ANDROID
using UnityEngine.Android;
#elif UNITY_IOS
using UnityEngine.iOS;
#endif
using Vuforia;
using UnityEngine.SceneManagement;

namespace App.Game
{
    public class Loader : MonoBehaviour
    {
        void Start()
        {
            StartCoroutine(delayLoad());
        }

        IEnumerator delayLoad()
        {
            yield return new WaitForSeconds(0.2f);

            if (!CamPermission()) StartCoroutine(delayLoad());
            else
            {
                VuforiaRuntime.Instance.InitVuforia();
                SceneManager.LoadSceneAsync("Game");
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