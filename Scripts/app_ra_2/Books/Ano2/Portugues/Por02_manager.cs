using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking.Match;
using UnityEngine.UI;

public class Por02_manager : BaseBookManager, IClickInteraction
{
    private GameObject _letterGirl;
    private GameObject _pinkRobot;
    private GameObject _hologramRobot;

    public override void StartBook()
    {
        base.StartBook();


        foreach ( Transform element in transform.GetComponentsInChildren<Transform>() )
        {
            if ( element.name == ( "GirlLetter" ) )
                _letterGirl = element.gameObject;
            if ( element.name == ( "PinkRobot" ) )
                _pinkRobot = element.gameObject;
            if ( element.name == ( "HologramRobot" ) )
                _hologramRobot = element.gameObject;
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
                _isAnimating = true;

                if ( !_missionsCleared[0] && obj == _letterGirl && activityUnlocked[0] )
                {
                    PlayAnimation();

                    ClearMission( 1 );
                }

                if ( !_missionsCleared[1] && obj == _pinkRobot && activityUnlocked[1] )
                {
                    PlayAnimation();

                    ClearMission( 2 );
                }
                else if ( !_missionsCleared[2] && obj == _hologramRobot && activityUnlocked[2] )
                {
                    PlayAnimation();

                    ClearMission( 3 );
                }
            }
        }
        else
        {
            if ( obj != _pinkRobot )
            {
                PlayAnimation();
            }
        }

        void PlayAnimation()
        {
            objAnim.SetTrigger( "Play" );
            objAudio.Play();
        }
    }
}