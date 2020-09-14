using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BaseBookManager : MonoBehaviour
{
    public static BaseBookManager instance;

    //[SerializeField] protected GameObject _spritePrefab;

    protected GameObject _uiCanvas;
    protected GameObject _textMission;
    public String[] _textsMissionAr;

    protected GameObject _btnMission_1;
    protected GameObject _btnMission_2;
    protected GameObject _btnMission_3;
    protected GameObject[] _btnsMission;
    protected bool[] _missionsCleared;
    public AudioClip[] _instructionsAr;
    public AudioClip[] _feedbackAr;
    protected AudioSource _audioSource;
    protected bool _isAnimating;

    protected bool[] activityUnlocked;

    protected virtual void Awake()
    {
        MakeInstance();
        InitializeVariables();
        setMissions( new bool[3] {false, false, false} );

        activityUnlocked = new[] {false, false, false};
    }

    protected virtual void OnEnable()
    {
        BindButtonEvents();
        InstantiateSprites();
    }

    public virtual void InstantiateSprites()
    {
        StartCoroutine(LoadAsset());
    }

    IEnumerator LoadAsset()
    {
        string name = "Sprites" + gameObject.name.Substring(0, 5);
        var resource = Resources.LoadAsync(name);
        while(!resource.isDone)
        {
            yield return null;
        }
        GameObject b = Instantiate(resource.asset, transform.position, transform.rotation, transform) as GameObject;
        //GameObject b = Instantiate( _spritePrefab, transform.position, transform.rotation, transform );
        b.transform.localScale = Vector3.one;
        b.transform.localPosition = Vector3.zero;
        b.SetActive(false);
        yield return new WaitForSeconds(1.5f);
        b.SetActive(true);
    }

    protected virtual void OnDisable()
    {
        StopAllCoroutines();
        UnbindButtonEvents();
        if ( transform.childCount > 0 )
        {
            Destroy( transform.GetChild( 0 ).gameObject );
        }
        Resources.UnloadUnusedAssets();
    }

    protected void MakeInstance()
    {
        if ( instance == null ) instance = this;
    }

    protected virtual void InitializeVariables()
    {
        GameObject[] rootGameObjs = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
        foreach ( GameObject element in rootGameObjs )
        {
            if ( element.name == "Canvas" )
                _uiCanvas = element.transform.Find( "ScreenGame" ).gameObject;
        }

        _btnMission_1 = _uiCanvas.transform.Find( "Group_Btn_Missions/Btn_Mission_1" ).gameObject;
        _btnMission_2 = _uiCanvas.transform.Find( "Group_Btn_Missions/Btn_Mission_2" ).gameObject;
        _btnMission_3 = _uiCanvas.transform.Find( "Group_Btn_Missions/Btn_Mission_3" ).gameObject;
        _btnsMission = new[] {_btnMission_1, _btnMission_2, _btnMission_3};

        _textMission = _uiCanvas.transform.Find( "Mission_Text/Mission_BG" ).gameObject;

        RandomizeCanvasImages();

        _audioSource = GetComponent<AudioSource>();
        _isAnimating = false;
    }

    protected virtual void BindButtonEvents()
    {
        _btnMission_1.GetComponent<Button>().onClick.AddListener( delegate { PlayInstructionAudio( 1 ); } );
        _btnMission_2.GetComponent<Button>().onClick.AddListener( delegate { PlayInstructionAudio( 2 ); } );
        _btnMission_3.GetComponent<Button>().onClick.AddListener( delegate { PlayInstructionAudio( 3 ); } );
    }

    public void setMissions( bool[] missionsState )
    {
        if ( missionsState.Length != 3 )
            _missionsCleared = new[] {false, false, false};
        else
            _missionsCleared = missionsState;
    }

    protected virtual void UnbindButtonEvents()
    {
        if ( _btnMission_1 != null ) _btnMission_1.GetComponent<Button>().onClick.RemoveAllListeners();
        if ( _btnMission_2 != null ) _btnMission_2.GetComponent<Button>().onClick.RemoveAllListeners();
        if ( _btnMission_3 != null ) _btnMission_3.GetComponent<Button>().onClick.RemoveAllListeners();
    }

    public virtual void StartBook()
    {
        // Mostra os botões das missões
        for ( var i = 0; i < _missionsCleared.Length; i++ )
        {
            _btnsMission[i].SetActive( true );
            _btnsMission[i].GetComponent<Animator>().SetTrigger( "Show" );

            if ( !_missionsCleared[i] )
            {
                _btnsMission[i].GetComponent<Button>().interactable = true;
            }
        }
    }

    protected virtual void PlayInstructionAudio( int type )
    {
        activityUnlocked[ type - 1 ] = true;

        _audioSource.clip = _instructionsAr[type - 1];
        _audioSource.Play();

        // Mostra os textos, se existirem
        if ( _textsMissionAr.Length > 0 && _textsMissionAr[type - 1] != "" )
        {
            _textMission.SetActive( true );
            _textMission.GetComponent<Animator>().SetTrigger( "Show" );
            _textMission.transform.Find( "Text" ).gameObject.GetComponent<Text>().text = _textsMissionAr[type - 1];
        }
    }


    protected IEnumerator PlayFeedbackAudio( int type, float delay = 2f )
    {
        if ( _textsMissionAr.Length > 0 )
            _textMission.GetComponent<Animator>().SetTrigger( "Reset" );

        yield return new WaitForSeconds( delay );
        _audioSource.clip = _feedbackAr[type - 1];
        _audioSource.Play();

        _isAnimating = false;
    }

    protected virtual void ClearMission( int type, bool playFeedback = true, float delay = 2f )
    {
        switch ( type )
        {
            case 1:
                _btnMission_1.GetComponent<Button>().interactable = false;
                break;
            case 2:
                _btnMission_2.GetComponent<Button>().interactable = false;
                break;
            case 3:
                _btnMission_3.GetComponent<Button>().interactable = false;
                break;
        }

        _missionsCleared[type - 1] = true;

        if ( playFeedback )
            StartCoroutine( PlayFeedbackAudio( type, delay ) );
    }

    protected void RandomizeCanvasImages()
    {
        // Sorteia um dos fundos  sem repetir
        int[] spritesOptions = new[] {0, 1, 2, 3, 4, 5};
        Shuffle( spritesOptions );

        _btnMission_1.GetComponent<Image>().sprite =
            _btnMission_1.GetComponent<SpritesList>().spritesAr[spritesOptions[0]];
        _btnMission_2.GetComponent<Image>().sprite =
            _btnMission_2.GetComponent<SpritesList>().spritesAr[spritesOptions[1]];
        _btnMission_3.GetComponent<Image>().sprite =
            _btnMission_3.GetComponent<SpritesList>().spritesAr[spritesOptions[2]];

        // Sorteia um dos fundos
        int[] spritesMissionTextOptions = new[] {0, 1, 2};
        Shuffle( spritesMissionTextOptions );
        _textMission.GetComponent<Image>().sprite =
            _textMission.GetComponent<SpritesList>().spritesAr[spritesMissionTextOptions[0]];

        void Shuffle( int[] arr )
        {
            for ( int t = 0; t < arr.Length; t++ )
            {
                int tmp = arr[t];
                int r = UnityEngine.Random.Range( t, arr.Length );
                arr[t] = arr[r];
                arr[r] = tmp;
            }
        }
    }

    protected bool IsInArray( GameObject checkValue, GameObject[] array )
    {
        foreach ( var val in array )
        {
            if ( val == checkValue ) return true;
        }

        return false;
    }

    protected bool IsFinished()
    {
        foreach ( var mission in _missionsCleared )
        {
            if ( !mission ) return false;
        }

        return true;
    }

    protected virtual void ResetProgress()
    {
        _missionsCleared = new[] {false, false, false};
        activityUnlocked = new[] {false, false, false};
    }
}