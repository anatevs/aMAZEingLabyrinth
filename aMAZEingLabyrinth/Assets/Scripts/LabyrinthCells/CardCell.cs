using UnityEngine;

namespace GameCore
{
    public sealed class CardCell : MonoBehaviour
    {
        public CellGeometry Geometry => _cellGeometry;

        public RewardName Reward => _rewardName;

        [SerializeField]
        private Transform _cellTransform;

        [SerializeField]
        private CellGeometry _cellGeometry;

        [SerializeField]
        private RewardName _rewardName;

        [SerializeField]
        private SpriteRenderer _rewardImage;

        [SerializeField]
        private RewardsConfig _rewardsConfig;

        private CardCellValues _cellValues;

        private void Start()
        {
            SetRewardSprite();
        }

        private void OnDisable()
        {
            _cellValues.OnRotated -= SetRotation;
        }

        public void LinkWithValues(CardCellValues cellValues)
        {
            _cellValues = cellValues;

            _cellValues.OnRotated += SetRotation;
        }

        public void SetRotation(int angleDeg)
        {
            transform.rotation = CellRotationInfo.GetQuaternion90(angleDeg) * transform.rotation;
        }

        private void SetRewardSprite()
        {
            if (_rewardName != RewardName.None)
            {
                var sprite = _rewardsConfig.GetRewardSprite(_rewardName);

                _rewardImage.sprite = sprite;
            }
        }
    }
}