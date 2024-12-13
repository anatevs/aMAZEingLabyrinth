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
        }

        private void OnEnable()
        {
            for (int i = 0; i < _toggleViews.Length; i++)
            {
                _toggleViews[i].OnToggleChanged += ChooseToggle;
            }
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
            OnPlayerToggled?.Invoke(playerType, isActive);
        }
    }
}