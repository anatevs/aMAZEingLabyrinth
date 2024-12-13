using GameCore;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace GameUI
{
    public class PlayerTypeToggleView : MonoBehaviour
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

        public void SetToggle(bool value)
        {
            OnToggleChanged?.Invoke(_playerType, value);
        }

        public void SetLabel(string label)
        {
            _label.text = label;
        }

        public void SetPlayer(PlayerType playerType)
        {
            _playerType = playerType;

            SetLabel(playerType.ToString());
        }
    }
}