using System.Collections;
using UnityEngine;

namespace App.Screens
{
    public class BaseScreen : MonoBehaviour
    {
        protected virtual void OnEnable()
        {
            var cg = GetComponent<CanvasGroup>();
            cg.alpha = 0;
            fade(cg, 0, 1, false);
        }

        public virtual void Disable()
        {
            fade(gameObject.GetComponent<CanvasGroup>(), 1, 0, true);
        }

        protected void fade(CanvasGroup objToFade, float startAlpha, float endAlpha, bool hide = false)
        {
            LeanTween.alphaCanvas(objToFade, endAlpha, 0.3f).setOnComplete(() => { if (hide) this.gameObject.SetActive(false); });
        }

        protected void AnimPos(RectTransform objToMove, Vector3 targetPosition, float time = 0.3f)
        {
            LeanTween.move(objToMove, targetPosition, time).setEase(LeanTweenType.easeOutSine);
            //iTween.MoveTo(objToMove.gameObject, iTween.Hash("position", TargetPosition, "time", time, "easetype", iTween.EaseType.easeOutSine));
        }

    }
}
