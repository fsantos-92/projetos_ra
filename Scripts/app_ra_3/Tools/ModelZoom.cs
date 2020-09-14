using UnityEngine;
using System.Collections;

namespace App.Tools
{
    public class ModelZoom : PinchZoom
    {
        [SerializeField] GameObject RecoTarget;
        protected void Update()
        {
            if (IsDoubleTap())
            {
                Debug.Log("Double Tap");
                reset();
            }
        }

        void reset()
        {
            // Fazer animações com o LeanTween, itween não move localmente
            Vector3 pos = Vector3.zero;
            Vector3 scale = Vector3.one;
            Vector3 rotation = Vector3.zero;
            // this.transform.localPosition = pos;
            // this.transform.localScale = scale;
            // this.transform.localEulerAngles = rotation;
            iTween.MoveTo(this.gameObject, iTween.Hash("position", pos, "time", 0.5f, "isLocal", true,"easetype", iTween.EaseType.easeInOutCirc));
            iTween.ScaleTo(this.gameObject, iTween.Hash("scale", scale, "time", 0.5f, "isLocal", true, "easetype", iTween.EaseType.easeInOutCirc));
            iTween.RotateTo(this.gameObject, iTween.Hash("rotation", pos, "time", 0.5f, "isLocal", true, "easetype", iTween.EaseType.easeInOutCirc));
        }
    }
}
