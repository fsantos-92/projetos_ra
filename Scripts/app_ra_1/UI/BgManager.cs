using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BgManager : MonoBehaviour
{
    private static BgManager instance;
    public static BgManager Instance { get { return instance; } }

    [SerializeField]
    Image image;

    [SerializeField]
    RectTransform icons;

    CanvasGroup canvasGroupIcons;

    CanvasGroup canvasGroup;

    [SerializeField]
    Material materialDpriteColorized;

    [SerializeField]
    Color[] bgColors;

    RectTransform rectTransform;

    private void OnEnable()
    {
        if (!instance)
            instance = this;
    }

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroupIcons = icons.GetComponent<CanvasGroup>();
        if (!instance)
            instance = this;
        rectTransform = GetComponent<RectTransform>();

        materialDpriteColorized.SetColor("_Color", new Color(1,1,1,0));

        canvasGroupIcons.alpha = 1;


    }

    /// <summary>
    /// Altera o canvas group dos icones de fundo
    /// </summary>
    /// <param name="to"></param>
    public void SetIconsCanvasAlpha(float to)
    {
        LeanTween.alphaCanvas(canvasGroupIcons, to, 0.2f);
    }

    /// <summary>
    /// Altera a cor do BG principal
    /// </summary>
    /// <param name="to"></param>
    public void SetBgColor(BgColorsEnum bgColor)
    {
        
        LeanTween.cancel(rectTransform);

        LeanTween.alphaCanvas(canvasGroup, 1, 0.3f);

        Color c;

        switch (bgColor)
        {
            case BgColorsEnum.RED:
                c = bgColors[2];
                break;
            case BgColorsEnum.WHITE:
                c = bgColors[0];
                break;
            case BgColorsEnum.YELLOW:
                c = bgColors[1];
                break;
            default:
                c = Color.white;
                break;
        }

        LeanTween.color(rectTransform, c, 0.3f);
    }

    public void SetAllIconsWhiteColor(bool white)
    {
        StartCoroutine(AllIconsWhiteCo(white));
    }


    IEnumerator AllIconsWhiteCo(bool white)
    {
        Color col = materialDpriteColorized.GetColor("_Color");
        float a = materialDpriteColorized.GetColor("_Color").a;

        if (white)
        {
            while (col.a < 1)
            {
                col.a = Mathf.Clamp01(col.a += Time.deltaTime * 2f);
                materialDpriteColorized.SetColor("_Color", col);
                yield return null;
            }
        }
        else
        {
            while (col.a > 0)
            {
                
                col.a = Mathf.Clamp01(col.a -= Time.deltaTime * 2f);
                materialDpriteColorized.SetColor("_Color", col);
                yield return null;
            }
        }

        yield return null;
    }

    /// <summary>
    /// Seta o BG e ícones para transparente
    /// </summary>
    /// <param name="isTransparent"></param>
    public void SetBgTransparent(bool isTransparent)
    {
        LeanTween.alphaCanvas(canvasGroup, isTransparent ? 0 : 1, 0.3f);
    }

    private void OnApplicationQuit()
    {
        materialDpriteColorized.SetColor("_Color", Color.clear);
    }


}
