using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cie01_manager : BaseBookManager, IClickInteraction
{
    private GameObject YellowBag;
    private GameObject YellowBagPos;
    private GameObject TabletGuy;
    private GameObject Notification;

    private List<GameObject> _bushes;
    private GameObject[] BushInteracted;
    bool _feedbackPlayed;

    public override void StartBook()
    {
        base.StartBook();

        foreach ( Transform element in transform.GetComponentsInChildren<Transform>() )
        {
            if ( element.name == ( "BagCie01" ) )
                YellowBag = element.gameObject;
            if ( element.name == ( "BagSpotCie01" ) )
                YellowBagPos = element.gameObject;
            if ( element.name == ( "TabletGuyCie01" ) )
                TabletGuy = element.gameObject;
            if ( element.name == ( "NotificationCie01" ) )
                Notification = element.gameObject;
        }
    }

    void UpdateBagPosition()
    {
        this.YellowBag.transform.position = this.YellowBagPos.transform.position;
        this.YellowBag.GetComponent<SpriteRenderer>().sortingOrder = 4;
    }

    protected override void InitializeVariables()
    {
        base.InitializeVariables();
        _bushes = new List<GameObject>();
        _feedbackPlayed = false;
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

        ClearMission( mission, false );
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
            if ( obj == YellowBag && activityUnlocked[0] )
            {
                AnimateObj( obj, objAnim, 1 );
                Invoke( "UpdateBagPosition", 1 );
            }
            else if ( obj == TabletGuy && activityUnlocked[1]  )
            {
                AnimateObj( obj, objAnim, 2 );

                Notification.GetComponent<Animator>().SetTrigger( "Play" );
            }
            else if ( objName.Contains( "arbusto" ) && activityUnlocked[2]  )
            {
                _isAnimating = true;
                objAnim.SetTrigger( "Play" );
                obj.GetComponent<AudioSource>().Play();
                obj.transform.GetChild( 0 ).GetComponent<Animator>().SetTrigger( "Play" );

                GameObject[] auxArray = _bushes.ToArray();
                if ( !IsInArray( obj, auxArray ) )
                {
                    _bushes.Add( obj );
                }

                if ( _bushes.Count >= 3 )
                    ClearMission( 3, false );
                Invoke( "ResetAnim", 2 );
            }
        }

        int count = 0;
        foreach ( bool element in _missionsCleared )
        {
            if ( element )
                count++;
        }

        if ( count >= 3 && !_feedbackPlayed )
        {
            ClearMission( 1 );
            _feedbackPlayed = true;
        }
    }
}