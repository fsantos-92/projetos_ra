using System.Collections;
using System.Collections.Generic;
using App.Interactions;
using JetBrains.Annotations;
using UnityEngine;

public class Por03_playDarkLightObj : MonoBehaviour
{
    public GameObject litObj;
    public GameObject darkObj;
    protected AudioSource[] audioSource;

    protected virtual void Start()
    {
        audioSource = GetComponents<AudioSource>();
    }
    public void PlayAnim()
    {
        litObj.GetComponent<Animator>().SetTrigger( "Play" );
        darkObj.GetComponent<Animator>().SetTrigger( "Play" );

        if ( audioSource.Length > 0 )
        {
            foreach ( AudioSource element in audioSource )
            {
                element.Play();
            }
        }
    }
}