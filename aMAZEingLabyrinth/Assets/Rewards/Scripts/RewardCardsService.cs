using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameCore
{
    public class RewardCardsService : MonoBehaviour
    {
        [SerializeField]
        private GameObject _prefab;

        [SerializeField]
        private RewardsConfig _rewardConfig;

        public void DealOutCards(PlayersList players)
        {
            var indexes = new List<int>(Enumerable
                .Range(0, _rewardConfig.RewardsCount));

            if (indexes.Count % players.PlayersCount != 0)
            {
                throw new System.Exception($"rewards count is not a multiple of player's count");
            }

            var rewardsAmount = _rewardConfig.RewardsCount / players.PlayersCount;

            for (int i_player = 0; i_player < players.PlayersCount; i_player++)
            {
                for (int i = 0; i < rewardsAmount; i++)
                {
                    var index = Random.Range(0, indexes.Count);

                    var reward = _rewardConfig.GetRewardInfo(indexes[index]);

                    indexes.RemoveAt(index);

                    players.AddPlayerReward(i_player, reward.Name);

                    Debug.Log($"{reward.Name} added to {i_player}");
                }
            }
        }
    }
}