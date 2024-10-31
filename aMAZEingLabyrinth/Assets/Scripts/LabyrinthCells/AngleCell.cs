using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    public class AngleCell : MonoBehaviour
    {
        private bool[,] _values = new bool[3, 3]
        {
            { false, true, false },
            { false, true, true },
            { false, false, false }
        };

        private int _shift = 1;

        [SerializeField]
        private int[] _point = new int[2];

        [SerializeField]
        private bool _check = false;

        private void Update()
        {
            if (_check)
            {
                int indxRow = -_point[1] + _shift;
                int indxCol = _point[0] + _shift;

                Debug.Log(_values[indxRow, indxCol]);

                _check = false;
            }
        }
    }
}