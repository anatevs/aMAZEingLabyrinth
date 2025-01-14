using GameCore;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace GameUI
{
    public sealed class PlayerTypeToggleView : MonoBehaviour
    {
        public event Action<PlayerType, bool> OnToggleChanged;

        [SerializeField]
        private Toggle _toggle;

        [SerializeField]
        private Text _label;

        private PlayerType _playerType;

        public void OnEnable()
        {
            _toggle.onValueChanged.AddListener(SetToggle);
        }

        public void OnDisable()
        {
            _toggle.onValueChanged.RemoveAllListeners();
        }

        public void SetPlayer(PlayerType playerType)
        {
            _playerType = playerType;

            _label.text = playerType.ToString();
        }

        public void SetOn(bool isOn)
        {
            _toggle.isOn = isOn;
        }

        private void SetToggle(bool value)
        {
            OnToggleChanged?.Invoke(_playerType, value);
        }
    }
}