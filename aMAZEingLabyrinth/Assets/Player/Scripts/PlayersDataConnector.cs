using GameCore;
using System;
using System.Collections.Generic;

namespace SaveLoadNamespace
{
    public class PlayersDataConnector
    {
        public event Action OnPlayersRequested;

        public PlayersData Data => _playersData;

        public List<Player> Players => _players;

        private PlayersData _playersData = new();

        private List<Player> _players;

        private readonly PlayersDataConfig _dataConfig;

        public PlayersDataConnector(PlayersDataConfig playersDataConfig)
        {
            _dataConfig = playersDataConfig;
        }

        public void SetDefaultData()
        {
            _playersData = _dataConfig.Data;
        }

        public void SetPlayersData(PlayersData playersData)
        {
            _playersData = playersData;
        }

        public void SetupPlayers()
        {
            OnPlayersRequested?.Invoke();
        }
        
        public void SetPlayers(List<Player> players)
        {
            _players = players;
        }
    }
}