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

        private void Awake()
        {
            OnShow();
        }

        private void OnShow()
        {
            InitDropdown();

            _okButton.onClick.AddListener(MakeSelection);
        }

        private void OnDisable()
        {
            _okButton.onClick.RemoveAllListeners();
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
            OnShow();

            gameObject.SetActive(true);
        }

        private void Hide()
        {
            gameObject.SetActive(false);
        }

        private void MakeSelection()
        {
            var selectedPlayer = (PlayerType)_dropdown.value;

            OnPlayerSelected?.Invoke(selectedPlayer);

            Hide();
        }
    }
}