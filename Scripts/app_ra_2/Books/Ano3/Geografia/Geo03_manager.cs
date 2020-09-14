using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Geo03_manager : BaseBookManager, IClickInteraction
{
    [SerializeField] Button _btnBike;
    [SerializeField] Button _btnBoat;
    [SerializeField] GameObject _box_atv01;
    [SerializeField] GameObject _box_atv02;
    GameObject _boat;

    [SerializeField] Button _btnAgua;
    [SerializeField] Button _btnBarraca;
    [SerializeField] Button _btnPatins;
    [SerializeField] Button _btnFlores;

    [SerializeField] AudioClip _clipShow;
    [SerializeField] AudioClip _clipHide;
    [SerializeField] Button _btnClose;

    private List<GameObject> _mission02Elements;
    private List<GameObject> _trashes;

    public override void StartBook()
    {
        base.StartBook();

        foreach ( Transform element in transform.GetComponentsInChildren<Transform>() )
        {
            if ( element.name == ( "Barco" ) )
                _boat = element.gameObject;
        }
    }

    protected override void InitializeVariables()
    {
        base.InitializeVariables();
        _mission02Elements = new List<GameObject>();
        _trashes = new List<GameObject>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        BtnCloseClick();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _btnClose.onClick.RemoveListener( BtnCloseClick );
    }

    protected override void BindButtonEvents()
    {
        base.BindButtonEvents();
        _btnBoat.onClick.AddListener( btnBoatClick );
        _btnAgua.onClick.AddListener( delegate { BtnMission2Click( _btnAgua.gameObject ); } );
        _btnBarraca.onClick.AddListener( delegate { BtnMission2Click( _btnBarraca.gameObject ); } );
        _btnClose.onClick.AddListener( BtnCloseClick );
    }

    protected override void PlayInstructionAudio( int type )
    {
        base.PlayInstructionAudio( type );
        if ( _box_atv01.GetComponent<CanvasGroup>().blocksRaycasts == true ||
             _box_atv02.GetComponent<CanvasGroup>().blocksRaycasts == true )
        {
            BtnCloseClick();
            return;
        }

        if ( type == 1 )
        {
            Animator[] anims = new Animator[]
            {
                _box_atv01.GetComponent<Animator>(), _btnBike.GetComponent<Animator>(),
                _btnBoat.GetComponent<Animator>()
            };
            atv( anims, true );
        }

        if ( type == 2 )
        {
            Animator[] anims = new Animator[]
            {
                _box_atv02.GetComponent<Animator>(), _btnAgua.GetComponent<Animator>(),
                _btnBarraca.GetComponent<Animator>(), _btnPatins.GetComponent<Animator>(),
                _btnFlores.GetComponent<Animator>()
            };
            atv( anims, true );
        }
    }

    void atv( Animator[] anims, bool show )
    {
        foreach ( Animator element in anims )
        {
            element.gameObject.SetActive( true );
            if ( !show )
            {
                if ( !element.GetCurrentAnimatorStateInfo( 0 ).IsName( "hidden" ) )
                {
                    element.SetTrigger( "Hide" );
                }

                if ( element.gameObject.GetComponent<Button>() )
                    element.GetComponent<Button>().interactable = false;
                if ( element.gameObject.GetComponent<CanvasGroup>() )
                    element.gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
            }
            else
            {
                element.gameObject.SetActive( true );
                element.SetTrigger( "Play" );
                if ( element.gameObject.GetComponent<Button>() )
                    element.GetComponent<Button>().interactable = true;
                if ( element.gameObject.GetComponent<CanvasGroup>() )
                    element.gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
            }
        }
    }

    private void btnBoatClick()
    {
        StartCoroutine( boatAnim() );
    }

    void BtnCloseClick()
    {
        Animator[] anims = new Animator[]
        {
            _box_atv01.GetComponent<Animator>(),
            _btnBike.GetComponent<Animator>(),
            _btnBoat.GetComponent<Animator>(),
            _box_atv02.GetComponent<Animator>(),
            _btnAgua.GetComponent<Animator>(),
            _btnBarraca.GetComponent<Animator>(),
            _btnPatins.GetComponent<Animator>(),
            _btnFlores.GetComponent<Animator>()
        };
        atv( anims, false );
    }

    private void BtnMission2Click( GameObject btn )
    {
        GameObject[] auxArray = _mission02Elements.ToArray();
        if ( !IsInArray( btn, auxArray ) )
        {
            _mission02Elements.Add( btn );
            btn.GetComponent<Animator>().SetTrigger( "Hide" );
            btn.GetComponent<Button>().interactable = false;
        }

        if ( _mission02Elements.Count >= 2 )
        {
            Animator[] anims = new Animator[]
            {
                _box_atv02.GetComponent<Animator>(), _btnAgua.GetComponent<Animator>(),
                _btnBarraca.GetComponent<Animator>(), _btnPatins.GetComponent<Animator>(),
                _btnFlores.GetComponent<Animator>()
            };
            atv( anims, false );
            ClearMission( 2 );
        }
    }

    IEnumerator boatAnim()
    {
        _btnBoat.interactable = false;
        Animator[] anims = new Animator[]
            {_box_atv01.GetComponent<Animator>(), _btnBike.GetComponent<Animator>(), _btnBoat.GetComponent<Animator>()};
        atv( anims, false );
        yield return new WaitForSeconds( 0.5f );
        _boat.GetComponent<Animator>().SetTrigger( "Play" );
        yield return new WaitForSeconds( 1f );
        ClearMission( 1 );
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
            if ( objName.Contains( "Lixo" ) && activityUnlocked[2] )
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

                if ( _trashes.Count >= 4 )
                {
                    ClearMission( 3 );
                }

                Invoke( "ResetAnim", 1 );
            }
        }
    }

    void ResetAnim()
    {
        _isAnimating = false;
    }

    protected override void ResetProgress()
    {
        base.ResetProgress();

        InitializeVariables();

        _box_atv01.SetActive( false );
        _box_atv02.SetActive( false );
    }
}