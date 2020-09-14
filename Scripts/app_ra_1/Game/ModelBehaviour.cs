using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace App.Game
{

    public class ModelBehaviour : MonoBehaviour
    {
        Animator animator;
        bool CanPlay = true;
        [SerializeField]
        AudioClip ClipInteraction;
        // Start is called before the first frame update
        void Start()
        {
            animator = GetComponent<Animator>();
        }

        public void PlayAnim()
        {
            if (CanPlay)
            {
                animator.SetTrigger("Play");
                if(this.ClipInteraction)
                    SoundManager.Instance.PlayClip(ClipInteraction);
                CanPlay = false;
                StartCoroutine(ResetAnim(2));
            }
        }
        IEnumerator ResetAnim(float waitTime)
        {
            yield return new WaitForSeconds(waitTime + 1);
            CanPlay = true;
        }
    }
}
