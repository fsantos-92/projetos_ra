using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Geo01_manager : BaseBookManager, IClickInteraction
{
    private GameObject blueBalloon_A;
    private GameObject blueBalloon_B;
    private GameObject flash;
    private GameObject popcorn;

    private GameObject[] _blueBalloonsClicked;
    private int _balloonCounter;

    public override void StartBook()
    {
        base.StartBook();

        foreach ( Transform element in transform.GetComponentsInChildren<Transform>() )
        {
            if ( element.name == ( "BlueBalloon_A" ) )
                blueBalloon_A = element.gameObject;
            if ( element.name == ( "BlueBalloon_B" ) )
                blueBalloon_B = element.gameObject;
            if ( element.name == ( "Flash" ) )
                flash = element.gameObject;
            if ( element.name == ( "Popcorn" ) )
                popcorn = element.gameObject;
        }
    }

    protected override void InitializeVariables()
    {
        base.InitializeVariables();

        _blueBalloonsClicked = new[] {new GameObject(), new GameObject()};
        _balloonCounter = 0;
    }

    public void ClickInteraction( GameObject obj )
    {
        if ( _isAnimating ) return;

        string objName = obj.name;
        Animator objAnim = obj.GetComponent<Animator>();

        if ( objAnim.GetCurrentAnimatorStateInfo( 0 ).IsName( "Idle" ) )
        {
            if ( !IsFinished() )
            {
                if ( !_missionsCleared[0] && objName.Contains( "BlueBalloon" ) &&
                     !IsInArray( obj, _blueBalloonsClicked ) && activityUnlocked[0] )
                {
                    objAnim.SetTrigger( "Play" );

                    _blueBalloonsClicked[_balloonCounter] = obj;
                    _balloonCounter++;

                    if ( _balloonCounter >= _blueBalloonsClicked.Length )
                    {
                        ClearMission( 1 );
                    }
                }
                else if ( !_missionsCleared[1] && obj == flash && activityUnlocked[1]  )
                {
                    objAnim.SetTrigger( "Play" );
                    ClearMission( 2 );
                }
                else if ( !_missionsCleared[2] && obj == popcorn && activityUnlocked[2]  )
                {
                    objAnim.SetTrigger( "Play" );
                    ClearMission( 3 );
                }
            }
            else
            {
                objAnim.SetTrigger( "Play" );
            }
        }
    }

    protected override void ResetProgress()
    {
        base.ResetProgress();

        InitializeVariables();
    }
}