using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using GameCore;
using System.Collections.Generic;

namespace GameUI
{
    public sealed class PlayerSelector : MonoBehaviour
    {
        public event Action<int> OnOkClicked;

        [SerializeField]
        private TMP_Dropdown _dropdown;

        [SerializeField]
        private Button _okButton;

        private void OnDisable()
        {
            _okButton.onClick.RemoveAllListeners();
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

        private void MakeSelection()
        {
            OnOkClicked?.Invoke(_dropdown.value);

            Hide();
        }
    }
}