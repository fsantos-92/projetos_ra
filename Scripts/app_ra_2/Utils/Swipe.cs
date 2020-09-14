using App.Managers;
using UnityEngine;
namespace App.Utils {
    public class Swipe {
        Vector2 _firstPressPos;
        Vector2 _secondPressPos;
        public Vector2 currentSwipe;

        int sensibility = 3;
        
        public void TouchDown() {
            this._firstPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }

        public void Swiping() {
            this._secondPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            this.currentSwipe = new Vector2(this._secondPressPos.x - this._firstPressPos.x, this._secondPressPos.y - this._firstPressPos.y);
        }

        public void TouchUp() {
            this.currentSwipe = new Vector2(0, 0);
        }
    }
}
