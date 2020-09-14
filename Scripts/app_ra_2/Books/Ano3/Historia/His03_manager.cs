using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class His03_manager : BaseBookManager, IClickInteraction
{
    GameObject sanfona;
    GameObject gaucha;
    GameObject gaucho;
    [SerializeField] AudioClip _clipFrevo;
    private List<GameObject> _frevo;
    private bool[] _canPlayFeedback;

    public override void StartBook()
    {
        base.StartBook();

        foreach ( Transform element in transform.GetComponentsInChildren<Transform>() )
        {
            if ( element.name == ( "Menino_sanfona" ) )
                sanfona = element.gameObject;
            if ( element.name == ( "Gauchinha" ) )
                gaucha = element.gameObject;
            if ( element.name == ( "Gaucho_2" ) )
                gaucho = element.gameObject;
        }
    }

    protected override void InitializeVariables()
    {
        base.InitializeVariables();
        _frevo = new List<GameObject>();
        _canPlayFeedback = new bool[3] {false, false, false};
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
            if ( objName.Contains( "Frevo" ) && activityUnlocked[0] )
            {
                _isAnimating = true;
                objAnim.SetTrigger( "Play" );
                obj.GetComponent<AudioSource>().Play();

                GameObject[] auxArray = _frevo.ToArray();
                if ( !IsInArray( obj, auxArray ) )
                {
                    _frevo.Add( obj );
                }

                if ( _frevo.Count >= 3 )
                {
                    _audioSource.PlayOneShot( _clipFrevo );
                    ClearMission( 1, _canPlayFeedback[0] );
                    _canPlayFeedback[0] = false;
                }

                Invoke( "ResetAnim", 1 );
            }
            else if ( obj == sanfona && activityUnlocked[1] )
            {
                _isAnimating = true;
                objAnim.SetTrigger( "Play" );
                obj.GetComponent<AudioSource>().Play();

                ClearMission( 2, _canPlayFeedback[1] );
                _canPlayFeedback[1] = false;
                Invoke( "ResetAnim", 1 );
            }
            else if ( obj == gaucha && activityUnlocked[2] )
            {
                _isAnimating = true;
                objAnim.SetTrigger( "Play" );
                obj.GetComponent<AudioSource>().Play();
                gaucho.GetComponent<Animator>().SetTrigger( "Play" );
                gaucho.GetComponent<AudioSource>().Play();

                ClearMission( 3, _canPlayFeedback[2] );
                _canPlayFeedback[2] = false;
                Invoke( "ResetAnim", 1 );
            }
        }
    }


    protected override void ResetProgress()
    {
        base.ResetProgress();
        _frevo = new List<GameObject>();
        _canPlayFeedback = new bool[3] {false, false, false};
    }
}