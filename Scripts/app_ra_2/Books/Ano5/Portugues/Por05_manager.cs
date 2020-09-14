using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Por05_manager :  BaseBookManager, IClickInteraction
{
    private GameObject cameraPhoto;
    private GameObject bike;
    private GameObject ball;

    public override void StartBook()
    {
        base.StartBook();

        foreach (Transform element in transform.GetComponentsInChildren<Transform>())
        {
            if (element.name == ("Camera"))
                cameraPhoto = element.gameObject;
            if (element.name == ("Bike"))
                bike = element.gameObject;
            if (element.name == ("Ball"))
                ball = element.gameObject;
        }
    }

    public void ClickInteraction( GameObject obj )
    {
        if ( _isAnimating ) return;

        Animator objAnim = obj.GetComponent<Animator>();
        AudioSource objAudio = obj.GetComponent<AudioSource>();

        if ( !IsFinished() )
        {
            if ( objAnim.GetCurrentAnimatorStateInfo( 0 ).IsName( "Idle" ) )
            {
                 if ( !_missionsCleared[ 0 ]  && obj == cameraPhoto && activityUnlocked[0]  )
                {
                    _isAnimating = true;
                    objAnim.SetTrigger( "Play" );
                    objAudio.Play();ClearMission( 1 );
                }
                else if ( !_missionsCleared[ 1 ]  && obj == bike && activityUnlocked[1]  )
                {
                    _isAnimating = true;
                    objAnim.SetTrigger( "Play" );
                    objAudio.Play();ClearMission( 2 );
                }
                else if ( !_missionsCleared[ 2 ]  && obj == ball && activityUnlocked[2]   )
                {
                    _isAnimating = true;
                    objAnim.SetTrigger( "Play" );
                    objAudio.Play();ClearMission( 3 );
                }
            }
 
        }
        else
        {
            objAnim.SetTrigger( "Play" );
            objAudio.Play();
        }

    }
}