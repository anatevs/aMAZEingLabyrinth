using System;

namespace GameCore
{
    [Serializable]
    public struct CellData
    {
        public CellGeometry Geometry;

        public RewardName Reward;

        public int RotationDeg;

        public int X;

        public int Y;
    }
}