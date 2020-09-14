using UnityEngine;

namespace App.Tools
{
    public class ModelRotation : Swipe
    {
        protected void Update()
        {

            if (IsDoubleTap())
            {
                Debug.Log("Double Tap");
                reset();
            }

            Vector2 currentSwipe = SwipeValue();
            float a = GetComponent<Transform>().rotation.x;
            a = currentSwipe.x * SwipeSensibility;
            float b = GetComponent<Transform>().rotation.y;
            b = currentSwipe.y * SwipeSensibility;
            Vector3 rotation = new Vector3(-b, 0, a);
            transform.Rotate(rotation, Space.World);
        }
        void reset()
        {
            Vector3 pos = Vector3.zero;
            Vector3 scale = Vector3.one;
            Vector3 rotation = Vector3.zero;
            iTween.MoveTo(this.gameObject, iTween.Hash("position", pos, "time", 0.5f, "isLocal", true, "easetype", iTween.EaseType.easeInOutCirc));
            iTween.ScaleTo(this.gameObject, iTween.Hash("scale", scale, "time", 0.5f, "isLocal", true, "easetype", iTween.EaseType.easeInOutCirc));
            iTween.RotateTo(this.gameObject, iTween.Hash("rotation", pos, "time", 0.5f, "isLocal", true, "easetype", iTween.EaseType.easeInOutCirc));
        }
    }
}
