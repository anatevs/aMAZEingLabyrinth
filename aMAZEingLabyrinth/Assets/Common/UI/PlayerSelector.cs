using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using GameCore;
using System.Collections.Generic;
using System.Linq;

namespace GameUI
{
    public sealed class PlayerSelector : MonoBehaviour
    {
        public event Action<int> OnPlayerSelected;

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

        public void Show(ICollection<PlayerType> players)
        {
            InitDropdown(players);

            _okButton.onClick.AddListener(MakeSelection);

            gameObject.SetActive(true);
        }

        public void InitDropdown(ICollection<PlayerType> players)
        {
            _dropdown.options.Clear();

            _dropdown.RefreshShownValue();

            foreach (var player in players)
            {
                _dropdown.options.Add(new(player.ToString()));
            }
        }

        private void Hide()
        {
            gameObject.SetActive(false);
        }

        private void InitDropdown()
        {
            _dropdown.options.Clear();

            _dropdown.RefreshShownValue();

            foreach (var playerName in Enum.GetValues(typeof(PlayerType)))
            {
                _dropdown.options.Add(new(playerName.ToString()));
            }
        }

        private void MakeSelection()
        {
            OnPlayerSelected?.Invoke(_dropdown.value);

            Hide();
        }
    }
}