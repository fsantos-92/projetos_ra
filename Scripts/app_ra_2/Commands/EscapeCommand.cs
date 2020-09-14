using App.Commands;
using App.Managers;
using UnityEngine;

namespace App.Commands {
    public class EscapeCommand : Command {
        public override void Execute() => UIManager.Instance.Navigation.EscapeNavigation();
    }
}
