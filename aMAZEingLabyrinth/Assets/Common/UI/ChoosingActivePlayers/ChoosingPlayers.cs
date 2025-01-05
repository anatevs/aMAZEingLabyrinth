using GameCore;
using System;
using UnityEngine;

namespace GameUI
{
    public class ChoosingPlayers : MonoBehaviour
    {
        public event Action<PlayerType, bool> OnPlayerToggled;

        [SerializeField]
        private PlayerTypeToggleView[] _toggleViews;

        private int _selectedAmount;

        private void Awake()
        {
            var playerNames = Enum.GetValues(typeof(PlayerType));

            if (playerNames.Length != _toggleViews.Length)
            {
                throw new Exception($"amount of player types" +
                    $" {playerNames.Length} is not equals amount" +
                    $" of toggles {_toggleViews.Length}");
            }

            for (int i = 0; i < _toggleViews.Length; i++)
            {
                _toggleViews[i].SetPlayer((PlayerType)i);
            }

            _selectedAmount = _toggleViews.Length;
        }

        private void OnEnable()
        {
            for (int i = 0; i < _toggleViews.Length; i++)
            {
                _toggleViews[i].OnToggleChanged += ChooseToggle;
            }

            _selectedAmount = _toggleViews.Length;
        }

        private void OnDisable()
        {
            for (int i = 0; i < _toggleViews.Length; i++)
            {
                _toggleViews[i].OnToggleChanged -= ChooseToggle;
            }
        }

        private void ChooseToggle(PlayerType playerType, bool isActive)
        {
            if (!isActive)
            {
                _selectedAmount--;

                if (_selectedAmount <= 0)
                {
                    _toggleViews[(int)playerType].SetTrue();
                }
            }
            else
            {
                _selectedAmount++;
            }

            OnPlayerToggled?.Invoke(playerType, isActive);
        }
    }
}