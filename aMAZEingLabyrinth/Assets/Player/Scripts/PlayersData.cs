using System.Collections.Generic;

namespace GameCore
{
    public sealed class PlayersData
    {
        public Dictionary<PlayerType, OnePlayerData> Data => _playersData;

        private readonly Dictionary<PlayerType, OnePlayerData> _playersData = new();

        public void SetPlayersData(OnePlayerData[] dataArray)
        {
            foreach (var data in dataArray)
            {
                _playersData.Add(data.Type, data);
            }
        }

        public OnePlayerData GetPlayerData(PlayerType playerType)
        {
            return _playersData[playerType];
        }
    }
}