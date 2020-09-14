using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Geo02_manager : BaseBookManager, IClickInteraction
{
    private GameObject telescope;
    private GameObject picture;
    private GameObject petshop;
    private GameObject hospital;
    bool[] _canPlayFeedback;

    public override void StartBook()
    {
        base.StartBook();

        foreach ( Transform element in transform.GetComponentsInChildren<Transform>() )
        {
            if ( element.name == "telescope" )
                telescope = element.gameObject;
            if ( element.name == "Picture" )
                picture = element.gameObject;
            if ( element.name == "petshop" )
                petshop = element.gameObject;
            if ( element.name == "helicopter" )
                hospital = element.gameObject;
        }
    }

    protected override void InitializeVariables()
    {
        base.InitializeVariables();
        _canPlayFeedback = new bool[3] {true, true, false};
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
        _canPlayFeedback[mission - 1] = false;
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
            if ( obj == telescope && activityUnlocked[0] )
            {
                AnimateObj( obj, objAnim, 1 );

                picture.GetComponent<Animator>().SetTrigger( "Play" );
            }
            else if ( obj == petshop && activityUnlocked[1] )
            {
                AnimateObj( obj, objAnim, 2 );
            }
            else if ( obj == hospital && activityUnlocked[2] )
            {
                AnimateObj( obj, objAnim, 3 );
            }
        }
    }
}