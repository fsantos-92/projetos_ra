using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Geo05_manager : BaseBookManager, IClickInteraction
{
    [SerializeField] private GameObject questionGroup;
    [SerializeField] private Button btn_rural;
    [SerializeField] private Button btn_urban;

    [SerializeField] private GameObject mapGroup;

    [SerializeField] private Button[] _btnsNorth;
    [SerializeField] private Button[] _btnsNorthEast;
    [SerializeField] private Button[] _btnsMiddleWest;
    [SerializeField] private Button[] _btnsSouthEast;
    [SerializeField] private Button[] _btnsSouth;
    [SerializeField] private AudioClip[] _clipsRegions;

    private bool _questionAnswered = false;

    protected override void InitializeVariables()
    {
        base.InitializeVariables();

        btn_rural.onClick.AddListener( () =>
        {
            if ( _questionAnswered ) return;
            btn_rural.GetComponent<Animator>().Play( "Wrong" );
        } );

        btn_urban.onClick.AddListener( () =>
        {
            if ( _questionAnswered ) return;

            _questionAnswered = true;

            btn_urban.GetComponent<Animator>().Play( "Right" );

            ClearMission( 1 );

            Invoke( "HideQuestionGroup", 1 );
        } );

        // foreach (Transform stateBtn in mapGroup.transform.Find("Map"))
        // {
        //     stateBtn.GetComponent<Button>().onClick.AddListener(() =>
        //    {
        //        stateBtn.GetComponent<Animator>().SetTrigger("Play");
        //        stateBtn.GetComponent<AudioSource>().Play();
        //    });
        // }
    }

    protected override void BindButtonEvents()
    {
        _btnMission_1.GetComponent<Button>().onClick.AddListener( delegate { ShowQuestionGroup(); } );
        _btnMission_2.GetComponent<Button>().onClick.AddListener( () =>
        {
            HideMapGroup();
            HideQuestionGroup();
            PlayInstructionAudio( 2 );
        } );
        _btnMission_3.GetComponent<Button>().onClick.AddListener( delegate { ShowMapGroup(); } );

        foreach ( Button element in _btnsNorth )
            element.onClick.AddListener( delegate { BtnRegionsClick( "Norte" ); } );
        foreach ( Button element in _btnsNorthEast )
            element.onClick.AddListener( delegate { BtnRegionsClick( "Nordeste" ); } );
        foreach ( Button element in _btnsMiddleWest )
            element.onClick.AddListener( delegate { BtnRegionsClick( "CentroOeste" ); } );
        foreach ( Button element in _btnsSouthEast )
            element.onClick.AddListener( delegate { BtnRegionsClick( "Sudeste" ); } );
        foreach ( Button element in _btnsSouth ) element.onClick.AddListener( delegate { BtnRegionsClick( "Sul" ); } );
    }

    void BtnRegionsClick( string region )
    {
        switch ( region )
        {
            case "Norte":
                foreach ( Button element in _btnsNorth )
                {
                    element.GetComponent<Animator>().SetTrigger( "Play" );
                }

                _audioSource.PlayOneShot( _clipsRegions[0] );
                break;
            case "Nordeste":
                foreach ( Button element in _btnsNorthEast )
                {
                    element.GetComponent<Animator>().SetTrigger( "Play" );
                }

                _audioSource.PlayOneShot( _clipsRegions[1] );
                break;
            case "CentroOeste":
                foreach ( Button element in _btnsMiddleWest )
                {
                    element.GetComponent<Animator>().SetTrigger( "Play" );
                }

                _audioSource.PlayOneShot( _clipsRegions[2] );
                break;
            case "Sudeste":
                foreach ( Button element in _btnsSouthEast )
                {
                    element.GetComponent<Animator>().SetTrigger( "Play" );
                }

                _audioSource.PlayOneShot( _clipsRegions[3] );
                break;
            case "Sul":
                foreach ( Button element in _btnsSouth )
                {
                    element.GetComponent<Animator>().SetTrigger( "Play" );
                }

                _audioSource.PlayOneShot( _clipsRegions[4] );
                break;
        }
    }

    protected override void PlayInstructionAudio( int type )
    {
        base.PlayInstructionAudio( type );

        // Mostra os textos, se existirem
        //    if ( _textsMissionAr[ type - 1 ] != "" )
        //    {
        //      _textMission.SetActive( true );
        //      _textMission.GetComponent<Animator>().SetTrigger( "Show" );
        //      _textMission.transform.Find( "Text" ).gameObject.GetComponent<Text>().text = _textsMissionAr[ type - 1 ];
        //    }
        //    else
        //    {
        //      _textMission.GetComponent<Animator>().SetTrigger( "Reset" );
        //    }
    }

    public void ClickInteraction( GameObject obj )
    {
        if ( _isAnimating ) return;

        if ( !_missionsCleared[1] && obj.name == "Girl" && activityUnlocked[1] )
        {
            _isAnimating = true;

            obj.GetComponent<Animator>().SetTrigger( "Play" );

            ClearMission( 2 );
        }
    }

    private void ShowQuestionGroup()
    {
        if ( _missionsCleared[0] ) return;

        HideMapGroup();

        PlayInstructionAudio( 1 );

        questionGroup.SetActive( true );
        questionGroup.GetComponent<Animator>().Play( "Show" );
    }

    private void HideQuestionGroup()
    {
        if ( questionGroup.GetComponent<Animator>().GetCurrentAnimatorStateInfo( 0 ).IsName( "Show" ) )
        {
            questionGroup.GetComponent<Animator>().Play( "Hide" );
        }
    }


    private void ShowMapGroup()
    {
        HideQuestionGroup();

        if ( !mapGroup.GetComponent<Animator>().GetCurrentAnimatorStateInfo( 0 ).IsName( "Show" ) )
        {
            PlayInstructionAudio( 3 );
            mapGroup.SetActive( true );
            mapGroup.GetComponent<Animator>().SetTrigger( "Show" );
        }
        else
        {
            mapGroup.GetComponent<Animator>().SetTrigger( "Hide" );
        }
    }

    private void HideMapGroup()
    {
        if ( mapGroup.GetComponent<Animator>().GetCurrentAnimatorStateInfo( 0 ).IsName( "Show" ) )
        {
            mapGroup.GetComponent<Animator>().SetTrigger( "Hide" );
        }
    }

    protected override void ResetProgress()
    {
        base.ResetProgress();
        _questionAnswered = false;

        questionGroup.SetActive( false );
        mapGroup.SetActive( false );
    }
}