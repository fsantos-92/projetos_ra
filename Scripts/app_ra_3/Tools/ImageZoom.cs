using UnityEngine;
using UnityEngine.UI;

namespace App.Tools
{
    public class ImageZoom : PinchZoom
    {
        [SerializeField]
        Image img;

        float screenWidth;

        void Awake()
        {
            this.screenWidth = Screen.width;
            img.GetComponent<RectTransform>().sizeDelta = new Vector2(this.screenWidth, this.screenWidth);
        }
        protected void Update()
        {
            float diff = PinchDiff();
            float a = img.GetComponent<RectTransform>().sizeDelta.x;
            a -= diff * ZoomSpeed;

            if (a < MaxZoomValue && a > MinZoomValue)
                img.GetComponent<RectTransform>().sizeDelta = new Vector2(a, a);

            if (IsDoubleTap())
            {
                Debug.Log("Double Tap");
                img.GetComponent<RectTransform>().sizeDelta = new Vector2(this.screenWidth, this.screenWidth);
                img.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            }
        }
    }
}