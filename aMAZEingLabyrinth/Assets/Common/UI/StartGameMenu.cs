﻿using UnityEngine;
using UnityEngine.UI;
using System;

namespace GameUI
{
    public sealed class StartGameMenu : MonoBehaviour
    {
        public event Action OnNewGameClicked;
        public event Action OnLoadGameClicked;

        [SerializeField]
        private Button _newGameButton;

        [SerializeField]
        private Button _loadGameButton;

        private void OnEnable()
        {
            _newGameButton.onClick.AddListener(SetNewGame);
            _loadGameButton.onClick.AddListener(LoadGame);
        }

        private void OnDisable()
        {
            _newGameButton.onClick.RemoveAllListeners();
        }

        public void SetLoadButtonActive(bool isActive)
        {
            _loadGameButton.interactable = isActive;
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