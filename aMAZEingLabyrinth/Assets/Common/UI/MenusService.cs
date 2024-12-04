﻿using UnityEngine;

namespace GameUI
{
    public sealed class MenusService : MonoBehaviour
    {
        public PlayerSelector PlayerSelector => _playerSelector;
        public NoPathMessageWindow NoPathMenu => _noPathMessageWindow;

        public EndGameMenu EndGame => _endGame;

        public StartGameMenu StartGame => _startGame;

        [SerializeField]
        private PlayerSelector _playerSelector;

        [SerializeField]
        private NoPathMessageWindow _noPathMessageWindow;

        [SerializeField]
        private EndGameMenu _endGame;

        [SerializeField]
        private StartGameMenu _startGame;
    }
}