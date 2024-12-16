using SaveLoadNamespace;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VContainer;

namespace GameCore
{
    public sealed class PlayersList : MonoBehaviour
    {
        public event Action<ICollection<PlayerType>> OnListChanged;

        public Player CurrentPlayer => _playersDict[_activeTypes[_currentIndex]];

        [SerializeField]
        private Player[] _players;

        [SerializeField]
        private RewardCardsService _rewardCardsService;

        private List<Player> ActivePlayers => _activeTypes.Select(type => _playersDict[type]).ToList();

        private List<PlayerType> _activeTypes = new();

        private readonly Dictionary<PlayerType, Player> _playersDict = new();

        private PlayersDataConnector _playersDataConnector;

        private int _currentIndex = 0;

        [Inject]
        public void Construct(PlayersDataConnector playersDataConnector)
        {
            _playersDataConnector = playersDataConnector;
        }

        private void Awake()
        {
            foreach (var player in _players)
            {
                _activeTypes.Add(player.Type);
                _playersDict.Add(player.Type, player);
            }
        }

        private void OnEnable()
        {
            _playersDataConnector.OnPlayersRequested += SendPlayersToConnector;
        }

        private void OnDisable()
        {
            DisactivateAll();

            _playersDataConnector.OnPlayersRequested -= SendPlayersToConnector;
        }

        private void DisactivateAll()
        {
            foreach (var player in _players)
            {
                _rewardCardsService.UnsubscribePlayers(player);

                player.OnSetPlaying -=
                    _rewardCardsService.SetActivePlayerHighlight;

                player.gameObject.SetActive(false);
            }
        }

        public void SetPlayerToList(PlayerType type, bool isActive)
        {
            if (isActive)
            {
                _activeTypes.Add(type);
            }
            else
            {
                _activeTypes.Remove(type);
            }

            OnListChanged?.Invoke(_activeTypes);
        }

        public void InitPlayers(int firstPlayerIndex)
        {
            DisactivateAll();

            _rewardCardsService.InitRewardsViews(ActivePlayers);

            for (int i = 0; i < _activeTypes.Count; i++)
            {
                var type = _activeTypes[i];
                var player = InitPlayer(type);

                player.gameObject.SetActive(true);

                if (firstPlayerIndex == i)
                {
                    player.SetIsPlaying(true);
                    _currentIndex = i;
                }

                _rewardCardsService.SubscribePlayer(player);
            }

            _rewardCardsService.DealOutDefaultCards(ActivePlayers);
        }

        public void InitPlayers()
        {
            _activeTypes = _playersDataConnector.Data.GetActiveTypes().ToList();

            DisactivateAll();

            _rewardCardsService.InitRewardsViews(ActivePlayers);

            for (int i = 0; i < _activeTypes.Count; i++)
            {
                var type = _activeTypes[i];
                var player = InitPlayer(type);

                player.gameObject.SetActive(true);

                if (player.IsPlaying)
                {
                    player.SetIsPlaying(true);
                    _currentIndex = i;
                }

                _rewardCardsService.SubscribePlayer(player);
            }

            _rewardCardsService.DealOutLoadedCards(ActivePlayers);
        }

        private Player InitPlayer(PlayerType type)
        {
            var player = _playersDict[type];

            player.Init(_playersDataConnector.Data.GetPlayerData(type));

            player.OnSetPlaying +=
                _rewardCardsService.SetActivePlayerHighlight;

            return player;
        }

        public void SetNextPlayer()
        {
            CurrentPlayer.SetIsPlaying(false);

            _currentIndex = (_currentIndex + 1) % _activeTypes.Count;

            CurrentPlayer.SetIsPlaying(true);
        }

        public bool IsPlayerAtPointWithX(int x, out List<Player> players)
        {
            players = new();
            bool result = false;

            foreach (var type in _activeTypes)
            {
                if (_playersDict[type].Coordinate.X == x)
                {
                    players.Add(_playersDict[type]);
                    result = true;
                }
            }

            return result;
        }

        public bool IsPlayerAtPointWithY(int y, out List<Player> players)
        {
            players = new();
            bool result = false;

            foreach (var type in _activeTypes)
            {
                if (_playersDict[type].Coordinate.Y == y)
                {
                    players.Add(_playersDict[type]);
                    result = true;
                }
            }

            return result;
        }

        private void SendPlayersToConnector()
        {
            _playersDataConnector.SetPlayers(ActivePlayers);
        }
    }
}