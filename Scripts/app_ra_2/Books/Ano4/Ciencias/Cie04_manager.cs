using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cie04_manager : BaseBookManager, IClickInteraction
{
    GameObject Plant;
    GameObject Dog;
    GameObject Butterfly;

    bool[] _canPlayFeedback;

    public override void StartBook()
    {
        base.StartBook();

        foreach ( Transform element in transform.GetComponentsInChildren<Transform>() )
        {
            if ( element.name == ( "Plant" ) )
                Plant = element.gameObject;
            if ( element.name == ( "Cachorro" ) )
                Dog = element.gameObject;
            if ( element.name == ( "Borboleta" ) )
                Butterfly = element.gameObject;
        }
    }

    protected override void InitializeVariables()
    {
        base.InitializeVariables();
        _canPlayFeedback = new bool[3] {true, true, true};
    }

    void ResetAnim()
    {
        _isAnimating = false;
    }

    void AnimateObj( GameObject obj, Animator objAnim, int mission )
    {
        _isAnimating = true;
        objAnim.SetTrigger( "Play" );
        obj.GetComponent<AudioSource>().Play();

        ClearMission( mission, _canPlayFeedback[mission - 1] );
        Invoke( "ResetAnim", 2 );
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
            if ( obj == Plant  && activityUnlocked[0] )
            {
                AnimateObj( obj, objAnim, 1 );
            }
            else if ( obj == Dog  && activityUnlocked[1] )
            {
                AnimateObj( obj, objAnim, 2 );
            }
            else if ( obj == Butterfly  && activityUnlocked[2] )
            {
                AnimateObj( obj, objAnim, 3 );
            }
        }
    }
}