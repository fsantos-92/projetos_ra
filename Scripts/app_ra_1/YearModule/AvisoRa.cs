using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AvisoRa : MonoBehaviour
{

    private CanvasGroup canvasGroup;
    [SerializeField]
    private CanvasGroup canvasGroupBG;
    [SerializeField]
    private RectTransform rectTransformDialog;
    [SerializeField]
    private Button buttonSeguir;

    public static bool Confirmed { get; private set; }

    // Start is called before the first frame update
    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 1;
        canvasGroupBG.alpha = 0;
        rectTransformDialog.localScale = Vector3.zero;
        buttonSeguir.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        canvasGroup.blocksRaycasts = false;
        LeanTween.alphaCanvas(canvasGroup, 0, 0.3f).setOnComplete(() => { Confirmed = true; Destroy(gameObject);});
    }

    void Start()
    {
        var seq = LeanTween.sequence();
        seq.append(1f);
        seq.append(LeanTween.scale(rectTransformDialog, Vector3.one, 1.7f).setEase(LeanTweenType.easeOutElastic));

        LeanTween.alphaCanvas(canvasGroupBG, 1, 0.5f);
    }
}
