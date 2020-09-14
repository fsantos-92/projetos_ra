using UnityEngine;

public class His05_manager : BaseBookManager, IClickInteraction
{
    private GameObject tabletBoy;
    private GameObject oldPainting;
    private GameObject sabertoothPainting;

    public override void StartBook()
    {
        base.StartBook();

        foreach (Transform element in transform.GetComponentsInChildren<Transform>())
        {
            if (element.name == ("TabletBoy"))
                tabletBoy = element.gameObject;
            if (element.name == ("OldPainting"))
                oldPainting = element.gameObject;
            if (element.name == ("SabertoothPainting"))
                sabertoothPainting = element.gameObject;
        }
    }

    public void ClickInteraction( GameObject obj )
    {
        if ( _isAnimating ) return;

        Animator objAnim = obj.GetComponent<Animator>();
        AudioSource objAudio = obj.GetComponent<AudioSource>();
        
        Debug.Log( obj  );

        if ( !IsFinished() )
        {
            if ( objAnim.GetCurrentAnimatorStateInfo( 0 ).IsName( "Idle" ) )
            {
                if ( !_missionsCleared[ 0 ] && obj == tabletBoy && activityUnlocked[0] )
                {
                    _isAnimating = true;

                    objAudio.Play();
                    objAnim.SetTrigger( "Play" );

                    ClearMission( 1 );
                }
                else if ( !_missionsCleared[ 1 ] && obj == oldPainting && activityUnlocked[1] )
                {
                    _isAnimating = true;

                    objAudio.Play();
                    objAnim.SetTrigger( "Play" );

                    ClearMission( 2 );
                }
                else if ( !_missionsCleared[ 2 ] && obj == sabertoothPainting && activityUnlocked[2] )
                {
                    _isAnimating = true;

                    objAudio.Play();
                    objAnim.SetTrigger( "Play" );

                    ClearMission( 3 );
                }
            }
        }
        else
        {
            objAnim.SetTrigger( "Play" );
            objAudio.Play();
        }
    }
}