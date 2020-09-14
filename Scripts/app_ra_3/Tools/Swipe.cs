using UnityEngine;

namespace App.Tools
{
    public class Swipe : MonoBehaviour
    {
        public float SwipeSensibility = 50;

        float clicked = 0;
        float clicktime = 0;
        float clickdelay = 0.5f;
        protected static Vector2 SwipeValue()
        {
            // If there is one touch on the device...
            if (Input.touchCount == 1)
            {
                // Store touch.
                Touch touchZero = Input.GetTouch(0);

                // Find the position in the previous frame of the touch.
                Vector2 touchZeroPosition = touchZero.position;
                Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;

                // Find the difference in the distances between each frame.
                float diffX = touchZeroPrevPos.x - touchZeroPosition.x;
                float diffY = touchZeroPrevPos.y - touchZeroPosition.y;

                return new Vector2(diffX, diffY);
            }
            else
                return Vector2.zero;
        }
        protected static bool TouchUp()
        {
            if (Input.touchCount == 1)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Ended)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        protected virtual bool IsDoubleTap()
        {
            if (Input.GetMouseButtonDown(0))
            {
                clicked++;
                if (clicked == 1) clicktime = Time.time;
            }
            if (clicked > 1 && Time.time - clicktime < clickdelay)
            {
                clicked = 0;
                clicktime = 0;
                return true;
            }
            else if (clicked > 2 || Time.time - clicktime > 1) clicked = 0;
            return false;
        }
    }
}
