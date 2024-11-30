using GameCore;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameUI
{
    public class RewardsInfoViews : MonoBehaviour
    {
        [SerializeField]
        private RewardInfoView[] _views;

        [SerializeField]
        private PlayerTypesConfig _playerTypesConfig;

        private readonly Dictionary<PlayerType, RewardInfoView> _playersInfo = new();

        private void Awake()
        {
            var playersTypes = Enum.GetValues(typeof(PlayerType));

            if (_views.Length != playersTypes.Length)
            {
                throw new Exception("rewards info views number doesn't equals player types number");
            }

            for (int i = 0; i < _views.Length; i++)
            {
                var playerType = (PlayerType)i;

                _views[i].SetPlayerImage(
                    _playerTypesConfig.GetPlayerSprite(playerType));

                _playersInfo.Add(playerType, _views[i]);
            }
        }

        public void SetTargetsInfo(Player player, RewardsConfig rewardsConfig)
        {
            _playersInfo[player.Type].SetCurrentReward(
                rewardsConfig.GetRewardSprite(player.CurrentTarget));

            _playersInfo[player.Type].SetRemainTargets(
                Mathf.Max(0, player.RemainTargetsCount - 1));
        }
    }
}