using App.Commands;
using App.Managers;

namespace App.Commands {
    public class SwipeCommand : Command {
        public override void Execute() => GameManager.Instance.Swipe.Swiping();
    }
}
