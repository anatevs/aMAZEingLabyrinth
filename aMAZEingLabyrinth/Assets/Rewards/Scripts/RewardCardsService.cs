using GameUI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameCore
{
    public class RewardCardsService : MonoBehaviour
    {
        [SerializeField]
        private RewardsConfig _rewardsConfig;

        [SerializeField]
        private RewardsInfoViews _targetsUI;

        public void DealOutCards(Player[] players)
        {
            var indexes = new List<int>(Enumerable
                .Range(0, _rewardsConfig.RewardsCount));

            if (indexes.Count % players.Length != 0)
            {
                throw new System.Exception($"rewards count is not a multiple of player's count");
            }

            var rewardsAmount = _rewardsConfig.RewardsCount / players.Length;

            for (int i_player = 0; i_player < players.Length; i_player++)
            {
                for (int i = 0; i < rewardsAmount; i++)
                {
                    var index = Random.Range(0, indexes.Count);

                    var reward = _rewardsConfig.GetRewardInfo(indexes[index]);

                    indexes.RemoveAt(index);

                    players[i_player].AddReward(reward.Name);
                }

                SetTargetUIInfo(players[i_player]);

                players[i_player].OnTargetChanged += SetTargetUIInfo;
            }
        }

        public void SetActivePlayerHighlight(PlayerType playerType)
        {
            _targetsUI.SetActiveHighlight(playerType);
        }

        public void UnsubscribePlayers(Player player)
        {
            player.OnTargetChanged -= SetTargetUIInfo;
        }

        private void SetTargetUIInfo(Player player)
        {
            _targetsUI.SetTargetsInfo(player, _rewardsConfig);
        }
    }
}