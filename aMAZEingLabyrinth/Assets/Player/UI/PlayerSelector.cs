using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace GameCore
{
    public sealed class PlayerSelector : MonoBehaviour
    {
        public event Action<PlayerType> OnPlayerSelected;

        [SerializeField]
        private GameObject _window;

        [SerializeField]
        private TMP_Dropdown _dropdown;

        [SerializeField]
        private Button _button;

        private void Awake()
        {
            InitDropdown();

            _button.onClick.AddListener(MakeSelection);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveAllListeners();
        }

        private void InitDropdown()
        {
            _dropdown.options = new();

            foreach(var playerName in Enum.GetValues(typeof(PlayerType)))
            {
                _dropdown.options.Add(new(playerName.ToString()));
            }
        }

        public void Show()
        {
            _window.SetActive(true);
        }

        public void Hide()
        {
            _window.SetActive(false);
        }

        private void MakeSelection()
        {
            var selectedPlayer = (PlayerType)_dropdown.value;

            OnPlayerSelected?.Invoke(selectedPlayer);

            Hide();
        }
    }
}