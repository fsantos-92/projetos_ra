using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Mat05_manager : BaseBookManager, IClickInteraction
{
    [SerializeField] private GameObject bottlesGroup;
    [SerializeField] private GameObject changeGroup;
    [SerializeField] private GameObject graphGroup;

    [SerializeField] private Button _btnReal;
    [SerializeField] private Button _btnCentavos;
    [SerializeField] private Text _txtCounter;

    private bool _bottleChosen;

    private int _reais;
    private int _centavos;

    const string REAIS = "reais";
    const string CENTAVOS = "centavos";

    private float _totalAmount;

    private GameObject[] _groups;

    private GameObject[] _trashFound;
    private int _trashCounter;

    protected override void InitializeVariables()
    {
        base.InitializeVariables();
        _reais = 0;
        _centavos = 0;
        _totalAmount = 0;
        _txtCounter.text = ( "R$ " + _totalAmount.ToString( "N2" ) ).Replace( ".", "," );
        ;
        _trashFound = new[] {new GameObject(), new GameObject()};
        _trashCounter = 0;

        _groups = new[] {bottlesGroup, changeGroup, graphGroup};
        _bottleChosen = false;
    }

    protected override void BindButtonEvents()
    {
        _btnMission_1.GetComponent<Button>().onClick.AddListener( delegate
        {
            PlayInstructionAudio( 1 );
            HideAllVisibleGroupsBut( 1 );
            ShowGroup( 1 );
        } );
        _btnMission_2.GetComponent<Button>().onClick.AddListener( delegate
        {
            PlayInstructionAudio( 2 );
            HideAllVisibleGroupsBut( 2 );
            ShowGroup( 2 );
        } );
        _btnMission_3.GetComponent<Button>().onClick.AddListener( delegate
        {
            PlayInstructionAudio( 3 );
            HideAllVisibleGroups();
        } );


        // Cliques da primeira atividade
        int counter = 0;
        foreach ( Transform child in bottlesGroup.transform.Find( "Alternatives" ) )
        {
            int auxCounter = counter;
            child.GetComponent<Button>().onClick.AddListener( () =>
            {
                if ( _bottleChosen ) return;

                if ( auxCounter != 1 )
                {
                    child.GetComponent<Animator>().Play( "Wrong" );
                }
                else
                {
                    _bottleChosen = true;
                    child.GetComponent<Animator>().Play( "Right" );

                    ClearMission( 1 );

                    Invoke( "HideAllVisibleGroups", 1 );
                }
            } );
            counter++;
        }

        // Cliques da segunda atividade
        _btnReal.onClick.RemoveAllListeners();
        _btnReal.onClick.AddListener( delegate { IncreaseCounter( REAIS ); } );

        _btnCentavos.onClick.RemoveAllListeners();
        _btnCentavos.onClick.AddListener( delegate { IncreaseCounter( CENTAVOS ); } );

        changeGroup.transform.Find( "BG/BtnConfirm" ).GetComponent<Button>().onClick.AddListener( () =>
            CheckChangeValue()
        );

        // Fecha a tela do grafico
//        graphGroup.transform.Find( "BG/Graph" ).GetComponent<Button>().onClick
//            .AddListener( delegate { HideAllVisibleGroupsBut(); } );
    }

    void IncreaseCounter( string type )
    {
        switch ( type )
        {
            case REAIS:
                _reais += 1;
                _totalAmount += 1;
                break;
            case CENTAVOS:
                _centavos += 10;
                _totalAmount += 0.1f;
                break;
        }

        if ( _centavos >= 100 )
        {
            _centavos = 0;
            _reais += 1;
        }

        if ( _reais >= 5 )
        {
            if ( _centavos != 0 )
                _reais = 0;
            else
            {
                if ( _reais > 5 )
                    _reais = 0;
            }
        }

        _totalAmount = _reais + _centavos * 0.01f;

        _txtCounter.text = ( "R$ " + _totalAmount.ToString( "N2" ) ).Replace( ".", "," );
    }

    public void ClickInteraction( GameObject obj )
    {
        string objName = obj.name;
        Animator objAnim = obj.GetComponent<Animator>();
        AudioSource objAudio = obj.GetComponent<AudioSource>();


        if ( objAnim.GetCurrentAnimatorStateInfo( 0 ).IsName( "Idle" ) )
        {
            if ( !_missionsCleared[2] && objName.Contains( "Lixo" ) && activityUnlocked[2] )
            {
                if ( !IsInArray( obj, _trashFound ) )
                {
                    _trashFound[_trashCounter] = obj;
                    _trashCounter++;

                    objAnim.SetTrigger( "Play" );
                    objAudio.Play();
                }

                if ( _trashCounter >= _trashFound.Length )
                {
                    _isAnimating = true;
                    ClearMission( 3 );
//                    ShowGroup( 3 );
                }
            }
        }
        else
        {
            objAnim.SetTrigger( "Play" );
            objAudio.Play();
        }
    }

    private void ShowGroup( int val )
    {
        for ( int i = 0; i < _groups.Length; i++ )
        {
            if ( i == val - 1 &&
                 !_groups[i].GetComponent<Animator>().GetCurrentAnimatorStateInfo( 0 ).IsName( "Show" ) )
            {
                _groups[i].SetActive( true );
                _groups[i].GetComponent<Animator>().SetTrigger( "Show" );
            }
        }
    }

    private void HideAllVisibleGroupsBut( int exception = -1 )
    {
        int counter = 0;
        foreach ( var grp in _groups )
        {
            if ( counter != exception - 1 &&
                 grp.GetComponent<Animator>().GetCurrentAnimatorStateInfo( 0 ).IsName( "Show" ) )
            {
                grp.GetComponent<Animator>().SetTrigger( "Hide" );
            }

            counter++;
        }
    }

    private void HideAllVisibleGroups()
    {
        foreach ( var grp in _groups )
        {
            if ( grp.GetComponent<Animator>().GetCurrentAnimatorStateInfo( 0 ).IsName( "Show" ) )
            {
                grp.GetComponent<Animator>().SetTrigger( "Hide" );
            }
        }
    }

    private void CheckChangeValue()
    {
        if ( _reais == 2 && _centavos == 30 )
        {
            ClearMission( 2 );
            Invoke( "HideAllVisibleGroups", 1 );
        }
        else
        {
            _txtCounter.transform.parent.GetComponent<Animator>().SetTrigger( "Wrong" );
        }
    }


    protected override void ResetProgress()
    {
        base.ResetProgress();

        bottlesGroup.transform.parent.gameObject.SetActive( false );

        _centavos = 0;
        _reais = 0;
        _totalAmount = 0;
        _txtCounter.text = ( "R$ " + _totalAmount.ToString( "N2" ) ).Replace( ".", "," );
        ;
        _trashFound = new[] {new GameObject(), new GameObject()};
        _trashCounter = 0;

        _bottleChosen = false;
    }
}