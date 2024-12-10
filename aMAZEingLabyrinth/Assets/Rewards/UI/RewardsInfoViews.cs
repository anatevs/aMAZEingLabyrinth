using GameCore;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameUI
{
    public class RewardsInfoViews : MonoBehaviour
    {
        [SerializeField]
        private RewardInfoView[] _views;

        [SerializeField]
        private PlayersSpritesConfig _playerTypesConfig;

        [SerializeField]
        private Image _activePlayerHighlight;

        private readonly Dictionary<PlayerType, RewardInfoView> _playersInfo = new();

        private readonly Dictionary<PlayerType, Vector3> _highlightPositions = new();

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

                var highlightPos = new Vector3(_activePlayerHighlight.transform.localPosition.x,
                    _views[i].transform.localPosition.y,
                    _activePlayerHighlight.transform.localPosition.z);

                _highlightPositions.Add(playerType, highlightPos);
            }
        }

        public void SetTargetsInfo(Player player, RewardsConfig rewardsConfig)
        {
            try
            {
                _playersInfo[player.Type].SetCurrentRewardSprite(
                    rewardsConfig.GetRewardSprite(player.CurrentTarget));
            }
            catch
            {
                _playersInfo[player.Type].SetNoReward();
            }

            _playersInfo[player.Type].SetRemainTargets(
                Mathf.Max(0, player.RemainTargetsCount - 1));
        }

        public void SetActiveHighlight(PlayerType playerType)
        {
            _activePlayerHighlight.transform.localPosition =
                _highlightPositions[playerType];
        }
    }
}