using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace App.Buttons
{
    public class MissionButtons : MonoBehaviour
    {
        bool CanInteract = true;
        bool MissionComplete = false;

        public GameObject Position;

        Button Btn;

        AudioSource audioSource;
        public AudioClip SfxButton;
        // Start is called before the first frame update
        void Awake()
        {
            this.Btn = this.GetComponent<Button>();
            this.Btn.onClick.AddListener(BtnClick);
            this.audioSource = GetComponent<AudioSource>();
            this.Btn.interactable = false;
        }

        private void BtnClick()
        {
            this.audioSource.PlayOneShot(SfxButton);
        }

        public void SetInteraction(bool canInteract)
        {
            this.CanInteract = canInteract;
            if (!this.CanInteract)
            {
                this.GetComponent<Button>().interactable = false;
            }
        }
        void OnEnable()
        {
            StartCoroutine(movePos(this.gameObject, this.Position.GetComponent<RectTransform>().transform.position, this.Btn));
        }

        IEnumerator movePos(GameObject toMove, Vector3 target, Button btn)
        {
            yield return new WaitForSeconds(0.1f);
            for (; Vector3.Distance(toMove.GetComponent<RectTransform>().transform.position, target) > 0.5f || Vector3.Distance(toMove.GetComponent<RectTransform>().transform.position, target) < -0.5f;)
            {
                toMove.GetComponent<RectTransform>().transform.position = Vector3.Lerp(toMove.GetComponent<RectTransform>().transform.position, target, 8f * Time.deltaTime);
                yield return null;
            }
            if (this.CanInteract) btn.interactable = true;
        }
    }
}
