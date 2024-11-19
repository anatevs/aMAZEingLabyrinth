using System;
using System.Linq;
using UnityEngine;

namespace GameCore
{
    [CreateAssetMenu(
        fileName = "MovableCellsConfig",
        menuName = "Configs/MovableCells"
        )]
    public class MovableCellsConfig : ScriptableObject
    {
        public int Count => _movableCells.Length;

        [SerializeField]
        private RewardName[] _angleRewardCells = new RewardName[6];

        [SerializeField]
        private RewardName[] _tShapeRewardCells = new RewardName[6];

        private readonly int _tCount = 6;
        private readonly int _angleRewardCount = 6;

        private readonly int _angleEmptyCount = 10;
        private readonly int _lineCount = 12;

        private CardCellsType[] _movableCells;

        private void OnValidate()
        {
            var angleShapeReward = new CardCellsType[_angleRewardCount];
            var tShape = new CardCellsType[_tCount];
            var angleShapeEmpty = new CardCellsType[_angleEmptyCount];
            var lineShape = new CardCellsType[_lineCount];

            FillTypesArray(angleShapeReward, CellGeometry.Angle, _angleRewardCells);
            FillTypesArray(angleShapeEmpty, CellGeometry.Angle);
            FillTypesArray(tShape, CellGeometry.TShape, _tShapeRewardCells);
            FillTypesArray(lineShape, CellGeometry.Line);

            _movableCells = angleShapeReward
                .Concat(angleShapeEmpty)
                .Concat(tShape)
                .Concat(lineShape)
                .ToArray();
        }

        public CardCellsType GetCardCellType(int i)
        {
            return _movableCells[i];
        }

        private void FillTypesArray(CardCellsType[] typesArray, CellGeometry geometry, RewardName[] rewards = null)
        {
            if (rewards != null && rewards.Length != typesArray.Length)
            {
                throw new Exception($"cell types array length != reward names array!");
            }

            for (int i = 0; i < typesArray.Length; i++)
            {
                typesArray[i].Geometry = geometry;
                typesArray[i].Reward = RewardName.None;

                if (rewards != null)
                {
                    typesArray[i].Reward = rewards[i];
                }
            }
        }
    }

    [Serializable]
    public struct CardCellsType
    {
        public CellGeometry Geometry;

        public RewardName Reward;
    }
}