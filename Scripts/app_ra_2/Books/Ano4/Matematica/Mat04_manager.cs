using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking.Match;
using UnityEngine.UI;

public class Mat04_manager : BaseBookManager, IClickInteraction
{
    private GameObject _blueBlocks;
    private GameObject _yellowBlocks;
    private GameObject _blocksBoard;

    [SerializeField] private GameObject question_1;
    [SerializeField] private GameObject question_2;
    [SerializeField] private GameObject question_3;

    public override void StartBook()
    {
        base.StartBook();

        foreach ( Transform element in transform.GetComponentsInChildren<Transform>() )
        {
            if ( element.name == ( "BlueBlocks" ) )
                _blueBlocks = element.gameObject;
            if ( element.name == ( "YellowBlocks" ) )
                _yellowBlocks = element.gameObject;
            if ( element.name == ( "BlocksBoard" ) )
                _blocksBoard = element.gameObject;
        }
    }

    protected override void BindButtonEvents()
    {
        _btnMission_1.GetComponent<Button>().onClick.AddListener( delegate { ShowQuestion( 1 ); } );
        _btnMission_2.GetComponent<Button>().onClick.AddListener( delegate { ShowQuestion( 2 ); } );
        _btnMission_3.GetComponent<Button>().onClick.AddListener( delegate { ShowQuestion( 3 ); } );
    }

    public void ClickInteraction( GameObject obj )
    {
        Animator objAnim = obj.GetComponent<Animator>();
        AudioSource objAudio = obj.GetComponent<AudioSource>();

//        if ( !IsFinished() )
//        {
//            if ( objAnim.GetCurrentAnimatorStateInfo( 0 ).IsName( "Idle" ) )
//            {
//                objAnim.SetTrigger( "Play" );
//                objAudio.Play();
//
//                if ( !_missionsCleared[ 0 ] && obj == _blueBlocks )
//                {
//                    question_1.GetComponent<Mat04_questionManager>().ShowQuestion();
//                }
//                else if ( !_missionsCleared[ 1 ] && obj == _yellowBlocks )
//                {
//                    question_2.GetComponent<Mat04_questionManager>().ShowQuestion();
//                }
//                else if ( !_missionsCleared[ 2 ] && obj == _blocksBoard )
//                {
//                    Debug.Log( "685464163" );
//
//                    question_3.GetComponent<Mat04_questionManager>().ShowQuestion();
//                }
//            }
//        }
//        else
//        {
        objAnim.SetTrigger( "Play" );
        objAudio.Play();
//        }
    }

    private void ShowQuestion( int type )
    {
        PlayInstructionAudio( type );
        switch ( type )
        {
            case 1:
                question_1.GetComponent<Mat04_questionManager>().ShowQuestion();

                if ( question_2.GetComponent<Animator>().GetCurrentAnimatorStateInfo( 0 ).IsName( "Show" ) )
                    question_2.GetComponent<Mat04_questionManager>().HideQuestion();
                if ( question_3.GetComponent<Animator>().GetCurrentAnimatorStateInfo( 0 ).IsName( "Show" ) )
                    question_3.GetComponent<Mat04_questionManager>().HideQuestion();

                break;
            case 2:
                question_2.GetComponent<Mat04_questionManager>().ShowQuestion();

                if ( question_1.GetComponent<Animator>().GetCurrentAnimatorStateInfo( 0 ).IsName( "Show" ) )
                    question_1.GetComponent<Mat04_questionManager>().HideQuestion();
                if ( question_3.GetComponent<Animator>().GetCurrentAnimatorStateInfo( 0 ).IsName( "Show" ) )
                    question_3.GetComponent<Mat04_questionManager>().HideQuestion();
                break;
            case 3:
                question_3.GetComponent<Mat04_questionManager>().ShowQuestion();

                if ( question_1.GetComponent<Animator>().GetCurrentAnimatorStateInfo( 0 ).IsName( "Show" ) )
                    question_1.GetComponent<Mat04_questionManager>().HideQuestion();
                if ( question_2.GetComponent<Animator>().GetCurrentAnimatorStateInfo( 0 ).IsName( "Show" ) )
                    question_2.GetComponent<Mat04_questionManager>().HideQuestion();
                break;
        }
    }

    public void AnswerQuestion( int questionNumber )
    {
        ClearMission( questionNumber );
    }

    protected override void ResetProgress()
    {
        base.ResetProgress();

        question_1.transform.parent.gameObject.SetActive( false );
    }
}