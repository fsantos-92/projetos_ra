using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Geo04_manager : BaseBookManager, IClickInteraction
{
    [SerializeField] private GameObject BtnShowWaterInfo;
    [SerializeField] private GameObject ImgWaterInfo;
    private GameObject Woods;

    private List<GameObject> _trashes;
    private bool[] _canPlayFeedback;

    public override void StartBook()
    {
        base.StartBook();

        foreach ( Transform element in transform.GetComponentsInChildren<Transform>() )
        {
            if ( element.name == ( "MataCiliar" ) )
                Woods = element.gameObject;
        }
    }

    protected override void InitializeVariables()
    {
        base.InitializeVariables();
        _canPlayFeedback = new bool[3] {true, true, true};
        _trashes = new List<GameObject>();
    }

    protected override void BindButtonEvents()
    {
        base.BindButtonEvents();
        BtnShowWaterInfo.GetComponent<Button>().onClick.AddListener( BtnWaterInfoClick );
    }

    protected override void PlayInstructionAudio( int type )
    {
        base.PlayInstructionAudio( type );

        if ( type == 1 )
        {
            BtnShowWaterInfo.gameObject.SetActive( true );
            BtnShowWaterInfo.gameObject.GetComponent<Animator>().SetTrigger( "Play" );
        }
    }

    void BtnWaterInfoClick()
    {
        StartCoroutine( ShowWaterInfo() );
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        if ( BtnShowWaterInfo.gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo( 0 ).IsName( "Action" ) )
            BtnShowWaterInfo.gameObject.GetComponent<Animator>().SetTrigger( "Play" );

        if ( ImgWaterInfo.gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo( 0 ).IsName( "Action" ) )
            ImgWaterInfo.gameObject.GetComponent<Animator>().SetTrigger( "Play" );

        this.ImgWaterInfo.SetActive( false );
        this.BtnShowWaterInfo.gameObject.SetActive( false );
    }

    IEnumerator ShowWaterInfo()
    {
        BtnShowWaterInfo.gameObject.GetComponent<Animator>().SetTrigger( "Play" );
        yield return new WaitForSeconds( 0.5f );
        this.BtnShowWaterInfo.gameObject.SetActive( false );
        this.ImgWaterInfo.SetActive( true );
        this.ImgWaterInfo.GetComponent<Animator>().SetTrigger( "Play" );
        yield return new WaitForSeconds( 5f );
        this.ImgWaterInfo.GetComponent<Animator>().SetTrigger( "Play" );
        yield return new WaitForSeconds( 0.5f );
        this.ImgWaterInfo.SetActive( false );
        ClearMission( 1 );
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
            if ( obj == Woods && activityUnlocked[1] )
            {
                _isAnimating = true;
                objAnim.SetTrigger( "Play" );
                obj.GetComponent<AudioSource>().Play();

                ClearMission( 2, _canPlayFeedback[1] );
                _canPlayFeedback[1] = false;
            }
            else if ( objName.Contains( "Lixo" ) && activityUnlocked[2] )
            {
                _isAnimating = true;
                objAnim.SetTrigger( "Play" );
                obj.GetComponent<AudioSource>().Play();

                if ( obj.GetComponent<BoxCollider>().enabled )
                    obj.GetComponent<BoxCollider>().enabled = false;

                GameObject[] auxArray = _trashes.ToArray();
                if ( !IsInArray( obj, auxArray ) )
                {
                    _trashes.Add( obj );
                }

                if ( _trashes.Count >= 3 )
                {
                    ClearMission( 3, _canPlayFeedback[2] );
                    _canPlayFeedback[2] = false;
                }

                Invoke( "ResetAnim", 1 );
            }
        }
    }


    protected override void ResetProgress()
    {
        base.ResetProgress();

        InitializeVariables();

        BtnShowWaterInfo.SetActive( false );
        ImgWaterInfo.SetActive( false );
    }
}