using UnityEngine;

public class His02_manager : BaseBookManager, IClickInteraction
{
    private GameObject boxGirl;
    private GameObject tabletTrio;
    private GameObject shelfGirl;

    public override void StartBook()
    {
        base.StartBook();

        foreach ( Transform element in transform.GetComponentsInChildren<Transform>() )
        {
            if ( element.name == ( "BoxGirl" ) )
                boxGirl = element.gameObject;
            if ( element.name == ( "TabletTrio" ) )
                tabletTrio = element.gameObject;
            if ( element.name == ( "ShelfGirl" ) )
                shelfGirl = element.gameObject;
        }
    }

    public void ClickInteraction( GameObject obj )
    {
        if ( _isAnimating ) return;

        Animator objAnim = obj.GetComponent<Animator>();
        AudioSource objAudio = obj.GetComponent<AudioSource>();

        if ( objAnim.GetCurrentAnimatorStateInfo( 0 ).IsName( "Idle" ) )
        {
            if ( !IsFinished() )
            {
                if ( !_missionsCleared[0] && obj == boxGirl && activityUnlocked[0] )
                {
                    PlayAnimation();
                    ClearMission( 1 );
                }
                else if ( !_missionsCleared[1] && obj == tabletTrio && activityUnlocked[1] )
                {
                    PlayAnimation();
                    ClearMission( 2 );
                }
                else if ( !_missionsCleared[2] && obj == shelfGirl && activityUnlocked[2] )
                {
                    PlayAnimation();
                    ClearMission( 3 );
                }
            }
            else
            {
                PlayAnimation();
            }
        }

        void PlayAnimation()
        {
            objAnim.SetTrigger( "Play" );
            objAudio.Play();
        }
    }
}