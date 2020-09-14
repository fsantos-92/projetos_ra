using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Mat01_manager : BaseBookManager, IClickInteraction
{
    private GameObject candles;
    private GameObject cat_1;
    private GameObject cat_2;
    private GameObject balloon_1;
    private GameObject balloon_2;
    private GameObject balloon_3;
    private GameObject balloon_4;
    private GameObject balloon_5;
    private GameObject balloon_6;

    private GameObject[] _catsAr;
    private GameObject[] _catsFound;
    private int catsCounter;
    private GameObject[] _balloonsAr;
    private GameObject[] _balloonsPopped;
    private int balloonsCounter;

    public override void StartBook()
    {
        base.StartBook();

        foreach ( Transform element in transform.GetComponentsInChildren<Transform>() )
        {
            if ( element.name == ( "Candles" ) )
                candles = element.gameObject;
            if ( element.name == ( "Cat_party" ) )
                cat_1 = element.gameObject;
            if ( element.name == ( "Cat_desk" ) )
                cat_2 = element.gameObject;
            if ( element.name == ( "Balloon_yellow_A" ) )
                balloon_1 = element.gameObject;
            if ( element.name == ( "Balloon_yellow_B" ) )
                balloon_2 = element.gameObject;
            if ( element.name == ( "Balloon_red_A" ) )
                balloon_3 = element.gameObject;
            if ( element.name == ( "Balloon_red_B" ) )
                balloon_4 = element.gameObject;
            if ( element.name == ( "Balloon_red_C" ) )
                balloon_5 = element.gameObject;
            if ( element.name == ( "Balloon_pink" ) )
                balloon_6 = element.gameObject;
        }

        InitializeVariables();
    }

    protected override void InitializeVariables()
    {
        base.InitializeVariables();

        _catsAr = new[] {cat_1, cat_2};
        _catsFound = new[] {new GameObject(), new GameObject()};
        catsCounter = 0;

        _balloonsAr = new[] {balloon_1, balloon_2, balloon_3, balloon_4, balloon_5, balloon_6};
        _balloonsPopped = new[]
        {
            new GameObject(), new GameObject(), new GameObject(), new GameObject(), new GameObject(), new GameObject()
        };
        balloonsCounter = 0;
    }

    protected override void PlayInstructionAudio( int type )
    {
        base.PlayInstructionAudio( type );

        if ( type == 3 )
        {
            foreach ( var balloon in _balloonsAr )
            {
                var balloonAnim = balloon.GetComponent<Animator>();
                if ( balloonAnim.GetCurrentAnimatorStateInfo( 0 ).IsName( "Idle" ) )
                    balloonAnim.SetTrigger( "Move" );
            }
        }
    }

    public void ClickInteraction( GameObject obj )
    {
        if ( _isAnimating ) return;

        string objName = obj.name;
        Animator objAnim = obj.GetComponent<Animator>();
        AudioSource objAudio = obj.GetComponent<AudioSource>();

        if ( !IsFinished() )
        {
            if ( objAnim.GetCurrentAnimatorStateInfo( 0 ).IsName( "Idle" ))
            {
                // Se foi a vela
                if ( !_missionsCleared[0] && obj == candles && activityUnlocked[0]  )
                {
                    _isAnimating = true;

                    objAudio.Play();
                    objAnim.Play( "Blow" );

                    ClearMission( 1 );
                }

                // Se for um gato
                else if ( !_missionsCleared[1] && objName.Contains( "Cat" ) && activityUnlocked[1] )
                {
                    // Verifica se o objeto já foi clicado
                    if ( !IsInArray( obj, _catsFound ) )
                    {
                        _catsFound[catsCounter] = obj;
                        catsCounter++;

                        objAnim.SetTrigger( "Play" );
                        objAudio.Play();
                    }

                    // Clicou em todos, toca o feedback e a próxima instrucao
                    if ( catsCounter >= _catsAr.Length )
                    {
                        _isAnimating = true;
                        ClearMission( 2 );
                    }
                }
            }
            // Se for uma bexiga
            else if ( !_missionsCleared[2] && objName.Contains( "Balloon" ) &&
                      objAnim.GetCurrentAnimatorStateInfo( 0 ).IsName( "Moving" ) && activityUnlocked[2] )
            {
                // Verifica se o objeto já foi clicado
                if ( !IsInArray( obj, _balloonsPopped ) )
                {
                    _balloonsPopped[balloonsCounter] = obj;
                    objAnim.SetTrigger( "Pop" );
                    balloonsCounter++;
                }

                // Clicou em todos, toca o feedback
                if ( balloonsCounter >= _balloonsAr.Length )
                {
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

    protected override void ResetProgress()
    {
        base.ResetProgress();

        _catsFound = new[] {new GameObject(), new GameObject()};
        catsCounter = 0;

        _balloonsPopped = new[]
        {
            new GameObject(), new GameObject(), new GameObject(), new GameObject(), new GameObject(), new GameObject()
        };
        balloonsCounter = 0;
    }
}