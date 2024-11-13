using System;

namespace GameCore
{
    [Serializable]
    public struct CellData
    {
        public int RotationDeg;

        public CellGeometry Geometry;

        public int Reward;
    }
}