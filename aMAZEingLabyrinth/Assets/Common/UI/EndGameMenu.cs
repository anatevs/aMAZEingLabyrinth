using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameUI
{
    public sealed class EndGameMenu : MonoBehaviour
    {
        public event Action OnNewGameClicked;

        public event Action OnExitClicked;

        public event Action OnGameEnded;

        [SerializeField]
        private TMP_Text _winner;

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

        public void SetWinner(string winner)
        {
            _winner.text = winner;
        }

        public void Show()
        {
            OnGameEnded?.Invoke();

            gameObject.SetActive(true);
        }

        private void Hide()
        {
            gameObject.SetActive(false);
        }

        private void SetNewGame()
        {
            Hide();
            OnNewGameClicked?.Invoke();
        }

        private void Exit()
        {
            Hide();
            OnExitClicked?.Invoke();
        }
    }
}