using UnityEngine;
using System;
using UnityEngine.UI;

namespace GameUI
{
    public class InGameMenu : MonoBehaviour
    {
        public event Action OnNewGameClicked;

        public event Action OnExitClicked;

        [SerializeField]
        private Button _newGame;

        [SerializeField]
        private Button _exit;

        private void OnEnable()
        {
            _newGame.onClick.AddListener(SetNewGame);
            _exit.onClick.AddListener(Exit);
        }

        private void OnDisable()
        {
            _newGame.onClick.RemoveAllListeners();
            _exit.onClick.RemoveAllListeners();
        }

        private void SetNewGame()
        {
            OnNewGameClicked?.Invoke();
        }

        private void Exit()
        {
            OnExitClicked?.Invoke();
        }
    }
}