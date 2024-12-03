using System;

namespace GameCore
{
    [Serializable]
    public struct OneCellData
    {
        public CellGeometry Geometry;

        public RewardName Reward;

        public int RotationDeg;

        //public (int X, int Y) Origin;

        public (int Row, int Col) Index;

        public OneCellData(CellGeometry geometry,
            RewardName reward, int rotationDeg,
            //(int X, int Y) origin,
            (int Row, int Col) index)
        {
            Geometry = geometry;
            Reward = reward;
            RotationDeg = rotationDeg;
            //Origin = origin;
            Index = index;
        }
    }
}