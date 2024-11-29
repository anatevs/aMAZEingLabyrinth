using UnityEngine;

namespace GameUI
{
    public sealed class MenusService : MonoBehaviour
    {
        public PlayerSelector PlayerSelector => _playerSelector;
        public NoPathMessageWindow NoPathMenu => _noPathMessageWindow;

        [SerializeField]
        private PlayerSelector _playerSelector;

        [SerializeField]
        private NoPathMessageWindow _noPathMessageWindow;
    }
}