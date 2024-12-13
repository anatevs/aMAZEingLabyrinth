using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using GameCore;

namespace GameUI
{
    public sealed class PlayerSelector : MonoBehaviour
    {
        public event Action<PlayerType> OnPlayerSelected;

        [SerializeField]
        private TMP_Dropdown _dropdown;

        [SerializeField]
        private Button _okButton;

        private void OnDisable()
        {
            _okButton.onClick.RemoveAllListeners();
        }

        public void Show()
        {
            InitDropdown();

            _okButton.onClick.AddListener(MakeSelection);

            gameObject.SetActive(true);
        }

        private void Hide()
        {
            gameObject.SetActive(false);
        }

        private void InitDropdown()
        {
            _dropdown.options = new();

            //make here reference to playersList and init from it

            foreach (var playerName in Enum.GetValues(typeof(PlayerType)))
            {
                _dropdown.options.Add(new(playerName.ToString()));
            }
        }

        private void MakeSelection()
        {
            var selectedPlayer = (PlayerType)_dropdown.value;

            OnPlayerSelected?.Invoke(selectedPlayer);

            Hide();
        }
    }
}