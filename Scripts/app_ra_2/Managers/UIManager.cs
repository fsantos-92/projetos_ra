using App.Screens;
using UnityEngine;
using UnityEngine.UI;

namespace App.Managers
{
    public class UIManager : BaseManager<UIManager>
    {
        private RectTransform _canvas;
        public RectTransform Canvas { get => _canvas; }
        private NavigationScreen _navigation;
        public NavigationScreen Navigation { get => _navigation; }

        private void Start() {
            this._canvas = GetComponent<RectTransform>();
        }
    }
}
