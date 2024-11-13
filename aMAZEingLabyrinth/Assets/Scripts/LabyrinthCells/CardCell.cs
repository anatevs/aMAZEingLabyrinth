using UnityEngine;

namespace GameCore
{
    public class CardCell : MonoBehaviour
    {
        public CellGeometry Geometry => _cellGeometry;

        public RewardName Reward => _rewardName;

        [SerializeField]
        private Transform _cellTransform;

        [SerializeField]
        private CellGeometry _cellGeometry;

        [SerializeField]
        private RewardName _rewardName;
    }
}