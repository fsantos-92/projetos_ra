using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Por01_manager : BaseBookManager, IClickInteraction
{
    private GameObject turtle;
    private GameObject goat;
    private GameObject hen;
    private GameObject cat;
    private GameObject dancers;
    private GameObject windowFrog;

    private GameObject[] _instrumentsClicked;
    private int _instrumentsCounter;

    public override void StartBook()
    {
        base.StartBook();

        foreach ( Transform element in transform.GetComponentsInChildren<Transform>() )
        {
            if ( element.name == ( "Dancers" ) )
                dancers = element.gameObject;
            if ( element.name == ( "WindowFrog" ) )
                windowFrog = element.gameObject;
        }
    }

    protected override void InitializeVariables()
    {
        base.InitializeVariables();

        _instrumentsClicked = new[] {new GameObject(), new GameObject(), new GameObject(), new GameObject(),};
        _instrumentsCounter = 0;
    }

    public void ClickInteraction( GameObject obj )
    {
        if ( _isAnimating ) return;

        Animator objAnim = obj.GetComponent<Animator>();
        AudioSource objAudio = obj.GetComponent<AudioSource>();
        string objName = obj.name;

        // Se o objeto nao estiver sendo animado
        if ( objAnim.GetCurrentAnimatorStateInfo( 0 ).IsName( "Idle" ) )
        {
            // Se as atividades do livro ainda nao foram completadas
            if ( !IsFinished() )
            {
                if ( !_missionsCleared[0] && objName.Contains( "Musical" ) && activityUnlocked[0] )
                {
                    AnimateClickedObj();

                    if ( !IsInArray( obj, _instrumentsClicked ) )
                    {
                        _instrumentsClicked[_instrumentsCounter] = obj;
                        _instrumentsCounter++;
                    }

                    if ( _instrumentsCounter >= 4 )
                    {
                        _isAnimating = true;

                        ClearMission( 1 );
                    }
                }
                else if ( !_missionsCleared[1] && obj == dancers && activityUnlocked[1] )
                {
                    _isAnimating = true;

                    // ClearMission( 2 );
                    Invoke( "DancersMission", 8f );

                    AnimateClickedObj();
                }
                else if ( !_missionsCleared[2] && obj == windowFrog && activityUnlocked[2] )
                {
                    _isAnimating = true;

                    ClearMission( 3 );

                    AnimateClickedObj();
                }
            }
            // Se as atividades foram completadas, o objeto é apenas animado
            else
            {
                AnimateClickedObj();
            }
        }

        void AnimateClickedObj()
        {
            objAnim.SetTrigger( "Play" );
            if ( objAudio )
                objAudio.Play();
        }
    }

    void DancersMission()
    {
        ClearMission( 2 );
    }

    protected override void ResetProgress()
    {
        base.ResetProgress();

        _instrumentsClicked = new[] {new GameObject(), new GameObject(), new GameObject(), new GameObject(),};
        _instrumentsCounter = 0;
    }
}