using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    [CreateAssetMenu(
        fileName = "PlayersDataConfig",
        menuName = "Configs/PlayersData"
        )]
    public sealed class PlayersDataConfig : ScriptableObject
    {
        [SerializeField]
        private PlayerData[] _playersData = new PlayerData[4];

        private readonly Dictionary<PlayerType, PlayerData> _dataDict = new();

        private void OnEnable()
        {
            for (int i = 0; i < _playersData.Length; i++)
            {
                _dataDict.Add(_playersData[i].Type, _playersData[i]);
            }
        }

        public PlayerData GetData(PlayerType type)
        {
            return _dataDict[type];
        }
    }
}