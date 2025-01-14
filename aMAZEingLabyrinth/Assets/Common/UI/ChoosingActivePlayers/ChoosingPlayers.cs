using GameCore;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace GameUI
{
    public sealed class ChoosingPlayers : MonoBehaviour
    {
        public event Action<PlayerType, bool> OnPlayerToggled;

        [SerializeField]
        private PlayerTypeToggleView[] _toggleViews;

        private int _selectedAmount;

        public void Show(ICollection<PlayerType> players)
        {
            var playerNames = Enum.GetValues(typeof(PlayerType));

            if (playerNames.Length != _toggleViews.Length)
            {
                throw new Exception($"amount of player types" +
                    $" {playerNames.Length} is not equals amount" +
                    $" of toggles {_toggleViews.Length}");
            }

            var playersNumbers = new List<int>(Enumerable.Range(0, _toggleViews.Length));

            foreach (var toggledPl in players)
            {
                var number = (int)toggledPl;

                InitToggleView(number, true);

                playersNumbers.Remove(number);
            }

            foreach (var number in playersNumbers)
            {
                InitToggleView(number, false);
            }

            _selectedAmount = players.Count;
        }

        private void OnDisable()
        {
            for (int i = 0; i < _toggleViews.Length; i++)
            {
                _toggleViews[i].OnToggleChanged -= ChooseToggle;
            }
        }

        private void InitToggleView(int number, bool isOn)
        {
            _toggleViews[number].SetPlayer((PlayerType)number);

            _toggleViews[number].SetOn(isOn);

            _toggleViews[number].OnToggleChanged += ChooseToggle;
        }

        private void ChooseToggle(PlayerType playerType, bool isActive)
        {
            if (!isActive)
            {
                _selectedAmount--;

                if (_selectedAmount <= 0)
                {
                    _toggleViews[(int)playerType].SetOn(true);
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