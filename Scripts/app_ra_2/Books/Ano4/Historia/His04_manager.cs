using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class His04_manager : BaseBookManager, IClickInteraction
{
    private GameObject[] _sellersFound;
    private int _sellersCounter;
    private GameObject[] _technologyFound;
    private int _technologyCounter;
    private GameObject[] _toysFound;
    private int _toysCounter;

    protected override void InitializeVariables()
    {
        base.InitializeVariables();

        _sellersFound = new[] {new GameObject(), new GameObject(), new GameObject()};
        _sellersCounter = 0;

        _technologyFound = new[]
        {
            new GameObject(), new GameObject(), new GameObject(), new GameObject(), new GameObject(), new GameObject()
        };
        _technologyCounter = 0;

        _toysFound = new[] {new GameObject(), new GameObject()};
        _toysCounter = 0;
    }

    public void ClickInteraction( GameObject obj )
    {
        if ( _isAnimating ) return;

        string objName = obj.name;
        Animator objAnim = obj.GetComponent<Animator>();
        AudioSource objAudio = obj.GetComponent<AudioSource>();

        if ( !IsFinished() )
        {
            if ( objAnim.GetCurrentAnimatorStateInfo( 0 ).IsName( "Idle" ) )
            {
                if ( !_missionsCleared[ 0 ] && objName.Contains( "Seller" ) && activityUnlocked[0] )
                {
                    if ( !IsInArray( obj, _sellersFound ) )
                    {
                        _sellersFound[ _sellersCounter ] = obj;
                        _sellersCounter++;

                        objAnim.SetTrigger( "Play" );
                        objAudio.Play();
                    }

                    if ( _sellersCounter >= _sellersFound.Length )
                    {
                        _isAnimating = true;
                        ClearMission( 1 );
                    }
                }
                else if ( !_missionsCleared[ 1 ] && objName.Contains( "Tech" ) && activityUnlocked[1] )
                {
                    if ( !IsInArray( obj, _technologyFound ) )
                    {
                        _technologyFound[ _technologyCounter ] = obj;
                        _technologyCounter++;

                        objAnim.SetTrigger( "Play" );
                        objAudio.Play();
                    }

                    if ( _technologyCounter >= _technologyFound.Length )
                    {
                        _isAnimating = true;
                        ClearMission( 2 );
                    }
                }
                else if ( !_missionsCleared[ 2 ] && objName.Contains( "Toy" ) && activityUnlocked[2] )
                {
                    if ( !IsInArray( obj, _toysFound ) )
                    {
                        _toysFound[ _toysCounter ] = obj;
                        _toysCounter++;

                        objAnim.SetTrigger( "Play" );
                        objAudio.Play();
                    }

                    if ( _toysCounter >= _toysFound.Length )
                    {
                        _isAnimating = true;
                        ClearMission( 3 );
                    }
                }
            }
        }
        else
        {
            objAnim.SetTrigger( "Play" );
            objAudio.Play();
        }
    }

    protected override void ResetProgress()
    {
        base.ResetProgress();
        
        _sellersFound = new[] {new GameObject(), new GameObject(), new GameObject()};
        _sellersCounter = 0;

        _technologyFound = new[]
        {
            new GameObject(), new GameObject(), new GameObject(), new GameObject(), new GameObject(), new GameObject()
        };
        _technologyCounter = 0;

        _toysFound = new[] {new GameObject(), new GameObject()};
        _toysCounter = 0;
    }
}