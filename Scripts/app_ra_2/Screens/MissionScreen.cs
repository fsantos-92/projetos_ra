using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace App.Screens
{
  public class MissionScreen : BaseScreen
  {
    [SerializeField] Button BtnClose;

    public AudioClip SfxButton;

    AudioSource audioSource;

    void Start( )
    {
      BtnClose.onClick.AddListener( BtnCloseClick );
    }

    public void BtnCloseClick( )
    {
      // this.audioSource.PlayOneShot( SfxButton );
      StartCoroutine( fade( this.gameObject, 1, 0, true ) );
    }

    void OnEnable( )
    {
      StartCoroutine( fade( this.gameObject, 0, 1 ) );
      // StartCoroutine(PlaySoundDelayed(1f));
    }
  }
}