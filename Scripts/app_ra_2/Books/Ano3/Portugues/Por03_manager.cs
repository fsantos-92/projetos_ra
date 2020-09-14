using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking.Match;
using UnityEngine.UI;
using Vuforia;

public class Por03_manager : BaseBookManager, IClickInteraction
{
    private GameObject[] _birdsFound;
    private int _birdsCounter;

    private GameObject _lanternMask;

    protected override void Awake()
    {
        base.Awake();
        
//        foreach ( Transform element in transform.GetComponentsInChildren<Transform>() )
//        {
//            Debug.Log( element.name );
//            if ( element.name == "POR03_mask"  )
//                _lanternMask = element.gameObject;
//        }
//        
//        _lanternMask.transform.localPosition = new Vector3( 0, 0, -5 );
//        _lanternMask.transform.localScale = new Vector3( 0.3f, 0.3f, 0.3f );
    }

    protected override void InitializeVariables()
    {
        base.InitializeVariables();

        _birdsFound = new[] {new GameObject(), new GameObject()};
        _birdsCounter = 0;

    }

    public void ClickInteraction( GameObject obj )
    {
        string objName = obj.name;
        Animator objAnimLight = obj.GetComponent<Por03_playDarkLightObj>().litObj.GetComponent<Animator>();
//        Animator objAnimDark = obj.GetComponent<Por03_playDarkLightObj>().darkObj.GetComponent<Animator>();
//        AudioSource objAudio = obj.GetComponent<AudioSource>();

        if ( !IsFinished() )
        {
            if ( objAnimLight.GetCurrentAnimatorStateInfo( 0 ).IsName( "Idle" ) )
            {
                if ( !_missionsCleared[0] && obj == objName.Contains( "Lion" ) && activityUnlocked[0] )
                {
                    obj.GetComponent<Por03_playDarkLightObj>().PlayAnim();
                    ClearMission( 1 );
                }
                else if ( !_missionsCleared[1] && objName.Contains( "Frog" ) && activityUnlocked[1] )
                {
                    obj.GetComponent<Por03_playDarkLightObj>().PlayAnim();
                    ClearMission( 2 );
                }
                else if ( !_missionsCleared[2] && objName.Contains( "Toucan" ) ||
                          objName.Contains( "Owl" ) && activityUnlocked[2] )
                {
                    if ( !IsInArray( obj, _birdsFound ) )
                    {
                        _birdsFound[_birdsCounter] = obj;
                        _birdsCounter++;

                        obj.GetComponent<Por03_playDarkLightObj>().PlayAnim();
                    }

                    if ( _birdsCounter >= _birdsFound.Length )
                    {
                        ClearMission( 3 );
                    }
                }
            }
        }
        else
        {
            obj.GetComponent<Por03_playDarkLightObj>().PlayAnim();
        }

    }

    protected override void ClearMission( int type, bool playFeedback = true, float delay = 2 )
    {
        base.ClearMission( type, playFeedback, delay );

        if ( IsFinished() )
        {
            _lanternMask.transform.localPosition = new Vector3( 0, 0, 0 );
            _lanternMask.transform.localScale = new Vector3( 2, 2, 2 );
        }
    }
}