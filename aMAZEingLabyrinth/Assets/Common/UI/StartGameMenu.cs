using UnityEngine;
using UnityEngine.UI;
using System;

namespace GameUI
{
    public class StartGameMenu : MonoBehaviour
    {
        public event Action OnNewGameClicked;
        public event Action OnLoadGameClicked;

        [SerializeField]
        private Button _newGameButton;

        [SerializeField]
        private Button _loadGameButton;

        private void Awake()
        {
            _newGameButton.onClick.AddListener(SetNewGame);
            _loadGameButton.onClick.AddListener(LoadGame);
        }

        private void OnDisable()
        {
            _newGameButton.onClick.RemoveAllListeners();
        }

        public void Show()
        {
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

        private void LoadGame()
        {
            Hide();
            OnLoadGameClicked?.Invoke();
        }
    }
}