using App.Commands;
using UnityEngine;
namespace App.Core {
    public class InputHandler {

        private Command touchDownCommand;
        private Command swipeCommand;
        private Command touchUpCommand;
        private Command escapeCommand;

        public InputHandler() {
            this.touchDownCommand = new TouchDownCommand();
            this.swipeCommand = new SwipeCommand();
            this.touchUpCommand = new TouchUpCommand();
            this.escapeCommand = new EscapeCommand();
        }

        public Command Handle() {
            if (Input.GetMouseButtonDown(0)) return this.touchDownCommand;
            if (Input.GetMouseButton(0)) return this.swipeCommand;
            if (Input.GetMouseButtonUp(0)) return this.touchUpCommand;
            if (Input.GetKeyUp("escape")) return this.escapeCommand;

            return null; //TODO: add input logics to return command
        }

    }
}