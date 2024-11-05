using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    public static class CellRotationInfo
    {
        private static readonly int[] _angles = { 90, -90 };


        public static readonly Dictionary<int, (int, int)> Rot90MatrixCoef = new()
        {
            { _angles[0], (-1, 1) },
            { _angles[1], (1, -1) }
        };

        public static readonly Dictionary<int, Quaternion> Quaternioins90 = new()
        {
            {
                _angles[0], Quaternion.AngleAxis(_angles[0], Vector3.forward)
            },
            {
                _angles[1], Quaternion.AngleAxis(_angles[1], Vector3.forward)
            }
        };
    }
}