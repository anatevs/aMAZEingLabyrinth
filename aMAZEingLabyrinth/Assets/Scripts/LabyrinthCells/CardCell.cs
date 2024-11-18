using UnityEngine;

namespace GameCore
{
    public sealed class CardCell : MonoBehaviour
    {
        public CellGeometry Geometry => _cellGeometry;

        public RewardName Reward => _rewardName;

        public CardCellValues CellValues => _cellValues;

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
            _cellValues.OnRotated -= SetRotation90;
        }

        public void Init(RewardName rewardName, int initAngleDeg)
        {
            _rewardName = rewardName;
            SetRewardSprite();

            SetAngleRotation(CellRotationInfo.CalculateQuaternion(initAngleDeg));

            InitCellValues();
        }

        public CardCellValues InitCellValues()
        {
            var cellValues = new CardCellValues(Geometry, transform.eulerAngles.z, (int)_rewardName);

            _cellValues = cellValues;

            _cellValues.OnRotated += SetRotation90;

            return _cellValues;
        }

        private void SetRotation90(int angleDeg)
        {
            SetAngleRotation(CellRotationInfo.GetQuaternion90(angleDeg));
        }

        private void SetAngleRotation(Quaternion angleQuaternion)
        {
            transform.rotation = angleQuaternion * transform.rotation;
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