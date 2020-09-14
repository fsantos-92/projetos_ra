using App.Managers;
using UnityEngine;
namespace App.Screens
{
    public class NavigationScreen
    {

        public bool inGame;
        public bool inMenu;
        public bool inAbout;
        public bool inQuit;

        #region Button Functions
        // public void BoolInstructions(bool value) { inInstructions = value; gm.screenMenu.HowToPlayDefault(); }
        public void BoolGame(bool value) { inGame = value; }
        public void BoolAbout(bool value) { inAbout = value; }
        public void BoolQuit(bool value) { inQuit = value; }

        public void EscapeNavigation()
        {
            if (inGame) BoolGame(false);
            else if (!inGame)
            {
                if (inAbout) BoolAbout(false);
                else BoolQuit(!inQuit);
            }
        }
        #endregion
    }
}