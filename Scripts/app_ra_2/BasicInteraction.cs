using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace App.Interactions
{
    public class BasicInteraction : MonoBehaviour
    {
        protected Animator animator;
        protected AudioSource[] audioSource;

        protected bool CanAnimate = true;

        // Start is called before the first frame update
        protected virtual void Start()
        {
            animator = GetComponent<Animator>();
            audioSource = GetComponents<AudioSource>();
        }

        public virtual void PlayAnim()
        {
            if (!CanAnimate)
                return;

            animator.SetTrigger("Play");
            if (audioSource.Length > 0)
                foreach (AudioSource element in audioSource)
                {
                  element.Play();
                }
            CanAnimate = false;
            Invoke("ResetAnim", 2);
        }

        protected void ResetAnim()
        {
            CanAnimate = true;
        }
    }
}