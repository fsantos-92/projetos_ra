using App.Commands;
using App.Managers;

namespace App.Commands {
    public class TouchDownCommand : Command {
        public override void Execute() => GameManager.Instance.Swipe.TouchDown();
    }
}
