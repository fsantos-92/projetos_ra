using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class His01_manager : BaseBookManager, IClickInteraction
{
    private List<GameObject> _hidingKids;
    private List<GameObject> _playsWithP;
    private List<GameObject> _heroes;
    bool[] _canPlayFeedback;

    protected override void InitializeVariables()
    {
        base.InitializeVariables();
        _hidingKids = new List<GameObject>();
        _playsWithP = new List<GameObject>();
        _heroes = new List<GameObject>();
        _canPlayFeedback = new bool[3] {true, true, true};
    }

    void ResetAnim()
    {
        _isAnimating = false;
    }

    public void ClickInteraction( GameObject obj )
    {
        if ( _isAnimating )
            return;
        Animator objAnim = obj.GetComponent<Animator>();
        string objName = obj.name;

        // Se o objeto nao estiver sendo animado
        if ( objAnim.GetCurrentAnimatorStateInfo( 0 ).IsName( "Idle" ) )
        {
            if ( objName.Contains( "Esconde" ) && activityUnlocked[0] )
            {
                _isAnimating = true;
                objAnim.SetTrigger( "Play" );
                obj.GetComponent<AudioSource>().Play();

                GameObject[] auxArray = _hidingKids.ToArray();
                if ( !IsInArray( obj, auxArray ) )
                {
                    _hidingKids.Add( obj );
                }

                if ( _hidingKids.Count >= 2 )
                {
                    ClearMission( 1, _canPlayFeedback[0] );
                    _canPlayFeedback[0] = false;
                }

                Invoke( "ResetAnim", 2 );
            }
            else if ( objName.Contains( "Brincadeira" ) && activityUnlocked[1]  )
            {
                _isAnimating = true;
                objAnim.SetTrigger( "Play" );
                obj.GetComponent<AudioSource>().Play();

                GameObject[] auxArray = _playsWithP.ToArray();
                if ( !IsInArray( obj, auxArray ) )
                {
                    _playsWithP.Add( obj );
                }

                if ( _playsWithP.Count >= 3 )
                {
                    ClearMission( 2, _canPlayFeedback[1] );
                    _canPlayFeedback[1] = false;
                }

                Invoke( "ResetAnim", 2 );
            }
            else if ( objName.Contains( "Heroi" ) && activityUnlocked[2]  )
            {
                _isAnimating = true;
                objAnim.SetTrigger( "Play" );
                obj.GetComponent<AudioSource>().Play();

                GameObject[] auxArray = _heroes.ToArray();
                if ( !IsInArray( obj, auxArray ) )
                {
                    _heroes.Add( obj );
                }

                if ( _heroes.Count >= 2 )
                {
                    ClearMission( 3, _canPlayFeedback[2] );
                    _canPlayFeedback[2] = false;
                }

                Invoke( "ResetAnim", 2 );
            }
        }
    }

    protected override void ResetProgress()
    {
        base.ResetProgress();

        _hidingKids = new List<GameObject>();
        _playsWithP = new List<GameObject>();
        _heroes = new List<GameObject>();
        _canPlayFeedback = new bool[3] {true, true, true};
    }
}