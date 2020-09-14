using UnityEngine;

namespace App.Tools
{

    public class ImageMovement : Swipe
    {
        bool canMove = false;
        float ScreenWidth;
        RectTransform rect;
        [SerializeField]
        float sensibility = 0.2f;
        void Awake()
        {
            this.ScreenWidth = Screen.width;
            rect = GetComponent<RectTransform>();
        }
        void Update()
        {
            if (!canMove)
                return;
            if (rect.sizeDelta.x > this.ScreenWidth || rect.sizeDelta.y > this.ScreenWidth)
            {
                Vector2 currentSwipe = SwipeValue();
                float PosX = -currentSwipe.x * this.sensibility;
                float PosY = -currentSwipe.y * this.sensibility;

                Vector2 translation = new Vector2(PosX, PosY);
                transform.Translate(translation, Space.Self);
            }
        }

        public void SetMovement()
        {
            this.canMove = !this.canMove;
        }

    }
}
