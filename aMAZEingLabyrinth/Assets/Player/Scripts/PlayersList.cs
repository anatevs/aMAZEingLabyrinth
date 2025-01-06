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

        public List<PlayerType> ActiveTypes => _activeTypes;

        [SerializeField]
        private Player[] _players;

        private RewardCardsService _rewardCardsService;

        private List<Player> ActivePlayers => _activeTypes.Select(type => _playersDict[type]).ToList();

        private List<PlayerType> _activeTypes = new();

        private readonly Dictionary<PlayerType, Player> _playersDict = new();

        private PlayersDataConnector _playersDataConnector;

        private int _currentIndex = 0;

        [Inject]
        public void Construct(RewardCardsService rewardCardsService, PlayersDataConnector playersDataConnector)
        {
            _rewardCardsService = rewardCardsService;
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
            _playersDataConnector.OnPlayersRequested -= SendPlayersToConnector;
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
            SetNoPlayingAll();

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

                    _rewardCardsService.SetActivePlayerHighlight(player.Type);
                }
            }

            _rewardCardsService.DealOutDefaultCards(ActivePlayers);
        }

        public void InitPlayers()
        {
            _activeTypes = _playersDataConnector.Data.GetActiveTypes().ToList();

            SetNoPlayingAll();

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

                    _rewardCardsService.SetActivePlayerHighlight(player.Type);
                }
            }

            _rewardCardsService.DealOutLoadedCards(ActivePlayers);
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

        private Player InitPlayer(PlayerType type)
        {
            var player = _playersDict[type];

            player.Init(_playersDataConnector.Data.GetPlayerData(type));

            return player;
        }

        private void SetNoPlayingAll()
        {
            foreach (var player in _players)
            {
                player.gameObject.SetActive(false);
            }
        }

        private void SendPlayersToConnector()
        {
            _playersDataConnector.SetPlayers(ActivePlayers);
        }
    }
}