using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimSkipButton : MonoBehaviour
{
    float PosX = 0;
    [SerializeField]
    Image ImgCard;

    void Start()
    {
        this.GetComponent<Button>().onClick.AddListener(OnBtnClick);
    }

    private void OnBtnClick()
    {
        StopAllCoroutines();
    }

    void OnEnable()
    {
        PosX = -(this.GetComponent<RectTransform>().sizeDelta.x) * 0.15f;
        this.GetComponent<RectTransform>().transform.localPosition = new Vector2(-Screen.width * 0.5f, this.GetComponent<RectTransform>().transform.localPosition.y);
        StartCoroutine(AnimPos());
        StartCoroutine(ChangeScale());
    }

    IEnumerator AnimPos()
    {
        for (; Mathf.Abs(Vector2.Distance(this.GetComponent<RectTransform>().transform.localPosition, new Vector2(PosX, this.GetComponent<RectTransform>().transform.localPosition.y))) > 0.5f;)
        {
            this.GetComponent<RectTransform>().transform.localPosition = Vector3.Lerp(this.GetComponent<RectTransform>().transform.localPosition, new Vector2(PosX, this.GetComponent<RectTransform>().transform.localPosition.y), 5f * Time.deltaTime);
            yield return null;
        }
        yield return null;
    }
    IEnumerator ChangeScale()
    {
        for (; ImgCard.GetComponent<RectTransform>().localScale.x > 0.79f;)
        {
            this.ImgCard.GetComponent<RectTransform>().localScale = new Vector3(ImgCard.GetComponent<RectTransform>().localScale.x - 0.015f, ImgCard.GetComponent<RectTransform>().localScale.y - 0.015f, 1);
            yield return null;
        }
    }
}
