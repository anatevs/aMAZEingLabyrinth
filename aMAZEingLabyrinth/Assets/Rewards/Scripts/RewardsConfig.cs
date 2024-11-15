using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameCore
{
    [CreateAssetMenu(
        fileName = "RewardsConfig",
        menuName = "Configs/RewardsConfig"
        )]

    public class RewardsConfig : ScriptableObject
    {
        [SerializeField]
        private List<RewardSprite> _rewards =
            Enum.GetValues(typeof(RewardName))
            .Cast<RewardName>()
            .Where(c => c != RewardName.None)
            .Select(c => new RewardSprite() { Name = c, RewardPicture = null })
            .ToList();

        private readonly Dictionary<RewardName, Sprite> _rewardsDict = new();

        private void OnEnable()
        {
            foreach (var info in _rewards)
            {
                _rewardsDict.Add(info.Name, info.RewardPicture);
            }
        }

        public Sprite GetRewardSprite(RewardName name)
        {
            return _rewardsDict[name];
        }
    }

    [Serializable]
    public struct RewardSprite
    {
        public RewardName Name;

        public Sprite RewardPicture;
    }
}