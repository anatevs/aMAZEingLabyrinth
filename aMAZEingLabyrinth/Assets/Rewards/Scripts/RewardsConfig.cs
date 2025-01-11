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
        public int RewardsCount => _rewards.Count;

        [SerializeField]
        private List<RewardSprite> _rewards =
            Enum.GetValues(typeof(RewardName))
            .Cast<RewardName>()
            .Where(c => c != RewardName.None)
            .Select(c => new RewardSprite()
            {
                Name = c,
                RewardPicture = null,
                Scale = 1,
                RotationZ = 0,
                ShiftX = 0,
                ShiftY = 0
            })
            .ToList();

        private readonly Dictionary<RewardName, Sprite> _rewardsDict = new();

        private readonly Dictionary<RewardName, (float scale, float rotZ, (float x, float y) shift)>
            _scaleRotDict = new();

        private void OnEnable()
        {
            foreach (var info in _rewards)
            {
                _rewardsDict.Add(info.Name, info.RewardPicture);

                _scaleRotDict.Add(info.Name, (info.Scale, info.RotationZ, (info.ShiftX, info.ShiftY)));
            }
        }

        public Sprite GetRewardSprite(RewardName name)
        {
            return _rewardsDict[name];
        }

        public (float scale, float rotZ, (float x, float y)) GetScaleRot(RewardName name)
        {
            return _scaleRotDict[name];
        }

        public RewardSprite GetRewardInfo(int index)
        {
            return _rewards[index];
        }
    }

    [Serializable]
    public struct RewardSprite
    {
        public RewardName Name;

        public Sprite RewardPicture;

        public float Scale;

        public float RotationZ;

        public float ShiftX;

        public float ShiftY;
    }
}