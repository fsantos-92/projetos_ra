using UnityEngine;

namespace App.Tools
{
    public class ModelMovement : MonoBehaviour
    {
        Vector2 _firstPressPos;
        Vector2 _secondPressPos;
        public Vector2 currentSwipe;

        public float sensibility = 0.1f;
        void Update()
        {
            if (Input.GetMouseButtonDown(1)) TouchDown();
            if (Input.GetMouseButton(1)) Swiping();
            if (Input.GetMouseButtonUp(1)) TouchUp();
        }

        void UpdatePosition()
        {
            float PosX = this.currentSwipe.x * this.sensibility;
            float PosY = this.currentSwipe.y * this.sensibility;

            Vector2 translation = new Vector2(PosX, PosY);
            transform.Translate(translation, Space.World);
        }

        public void TouchDown()
        {
            this._firstPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }

        public void Swiping()
        {
            if (Input.GetMouseButton(1))
                this._secondPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            if (Mathf.Abs(Vector2.Distance(this._firstPressPos, this._secondPressPos)) > 5f)
            {
                this.currentSwipe = new Vector2(this._secondPressPos.x - this._firstPressPos.x, this._secondPressPos.y - this._firstPressPos.y);
                this.UpdatePosition();
                this._firstPressPos = this._secondPressPos;
            }
        }

        public void TouchUp()
        {
            this.currentSwipe = new Vector2(0, 0);
        }
    }
}
