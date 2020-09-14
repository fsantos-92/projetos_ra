using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace App.Buttons
{
    public class MissionInfoButton : MonoBehaviour
    {
        [SerializeField]
        Button[] MissionButtons;

        Button Btn;

        AudioSource audioSource;

        public AudioClip SfxButton;
        // Start is called before the first frame update
        void Start()
        {
            this.Btn = GetComponent<Button>();
            audioSource = GetComponent<AudioSource>();
            this.Btn.onClick.AddListener(BtnClick);
        }

        private void BtnClick()
        {
            audioSource.PlayOneShot(SfxButton);
            this.Btn.interactable = false;
            foreach (Button element in MissionButtons)
            {
                if (element.gameObject.activeInHierarchy)
                {
                    element.interactable = false;
                    Vector3 target = this.transform.position;
                    StartCoroutine(movePos(element.gameObject, target));
                    StartCoroutine(fade(element.gameObject, 1, 0));
                }
                else
                {
                    element.gameObject.SetActive(true);
                    element.GetComponent<CanvasGroup>().alpha = 1;
                    this.Btn.interactable = true;
                }
            }
        }
        IEnumerator movePos(GameObject toMove, Vector3 target)
        {
            yield return new WaitForSeconds(0.1f);
            for (; Vector3.Distance(toMove.GetComponent<RectTransform>().transform.position, target) > 0.5f || Vector3.Distance(toMove.GetComponent<RectTransform>().transform.position, target) < -0.5f;)
            {
                toMove.GetComponent<RectTransform>().transform.position = Vector3.Lerp(toMove.GetComponent<RectTransform>().transform.position, target, 8f * Time.deltaTime);
                yield return null;
            }
            toMove.SetActive(false);
        }

        IEnumerator fade(GameObject objToChange, float startAlpha, float endAlpha)
        {
            for (; Mathf.Abs(objToChange.GetComponent<CanvasGroup>().alpha - endAlpha) > 0;)
            {
                objToChange.GetComponent<CanvasGroup>().alpha = Mathf.MoveTowards(objToChange.GetComponent<CanvasGroup>().alpha, endAlpha, Time.deltaTime * 2);
                yield return null;
            }
            this.Btn.interactable = true;
        }
    }
}
