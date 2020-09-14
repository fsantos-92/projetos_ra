using App.Commands;
using App.Managers;

namespace App.Commands {
    public class TouchUpCommand : Command {
        public override void Execute() => GameManager.Instance.Swipe.TouchUp();
    }
}
