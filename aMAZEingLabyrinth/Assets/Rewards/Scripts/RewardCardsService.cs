using Cysharp.Threading.Tasks;
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

        [SerializeField]
        private float _setHighlightDuration = 0.2f;

        public void InitRewardsViews(ICollection<Player> players)
        {
            _targetsUI.InitViews(players);
        }

        public void DealOutDefaultCards(ICollection<Player> players)
        {
            var rewardIndexes = new List<int>(Enumerable
                .Range(0, _rewardsConfig.RewardsCount));

            if (rewardIndexes.Count % players.Count != 0)
            {
                throw new System.Exception($"rewards count is not a multiple of player's count");
            }

            var rewardsAmount = _rewardsConfig.RewardsCount / players.Count;


            foreach (Player player in players)
            {
                for (int i = 0; i < rewardsAmount; i++)
                {
                    var index = Random.Range(0, rewardIndexes.Count);

                    var reward = _rewardsConfig.GetRewardInfo(rewardIndexes[index]);

                    rewardIndexes.RemoveAt(index);

                    player.AddReward(reward.Name);
                }

                SetTargetUIInfo(player);
            }
        }

        public void DealOutLoadedCards(ICollection<Player> players)
        {
            foreach (var player in players)
            {
                SetTargetUIInfo(player);
            }
        }

        public void SetActivePlayerHighlight(PlayerType playerType)
        {
            _targetsUI.SetActiveHighlight(playerType);
        }

        public async UniTask SmoothSetHighlight(PlayerType playerType)
        {
            await _targetsUI.SmoothSetActiveHighlight(playerType, _setHighlightDuration);
        }

        public void SetTargetUIInfo(Player player)
        {
            _targetsUI.SetTargetsInfo(player, _rewardsConfig);
        }

        public async UniTask SetNewTarget(Player player)
        {
            SetTargetUIInfo(player);

            await _targetsUI.ScaleViewUpDown(player);
        }
    }
}