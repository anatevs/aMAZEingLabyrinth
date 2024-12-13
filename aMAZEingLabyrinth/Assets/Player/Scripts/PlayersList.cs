﻿using SaveLoadNamespace;
using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace GameCore
{
    public sealed class PlayersList : MonoBehaviour
    {
        public event Action<HashSet<PlayerType>> OnListChanged;

        public Player CurrentPlayer => _players[_currentIndex];

        public HashSet<PlayerType> Players => _activePlayers;

        [SerializeField]
        private Player[] _players;

        [SerializeField]
        private RewardCardsService _rewardCardsService;

        private readonly HashSet<PlayerType> _activePlayers = new();

        private readonly Dictionary<PlayerType, Player> _playersDict = new();


        //make a List<Player> wich size and content will depend on selected players 
        //and substitute this list in except _players in this script

        private PlayersDataConnector _playersDataConnector;

        private int _currentIndex = 0;

        [Inject]
        public void Construct(PlayersDataConnector playersDataConnector)
        {
            _playersDataConnector = playersDataConnector;

            _playersDataConnector.OnPlayersRequested += SendPlayersToConnector;
        }

        private void Awake()
        {
            Init();
        }

        private void Init()
        {
            foreach (var player in _players)
            {
                _activePlayers.Add(player.Type);
                _playersDict.Add(player.Type, player);
            }
        }

        private void OnDisable()
        {
            foreach (var player in _players)
            {
                _rewardCardsService.UnsubscribePlayers(player);

                player.OnSetPlaying -=
                    _rewardCardsService.SetActivePlayerHighlight;
            }

            _playersDataConnector.OnPlayersRequested -= SendPlayersToConnector;
        }

        public void SetPlayerToList(PlayerType type, bool isActive)
        {
            if (isActive)
            {
                _activePlayers.Add(type);
            }
            else
            {
                _activePlayers.Remove(type);
            }

            OnListChanged?.Invoke(_activePlayers);
        }

        public void InitPlayers(PlayerType firstPlayer)
        {
            for (int i = 0; i < _players.Length; i++)
            {
                var player = InitPlayer(i);

                if (firstPlayer == player.Type)
                {
                    player.SetIsPlaying(true);

                    _currentIndex = i;
                }
            }

            _rewardCardsService.DealOutDefaultCards(_players);
        }

        public void InitPlayers()
        {
            for (int i = 0; i < _players.Length; i++)
            {
                var player = InitPlayer(i);

                if (player.IsPlaying)
                {
                    player.SetIsPlaying(true);

                    _currentIndex = i;
                }
            }

            _rewardCardsService.DealOutLoadedCards(_players);
        }

        private Player InitPlayer(int i)
        {
            var player = _players[i];

            player.Init(_playersDataConnector.Data.GetPlayerData(player.Type));

            player.OnSetPlaying +=
                _rewardCardsService.SetActivePlayerHighlight;

            return player;
        }

        public void SetNextPlayer()
        {
            CurrentPlayer.SetIsPlaying(false);

            _currentIndex = (_currentIndex + 1) % _players.Length;

            CurrentPlayer.SetIsPlaying(true);
        }

        public void ReleasePlayerReward(int plIndex)
        {
            _players[plIndex].ReleaseReward();
        }

        public bool IsPlayerAtPointWithX(int x, out List<Player> players)
        {
            players = new();
            bool result = false;

            for (int i =0; i < _players.Length; i++)
            {
                if (_players[i].Coordinate.X == x)
                {
                    players.Add(_players[i]);
                    result = true;
                }
            }

            return result;
        }

        public bool IsPlayerAtPointWithY(int y, out List<Player> players)
        {
            players = new();
            bool result = false;

            for (int i = 0; i < _players.Length; i++)
            {
                if (_players[i].Coordinate.Y == y)
                {
                    players.Add(_players[i]);
                    result = true;
                }
            }

            return result;
        }

        private void SendPlayersToConnector()
        {
            _playersDataConnector.SetPlayers(_players);
        }
    }
}