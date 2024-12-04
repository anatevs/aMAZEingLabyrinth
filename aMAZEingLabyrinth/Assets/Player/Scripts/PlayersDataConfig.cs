using UnityEngine;

namespace GameCore
{
    [CreateAssetMenu(
        fileName = "PlayersDataConfig",
        menuName = "Configs/PlayersData"
        )]
    public sealed class PlayersDataConfig : ScriptableObject
    {
        public PlayersData Data => _data;

        [SerializeField]
        private OnePlayerData[] _dataArray = new OnePlayerData[4];

        private readonly PlayersData _data = new();

        private void OnEnable()
        {
            _data.SetPlayersData(_dataArray);
        }

        public OnePlayerData GetData(PlayerType type)
        {
            return _data.GetPlayerData(type);
        }
    }
}