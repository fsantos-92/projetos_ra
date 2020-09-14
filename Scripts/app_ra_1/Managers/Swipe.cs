using UnityEngine;
public class Swipe : MonoBehaviour
{
    Vector2 _firstPressPos;
    Vector2 _secondPressPos;
    public Vector2 currentSwipe;

    public Navigation[] navigations;
    

    public bool touching;

    private void Update() {
            if (Input.GetMouseButtonDown(0)) TouchDown();
            if (Input.GetMouseButton(0)) Swiping();
            if (Input.GetMouseButtonUp(0)) TouchUp();
    }

    public void TouchDown()
    {
        this._firstPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        this.touching = true;
    }

    public void Swiping()
    {
        if (Input.GetMouseButton(0))
        this._secondPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        this.currentSwipe = new Vector2(this._secondPressPos.x - this._firstPressPos.x, this._secondPressPos.y - this._firstPressPos.y);
        for (int i = 0; i < navigations.Length; i++)
        {
            navigations[i].Swipe(currentSwipe);
        }
    }

    public void TouchUp()
    {
        this.currentSwipe = new Vector2(0, 0);
        this.touching = false;
        for (int i = 0; i < navigations.Length; i++)
        {
            navigations[i].TouchUp();
        }
    }
}
