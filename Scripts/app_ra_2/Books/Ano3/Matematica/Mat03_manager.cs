using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking.Match;
using UnityEngine.UI;

public class Mat03_manager : BaseBookManager, IClickInteraction
{
    private GameObject notesBoy;
    private GameObject mandala;
    private GameObject earth;

    [SerializeField] private GameObject question;

    [SerializeField] private GameObject questionBoxMission3;

    public override void StartBook()
    {
        base.StartBook();

        foreach ( Transform element in transform.GetComponentsInChildren<Transform>() )
        {
            if ( element.name == ( "NotesBoy" ) )
                notesBoy = element.gameObject;
            if ( element.name == ( "Mandala" ) )
                mandala = element.gameObject;
            if ( element.name == ( "Sol" ) )
                earth = element.gameObject;
        }
    }

    // Toca os sons da instrução da missão
    protected override void PlayInstructionAudio( int type )
    {
        base.PlayInstructionAudio( type );

        if ( type == 2 )
        {
            question.GetComponent<Mat03_questionManager>().ShowQuestion();
        }
        else if ( type != 2 && question.GetComponent<Animator>().GetCurrentAnimatorStateInfo( 0 ).IsName( "Show" ) &&
                  !_missionsCleared[1] )
        {
            question.GetComponent<Mat03_questionManager>().HideQuestion();
        }
        else if ( type == 3 )
        {
            Invoke( "CloseBox", 3 );
        }

        _isAnimating = false;
    }

    void CloseBox()
    {
        if ( questionBoxMission3.activeInHierarchy )
        {
            questionBoxMission3.SendMessage( "BtnCloseClick" );
        }
    }

    // Trata os cliques dos elementos relacionados às missões
    public void ClickInteraction( GameObject obj )
    {
        if ( _isAnimating ) return;

        Animator objAnim = obj.GetComponent<Animator>();
        AudioSource objAudio = obj.GetComponent<AudioSource>();

        if ( !IsFinished() )
        {
            if ( objAnim.GetCurrentAnimatorStateInfo( 0 ).IsName( "Idle" ) )
            {
                if ( !_missionsCleared[0] && obj == notesBoy && activityUnlocked[0] )
                {
                    _isAnimating = true;
                    objAnim.SetTrigger( "Play" );
                    objAudio.Play();
                    ClearMission( 1 );
                }
                else if ( !_missionsCleared[2] && obj == earth && activityUnlocked[2] )
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

    public void AnswerQuestion()
    {
        ClearMission( 2 );
        mandala.GetComponent<Animator>().SetTrigger( "Play" );
    }


    protected override void ResetProgress()
    {
        base.ResetProgress();

        question.GetComponent<Mat03_questionManager>().lockQuestion = false;
        question.transform.parent.gameObject.SetActive( false );
    }
}