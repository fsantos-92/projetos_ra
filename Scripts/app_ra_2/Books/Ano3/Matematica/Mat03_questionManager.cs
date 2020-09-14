using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mat03_questionManager : MonoBehaviour
{
  public static Mat03_questionManager instance;

  [SerializeField] private Button _btn_1;
  [SerializeField] private Button _btn_2;
  [SerializeField] private Button _btn_3;
  [SerializeField] private GameObject _manager;

  private Animator questionAnim;
  private AudioSource questionAudio;
  private GameObject questionPanel;
  private int correctAnswer;
  public bool lockQuestion;
  private AudioClip[] feedbackAr;

  void Awake( )
  {
    MakeInstance();
    InitializeVariables();
  }

  void MakeInstance( )
  {
    if ( instance == null ) instance = this;
  }

  void InitializeVariables( )
  {
    questionAnim = GetComponent<Animator>();
    questionAudio = GetComponent<AudioSource>();

    _btn_1.onClick.AddListener( ( ) => ClickAnswer( 0 ) );
    _btn_2.onClick.AddListener( ( ) => ClickAnswer( 1 ) );
    _btn_3.onClick.AddListener( ( ) => ClickAnswer( 2 ) );

    correctAnswer = 1;

    lockQuestion = false;
  }

  public void ShowQuestion( )
  {
    questionAnim.Play( "Show" );
  }

  private void ClickAnswer( int num )
  {
    if ( lockQuestion ) return;

    var btn = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
    var btnAnim = btn.GetComponent<Animator>();

    if ( num == correctAnswer )
    {
      btnAnim.SetTrigger( "Right" );
      lockQuestion = true;

      _manager.GetComponent<Mat03_manager>().AnswerQuestion();

      Invoke( "HideQuestion", 2 );
    }
    else
    {
      btnAnim.SetTrigger( "Wrong" );
    }
  }

  public void HideQuestion( )
  {
    questionAnim.Play( "Hide" );
  }
}