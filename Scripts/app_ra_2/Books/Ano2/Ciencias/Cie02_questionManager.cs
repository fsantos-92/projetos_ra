using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cie02_questionManager : MonoBehaviour
{
  public static Cie02_questionManager instance;

  [SerializeField] private AudioClip feedback_3;
  [SerializeField] private Button btn_1;
  [SerializeField] private Button btn_2;
  [SerializeField] private Button btn_3;
  
  [SerializeField] private GameObject manager;

  private Animator questionAnim;
  private AudioSource questionAudio;
  private GameObject questionPanel;
  private int correctAnswer;
  public bool lockQuestion = false;

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

    btn_1.onClick.AddListener( ( ) => ClickAnswer( 0 ) );
    btn_2.onClick.AddListener( ( ) => ClickAnswer( 1 ) );
    btn_3.onClick.AddListener( ( ) => ClickAnswer( 2 ) );
  }

  public void SetCorrectAnswer( int correct )
  {
    correctAnswer = correct;
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
      
      manager.GetComponent<Cie02_manager>().AnswerQuestion();
      
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