using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    [CreateAssetMenu(fileName = "PlayerTypesConfig",
        menuName = "Configs/PlayerTypesConfig")]
    public class PlayersSpritesConfig : ScriptableObject
    {
        [SerializeField]
        private PlayerSprite[] _playerTypesConfigs;

        private readonly Dictionary<PlayerType, Sprite> _sprites = new();

        private void OnEnable()
        {
            foreach (var playerSprite in _playerTypesConfigs)
            {
                _sprites.Add(playerSprite.PlayerType, playerSprite.Sprite);
            }
        }

        public Sprite GetPlayerSprite(PlayerType playerType)
        {
            return _sprites[playerType];
        }
    }

    [Serializable]
    public struct PlayerSprite
    {
        public PlayerType PlayerType;
        public Sprite Sprite;
    }
}