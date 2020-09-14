using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Cie02_manager : BaseBookManager, IClickInteraction
{
    private GameObject seedling;
    private GameObject waterCan;
    private GameObject plantedSeedling;
    private GameObject blueBird;
    private GameObject pinkBird;
    [SerializeField] private GameObject question;

    private string[] plantsAnim;
    private int plantType;

    private int missionState;

    protected override void InitializeVariables()
    {
        base.InitializeVariables();

        plantsAnim = new[] {"rose", "sunflower", "tulip"};
        //    plantType = UnityEngine.Random.Range( 0, plantsAnim.Length ); // sorteia a flor a ser plantada
        plantType = 1;

        if ( seedling )
            // toca a animacao "Idle" da planta sorteada
            seedling.GetComponent<Animator>().Play( string.Join(
                "",
                new string[] {"seedling_", plantsAnim[plantType], "_idle"} )
            );

        // esconde os beija-flores
        if ( blueBird )
            blueBird.SetActive( false );
        if ( pinkBird )
            pinkBird.SetActive( false );

        missionState = 0;
    }

    public override void StartBook()
    {
        foreach ( Transform element in transform.GetComponentsInChildren<Transform>() )
        {
            if ( element.name == ( "Seedling" ) )
                seedling = element.gameObject;
            if ( element.name == ( "WaterCan" ) )
                waterCan = element.gameObject;
            if ( element.name == ( "PlantedSeedling" ) )
                plantedSeedling = element.gameObject;
            if ( element.name == ( "Blue_bird" ) )
                blueBird = element.gameObject;
            if ( element.name == ( "Pink_bird" ) )
                pinkBird = element.gameObject;
        }

        // Mostra os botões das missões
        for ( var i = 0; i < _missionsCleared.Length; i++ )
        {
            _btnsMission[i].SetActive( true );
            _btnsMission[i].GetComponent<Animator>().SetTrigger( "Show" );

            _btnsMission[i].GetComponent<Button>().interactable = ( i == missionState );
        }

        if ( missionState >= 1 )
        {
            seedling.GetComponent<Animator>().Play( string.Join(
                "",
                new string[] {"seedling_", plantsAnim[1], "_disappear"} )
            );
            plantedSeedling.GetComponent<Animator>().Play( string.Join(
                "",
                new string[] {"plantedSeedling_", plantsAnim[1], "_appear"} )
            );
        }
    }

    protected override void BindButtonEvents()
    {
        _btnMission_1.GetComponent<Button>().onClick.AddListener( delegate { PlayInstructionAudio( 1 ); } );

        var btn2 = _btnMission_2.GetComponent<Button>();
        btn2.interactable = false;
        btn2.onClick.AddListener( delegate { PlayInstructionAudio( 2 ); } );

        var btn3 = _btnMission_3.GetComponent<Button>();
        btn3.interactable = false;
        btn3.onClick.AddListener( delegate { OpenQuestion(); } );
    }

    public void ClickInteraction( GameObject obj )
    {
        if ( _isAnimating ) return;

        Animator objAnim = obj.GetComponent<Animator>();
        AudioSource objAudio = obj.GetComponent<AudioSource>();

        if ( !IsFinished() )
        {
            if ( obj == seedling && missionState == 0 && activityUnlocked[0] )
            {
                seedling.GetComponent<AudioSource>().Play();
                seedling.GetComponent<Animator>().Play( string.Join(
                    "",
                    new string[] {"seedling_", plantsAnim[plantType], "_disappear"} )
                );
                Invoke( "PlantFlower", 1 );

                missionState = 1;

                _btnMission_1.GetComponent<Button>().interactable = false;
                _btnMission_2.GetComponent<Button>().interactable = true;
            }
            else if ( obj == waterCan && missionState == 1 && activityUnlocked[1] )
            {
                waterCan.GetComponent<Animator>().Play( "waterCan_disappear" );
                Invoke( "FlyBirds", 1 );

                missionState = 2;

                _btnMission_2.GetComponent<Button>().interactable = false;
                _btnMission_3.GetComponent<Button>().interactable = true;
            }
        }
    }

    // Animacao da planta aparecer no buraco
    private void PlantFlower()
    {
        plantedSeedling.GetComponent<AudioSource>().Play();

        plantedSeedling.GetComponent<Animator>().Play( string.Join(
            "",
            new string[] {"plantedSeedling_", plantsAnim[plantType], "_appear"} )
        );

        ClearMission( 1 );
    }

    private void FlyBirds()
    {
        pinkBird.SetActive( true );
        blueBird.SetActive( true );

        pinkBird.GetComponent<Animator>().Play( "Fly" );
        blueBird.GetComponent<Animator>().Play( "Fly" );

        pinkBird.GetComponent<AudioSource>().Play();
        blueBird.GetComponent<AudioSource>().Play();

        missionState = 2;

        ClearMission( 2 );
    }

    // Guarda a alternativa correta e abre a tela da pergunta
    private void OpenQuestion()
    {
        var qm = question.GetComponent<Cie02_questionManager>();
        qm.SetCorrectAnswer( plantType );
        qm.ShowQuestion();

        missionState = 3;

        PlayInstructionAudio( 3 );
    }

    public void AnswerQuestion()
    {
        ClearMission( 3 );
    }

    protected override void ResetProgress()
    {
        base.ResetProgress();

        InitializeVariables();

        question.GetComponent<Cie02_questionManager>().lockQuestion = false;
        question.transform.parent.gameObject.SetActive( false );
    }
}