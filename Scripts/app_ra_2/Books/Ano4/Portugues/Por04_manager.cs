using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Por04_manager : BaseBookManager, IClickInteraction
{
    private GameObject uau;
    private GameObject thought;
    private GameObject bum;

    public override void StartBook()
    {
        base.StartBook();

        foreach ( Transform element in transform.GetComponentsInChildren<Transform>() )
        {
            if ( element.name == ( "Uau_balloon" ) )
                uau = element.gameObject;
            if ( element.name == ( "Thought_balloon" ) )
                thought = element.gameObject;
            if ( element.name == ( "Bum" ) )
                bum = element.gameObject;
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
                if ( !_missionsCleared[0] && obj == uau && activityUnlocked[0] )
                {
                    _isAnimating = true;
                    objAnim.SetTrigger( "Play" );
                    objAudio.Play();
                    ClearMission( 1 );
                }
                else if ( !_missionsCleared[1] && obj == thought && activityUnlocked[1] )
                {
                    _isAnimating = true;

                    objAnim.SetTrigger( "Play" );
                    objAudio.Play();
                    ClearMission( 2 );
                }
                else if ( !_missionsCleared[2] && obj == bum && activityUnlocked[2] )
                {
                    _isAnimating = true;

                    objAnim.SetTrigger( "Play" );
                    objAudio.Play();
                    ClearMission( 3 );
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