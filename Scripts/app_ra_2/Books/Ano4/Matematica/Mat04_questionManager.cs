using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mat04_questionManager : MonoBehaviour
{
    public static Mat04_questionManager instance;

    [SerializeField] private Button _btn_correct;
    public Button[] _btns_wrong;
    [SerializeField] private GameObject _manager;
    [SerializeField] private int missionNumber;

    private Animator questionAnim;
    private GameObject questionPanel;
    private bool lockQuestion;

    void Awake()
    {
        MakeInstance();
        InitializeVariables();
    }

    void MakeInstance()
    {
        if ( instance == null ) instance = this;
    }

    void InitializeVariables()
    {
        questionAnim = GetComponent<Animator>();

        if ( _btns_wrong.Length > 0 )
        {
            foreach ( var btn in _btns_wrong )
            {
                btn.onClick.AddListener( () => ClickAnswer( false ) );
            }
        }

        _btn_correct.onClick.AddListener( () => ClickAnswer( true ) );

        lockQuestion = false;
    }

    public void ShowQuestion()
    {
        questionAnim.Play( "Show" );
    }

    private void ClickAnswer( bool correctAnswer )
    {
        if ( lockQuestion ) return;

        var btn = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        var btnAnim = btn.GetComponent<Animator>();

        if ( correctAnswer )
        {
            btnAnim.SetTrigger( "Right" );
            lockQuestion = true;

            _manager.GetComponent<Mat04_manager>().AnswerQuestion( missionNumber );

            Invoke( "HideQuestion", 2 );
        }
        else
        {
            btnAnim.SetTrigger( "Wrong" );
        }
    }

    public void HideQuestion()
    {
        questionAnim.Play( "Hide" );
    }
}