using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;
#if UNITY_ANDROID
using UnityEngine.Android;
#elif UNITY_IOS
using UnityEngine.iOS;
#endif

public class Intro : MonoBehaviour
{
    public GameObject PlayScreen;

    [SerializeField]
    RectTransform logoRect;

    [SerializeField]
    RectTransform[] logoTop;
    [SerializeField]
    RectTransform[] logoCenter;
    [SerializeField]
    RectTransform[] logoBottom;
    
    RectTransform[] elementsBg;

    [SerializeField]
    RectTransform elementsBgParent;

    [SerializeField]
    AudioSource loopAudioSource;

    CanvasGroup logoCanvas;

    CanvasGroup canvasGroup;

    bool animLogoRoutineFinished;

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    IEnumerator Start()
    {

        VuforiaRuntime.Instance.InitVuforia();

		logoCanvas = logoRect.GetComponent<CanvasGroup>();

		canvasGroup = GetComponent<CanvasGroup>();

		elementsBg = elementsBgParent.GetComponentsInChildren<RectTransform>();


		foreach (var item in logoTop)
		{
			item.localScale = Vector3.zero;
		}

		foreach (var item in logoCenter)
		{
			item.localScale = Vector3.zero;
		}

		foreach (var item in logoBottom)
		{
			item.localScale = Vector3.zero;
		}

		logoRect.anchoredPosition = Vector2.zero;

		//Anima os objetos de fundo
		foreach (var item in elementsBg)
        {
            if (item.Equals(elementsBgParent))
                continue;

            LeanTween.rotate(item.gameObject, item.eulerAngles + new Vector3(0, 0, 15), 1.2f).setEase(LeanTweenType.easeInOutSine).setLoopPingPong();
        }

        //Aguarda a confirmação do aviso RA
        while (!AvisoRa.Confirmed)
        {
            yield return new WaitForEndOfFrame();
        }

        while (!CamPermission())
        {
            yield return new WaitForSeconds(1);
        }

        loopAudioSource.Play();

        StartCoroutine(AnimLogo());

        while (!animLogoRoutineFinished)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ActivatePlayScreen();
                StopAllCoroutines();
            }
            yield return null;
        }

        yield return new WaitForSeconds(2);

        LeanTween.move(logoRect, new Vector2(0, 343.85f), 0.3f);

        yield return new WaitForSeconds(1);

        PlayScreen.SetActive(true);

        yield return new WaitForSeconds(1);

        canvasGroup.alpha = 0;

        yield return null;
    }

    void ActivatePlayScreen()
    {
        LeanTween.move(logoRect, new Vector2(0, 343.85f), 0.3f).setOnComplete(() => {
            PlayScreen.SetActive(true);
        });

        LeanTween.alphaCanvas(canvasGroup, 0, 0.3f).setDelay(0.5f);

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
#else
        return true;
#endif

    }

    IEnumerator AnimLogo()
    {
        
        yield return new WaitForSeconds(1.3f);

        for (int i = 0; i < logoTop.Length; i++)
        {
            var tween = LeanTween.scale(logoTop[i].gameObject, Vector3.one, 0.4f).setDelay(i*0.2f).setEase(LeanTweenType.easeOutBack);
        }

        yield return new WaitForSeconds(logoCenter.Length * 0.4f);

        for (int i = 0; i < logoCenter.Length; i++)
        {
            var tween = LeanTween.scale(logoCenter[i].gameObject, Vector3.one, 0.4f).setDelay(i * 0.1f).setEase(LeanTweenType.easeOutBack);
        }

        yield return new WaitForSeconds(logoCenter.Length * 0.2f);

        for (int i = 0; i < logoBottom.Length; i++)
        {
            var tween = LeanTween.scale(logoBottom[i].gameObject, Vector3.one, 0.4f).setDelay(i * 0.1f).setEase(LeanTweenType.easeOutElastic);

            if (i == logoBottom.Length - 1)
            {
                tween.setOnComplete(() => animLogoRoutineFinished = true);
            }
        }

        
    }

}
