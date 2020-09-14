using UnityEngine;

namespace App.Tools
{
    public class PinchZoom : MonoBehaviour
    {
        public float ZoomSpeed = 0.5f;
        public float MaxZoomValue = 2500f;
        public float MinZoomValue = 500f;

        float clicked = 0;
        float clicktime = 0;
        float clickdelay = 0.5f;

        protected float deltaMagnitudeDiff;

        protected static float PinchDiff()
        {
            // If there are two touches on the device...
            if (Input.touchCount == 2)
            {
                // Store both touches.
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                // Find the position in the previous frame of each touch.
                Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                // Find the magnitude of the vector (the distance) between the touches in each frame.
                float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

                // Find the difference in the distances between each frame.
                return prevTouchDeltaMag - touchDeltaMag;
            }
            else
                return 0;
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
