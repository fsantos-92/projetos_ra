using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cie05_manager : BaseBookManager, IClickInteraction
{
    private GameObject moon;
    private GameObject binoculars;
    private GameObject lamp;

    private GameObject insects;

    [SerializeField] private GameObject[] constellations;
    private GameObject starsTrigger;
    private GameObject[] stars;

    [SerializeField] AudioClip constellationShow;
    [SerializeField] AudioClip constellationHide;
    [SerializeField] AudioClip starsGlow;

    private bool[] _canPlayFeedback;
    private int _currentConstellation;

    public override void StartBook()
    {
        base.StartBook();

        List<GameObject> _stars = new List<GameObject>();

        foreach ( Transform element in transform.GetComponentsInChildren<Transform>() )
        {
            if ( element.name == ( "Lua" ) )
                moon = element.gameObject;
            if ( element.name == ( "Binoculos" ) )
                binoculars = element.gameObject;
            if ( element.name == ( "Luz" ) )
                lamp = element.gameObject;
            if ( element.name == "Insetos" )
                insects = element.gameObject;
            if ( element.name == "StarTrigger" )
                starsTrigger = element.gameObject;
            if ( element.name.Contains( "Estrela" ) )
                _stars.Add( element.gameObject );
        }

        stars = _stars.ToArray();
    }

    protected override void InitializeVariables()
    {
        base.InitializeVariables();
        _canPlayFeedback = new bool[3] {true, true, true};
        _currentConstellation = 0;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        foreach ( GameObject element in constellations )
        {
            if ( element.GetComponent<Animator>().GetCurrentAnimatorStateInfo( 0 ).IsName( "Show" ) )
                element.gameObject.GetComponent<Animator>().SetTrigger( "Hide" );

            element.SetActive( false );
        }
    }

    void ResetAnim()
    {
        _isAnimating = false;
    }

    IEnumerator ShowCurrentConstellation()
    {
        yield return new WaitForSeconds( 0.5f );
        constellations[_currentConstellation].SetActive( true );
        constellations[_currentConstellation].GetComponent<Animator>().SetTrigger( "Play" );
        _audioSource.PlayOneShot( constellationShow );
        yield return new WaitForSeconds( 3f );
        constellations[_currentConstellation].GetComponent<Animator>().SetTrigger( "Hide" );
        _audioSource.PlayOneShot( constellationHide );
        yield return new WaitForSeconds( 1f );
        constellations[_currentConstellation].SetActive( false );

        _currentConstellation++;

        if ( _currentConstellation >= constellations.Length )
        {
            ClearMission( 2, _canPlayFeedback[1] );
            _canPlayFeedback[1] = false;
            _currentConstellation = 0;
        }
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
            if ( obj == moon && activityUnlocked[0] )
            {
                _isAnimating = true;
                objAnim.SetTrigger( "Play" );
                obj.GetComponent<AudioSource>().Play();

                ClearMission( 1, _canPlayFeedback[0] );
                _canPlayFeedback[0] = false;

                Invoke( "ResetAnim", 2 );
            }
            else if ( obj == binoculars && activityUnlocked[1]  )
            {
                _isAnimating = true;
                objAnim.SetTrigger( "Play" );
                obj.GetComponent<AudioSource>().Play();

                StartCoroutine( ShowCurrentConstellation() );

                Invoke( "ResetAnim", 5 );
            }
            else if ( obj == lamp && activityUnlocked[2] )
            {
                _isAnimating = true;
                objAnim.SetTrigger( "Play" );
                obj.GetComponent<AudioSource>().Play();

                insects.GetComponent<Animator>().SetTrigger( "Play" );
                insects.GetComponent<AudioSource>().Play();

                ClearMission( 3, _canPlayFeedback[2] );
                _canPlayFeedback[2] = false;
                Invoke( "ResetAnim", 2 );
            }
            else if ( obj == starsTrigger )
            {
                obj.GetComponent<AudioSource>().Play();
                _audioSource.PlayOneShot( starsGlow );
                foreach ( GameObject element in stars )
                {
                    Animator anim = element.GetComponent<Animator>();
                    float random = Random.Range( 0f, 1f );
                    Debug.Log( random );
                    // anim.SetTrigger("Play");
                    StartCoroutine( AnimStar( anim, random ) );
                }

                Invoke( "ResetAnim", 2 );
            }
        }
    }

    IEnumerator AnimStar( Animator anim, float waitTime )
    {
        yield return new WaitForSeconds( waitTime );
        anim.SetTrigger( "Play" );
    }


    protected override void ResetProgress()
    {
        base.ResetProgress();

        InitializeVariables();

        foreach ( var img in constellations )
        {
            img.SetActive( false );
        }
    }
}