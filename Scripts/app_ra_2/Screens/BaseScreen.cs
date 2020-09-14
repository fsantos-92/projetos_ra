using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;

namespace App.Screens
{
    public class BaseScreen : MonoBehaviour
    {
        public virtual void SetButtonsInteractions(bool state)
        {
            Button[] btns = GetComponentsInChildren<Button>();
            foreach(Button element in btns)
            {
                element.interactable = state;
            }
        }

        public void StopCoroutines()
        {
            StopAllCoroutines();
        }

        protected virtual IEnumerator AnimPos(GameObject objToMove, Vector3 target, float waitTime = 0, bool hideScreen = false)
        {
            yield return new WaitForSeconds(waitTime);
            for (; Vector3.Distance(objToMove.GetComponent<RectTransform>().transform.position, target) > 0.1f || Vector3.Distance(objToMove.GetComponent<RectTransform>().transform.position, target) < -0.1f;)
            {
                objToMove.GetComponent<RectTransform>().transform.position = Vector3.Lerp(objToMove.GetComponent<RectTransform>().transform.position, target, 4f * Time.deltaTime);
                yield return null;
            }
            if (hideScreen) this.gameObject.SetActive(false);
        }
        protected virtual IEnumerator ScaleObject(GameObject objToScale, float scale, float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            for (; Mathf.Abs(objToScale.GetComponent<RectTransform>().transform.localScale.x - scale) > 0;)
            {
                objToScale.GetComponent<RectTransform>().transform.localScale = new Vector3(Mathf.MoveTowards(objToScale.GetComponent<RectTransform>().transform.localScale.x, scale, Time.deltaTime * 1), Mathf.MoveTowards(objToScale.GetComponent<RectTransform>().transform.localScale.y, scale, Time.deltaTime * 1), Mathf.MoveTowards(objToScale.GetComponent<RectTransform>().transform.localScale.z, scale, Time.deltaTime * 1));
                yield return null;
            }
        }

        protected virtual IEnumerator fade(GameObject objToChange, float startAlpha, float endAlpha, bool hideScreen = false)
        {
            for (; Mathf.Abs(objToChange.GetComponent<CanvasGroup>().alpha - endAlpha) > 0;)
            {
                objToChange.GetComponent<CanvasGroup>().alpha = Mathf.MoveTowards(objToChange.GetComponent<CanvasGroup>().alpha, endAlpha, Time.deltaTime * 2);
                yield return null;
            }
            if (hideScreen) this.gameObject.SetActive(false);
        }
    }
}