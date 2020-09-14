using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
#if UNITY_IOS
using UnityEngine.iOS;
#elif UNITY_ANDROID
using UnityEngine.Android;
#endif

public class Navigation : MonoBehaviour
{
    // public EventTrigger triggerNavigation;
    public CanvasGroup[] canGroups;
    public GameObject[] SelectionPos;
    public GameObject currentSelection;
    float distance;
    float ScreenWidth;
    public int start = 0;
    [SerializeField]
    float currentGroup;
    [SerializeField]
    float current;
    public int swipeSensibility = 15;
    public int changeSpeed = 5;

    private void Awake()
    {
        this.currentGroup = this.start;
        this.current = this.currentGroup;

        this.distance = this.SelectionPos[1].GetComponent<RectTransform>().transform.localPosition.x - this.SelectionPos[0].GetComponent<RectTransform>().transform.localPosition.x;
        this.ScreenWidth = Screen.width;

        int a = 1;
        if (this.canGroups.Length == 6) a = 6;
        if (this.canGroups.Length == 4) a = 5;
        if (this.canGroups.Length == 3) a = 4;
        this.swipeSensibility = (int)(Screen.width / 108) - a;
        if (this.swipeSensibility < 1)
            this.swipeSensibility = 1;

#if UNITY_IOS
        this.swipeSensibility = 1;
#endif

        ActiveGroup();
    }

    private void Update()
    {
        for (int i = 0; i < canGroups.Length; i++)
        {
            CanvasGroupPosition(this.canGroups[i], i);
        }
    }

    void OnEnable()
    {
        this.currentGroup = 0;
        this.current = this.currentGroup;
        ActiveGroup();
    }

    public void CanvasGroupPosition(CanvasGroup canGroup, int pos)
    {

        float form;
        if (current > pos) form = pos - current + 1;
        else form = current - pos + 1;

        float offset = 0;
        if (this.canGroups.Length == 6) offset = 4;
        if (this.canGroups.Length == 4) offset = 2;
        if (this.canGroups.Length == 3) offset = 1;
        this.currentSelection.GetComponent<RectTransform>().transform.localPosition = new Vector3((form + offset) * this.distance, 0, 0);
        float test = this.currentSelection.GetComponent<RectTransform>().transform.localPosition.x / this.distance;

        this.current = Mathf.Lerp(this.current,
        this.currentGroup, Time.smoothDeltaTime * changeSpeed);

        float x = ((pos - this.current) * this.ScreenWidth) + (this.ScreenWidth * 0.5f);

        canGroup.GetComponent<RectTransform>().transform.position = new Vector2(x, Screen.height * 0.5f);
    }

    public void Swipe(Vector2 currentSwipe)
    {
#if UNITY_ANDROID
        var sensibility = this.swipeSensibility * 100;
#elif UNITY_IOS
        var sensibility = this.swipeSensibility * 1100;
        if (this.canGroups.Length == 3) sensibility = this.swipeSensibility * 1120;
        if(this.canGroups.Length == 6) sensibility = this.swipeSensibility * 1250;
#endif
        this.current = currentGroup - currentSwipe.x / sensibility;

        if (this.current < -0.2f) this.current = -0.2f;
        if (this.current > this.canGroups.Length - 0.8f) this.current = this.canGroups.Length - 0.8f;
    }

    void ActiveGroup()
    {
        for (int i = 0; i < canGroups.Length; i++)
        {
            float x = (i - ((int)this.current)) * Screen.width;
            this.canGroups[i].GetComponent<RectTransform>().transform.localPosition = new Vector2(x, 0);
        }
    }

    public void TouchUp()
    {
        if (this.current < this.currentGroup)
        {
            if (Mathf.Abs(this.currentGroup - this.current) < 0.2f)
                return;
            this.current = this.current - 0.1f;
        }
        else
        {
            if (Mathf.Abs(this.currentGroup - this.current) < 0.2f)
                return;
            this.current = this.current + 0.1f;
        }
        StopAllCoroutines();
        if (this.gameObject.activeInHierarchy)
            StartCoroutine(Number());
    }

    IEnumerator Number()
    {
        int a = Convert.ToInt32(this.current);
        while (this.currentGroup != a)
        {
            this.currentGroup = Mathf.MoveTowards(this.current, a, Time.smoothDeltaTime * 10);
            yield return null;
        }
    }
}
