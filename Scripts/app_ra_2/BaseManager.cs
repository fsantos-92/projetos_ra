using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class BaseManager : MonoBehaviour
{
  public static BaseManager instance;

  [SerializeField] private AudioClip instruction_1;
  [SerializeField] private AudioClip instruction_2;
  [SerializeField] private AudioClip instruction_3;

  [SerializeField] private AudioClip feedback_1;
  [SerializeField] private AudioClip feedback_2;
  [SerializeField] private AudioClip feedback_3;
  
  [SerializeField] private GameObject _uiCanvas;
  [SerializeField] private String _activityText_1;
  [SerializeField] private String _activityText_2;
  [SerializeField] private String _activityText_3;
  private Transform _btnMission_1;
  private Transform _btnMission_2;
  private Transform _btnMission_3;
  
  private AudioClip[] _instructionsAr;
  private AudioClip[] _feedbackAr;
  private AudioSource _audioSource;
  

  private bool _isFinished;
  private bool _isAnimating;

  [HideInInspector] public int managerState;

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
    
    managerState = 0;

    _audioSource = GetComponent<AudioSource>();
    _instructionsAr = new[] { instruction_1, instruction_2, instruction_3 };
    _feedbackAr = new[] { feedback_1, feedback_2, feedback_3 };

    _isFinished = false;
    _isAnimating = false;
  }

  public void PlayInstructionAudio( )
  {
    if ( _isFinished ) return;

    _audioSource.clip = _instructionsAr[ managerState ];
    _audioSource.Play();
  }

  /***
   * Toca o audio de feedback
   */
  public void PlayFeedbackAudio( )
  {
    if ( _isFinished ) return;

    _audioSource.clip = _feedbackAr[ managerState ];
    _audioSource.Play();

    managerState++;
    _isAnimating = false;

    if ( managerState > 2 ) _isFinished = true;
  }

  public bool IsInArray( GameObject checkValue, GameObject[] array )
  {
    foreach ( var val in array )
    {
      if ( val == checkValue ) return true;
    }

    return false;
  }

  private void RandomizeCanvasImages( )
  {
    int[] spritesOptions = new[] { 0, 1, 2, 3, 4, 5 };
    Shuffle( spritesOptions );

    _btnMission_1.GetComponent<Image>().sprite = _btnMission_1.GetComponent<SpritesList>().spritesAr[ spritesOptions[0] ];
    _btnMission_2.GetComponent<Image>().sprite = _btnMission_2.GetComponent<SpritesList>().spritesAr[ spritesOptions[1] ];
    _btnMission_3.GetComponent<Image>().sprite = _btnMission_3.GetComponent<SpritesList>().spritesAr[ spritesOptions[2] ];
  }
  
  void Shuffle(int[] arr)
  {
    for (int t = 0; t < arr.Length; t++)
    {
      int tmp = arr[t];
      int r = Random.Range(t, arr.Length);
      arr[t] = arr[r];
      arr[r] = tmp;
    }
  }
}