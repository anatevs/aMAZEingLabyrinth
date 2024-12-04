using GameCore;
using System;

namespace SaveLoadNamespace
{
    public class PlayersDataConnector
    {
        public event Action OnPlayersRequested;

        public PlayersData Data => _playersData;

        public Player[] Players => _players;

        private PlayersData _playersData = new();

        private Player[] _players = new Player[Enum.GetNames(typeof(PlayerType)).Length];

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

        public OnePlayerData GetData(PlayerType playerType)
        {
            return _playersData.GetPlayerData(playerType);
        }

        public void SetupPlayers()
        {
            OnPlayersRequested?.Invoke();
        }
        
        public void SetPlayers(Player[] players)
        {
            _players = players;
        }
    }
}