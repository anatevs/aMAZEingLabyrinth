using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace GameCore
{
    public class AngleCell : MonoBehaviour
    {
        public event Action<int> OnRotated;

        private List<(int X, int Y)> _walkablePoints = new List<(int, int)>()
        {
            (1, 0),
            (0, 1)
        };

        private Dictionary<int, (int, int)> _rot90Info = new()
        {
            {90, (-1, 1) },
            {-90, (1, -1) }
        };

        private bool[,] _values = new bool[3, 3];
        
        private int _shift = 1;

        private void Init()
        {
            SetCoordinate((0, 0), true);

            SetWalkablePoints(true);
        }

        private void Rotate(int angleDeg)
        {
            SetWalkablePoints(false);

            for (int i = 0; i < _walkablePoints.Count; i++)
            {
                _walkablePoints[i] = GetRotatePoint(_walkablePoints[i], _rot90Info[angleDeg]);
            }

            SetWalkablePoints(true);

            OnRotated?.Invoke(angleDeg);
        }

        private (int X, int Y) GetRotatePoint((int X, int Y) point, (int, int) rot90)
        {
            return (point.Y * rot90.Item1, point.X * rot90.Item2);
        }

        private void SetWalkablePoints(bool value)
        {
            foreach (var point in _walkablePoints)
            {
                SetCoordinate(point, value);
            }
        }

        private void SetCoordinate((int X, int Y) point, bool value)
        {
            _values[AxisToIndex(-point.Y), AxisToIndex(point.X)] = value;
        }

        private int AxisToIndex(int coordinate)
        {
            return coordinate + _shift;
        }

        private int IndexToAxis(int index)
        {
            return index - _shift;
        }

        [SerializeField]
        private int _angleDeg = 90;

        [SerializeField]
        private bool _check = false;

        private void Start()
        {
            Init();

            PrintMatrix();
        }

        private void Update()
        {
            if (_check)
            {
                //int indxRow = -_point[1] + _shift;
                //int indxCol = _point[0] + _shift;

                //Debug.Log(_values[indxRow, indxCol]);

                Rotate(_angleDeg);

                PrintMatrix();

                _check = false;
            }
        }

        private void PrintMatrix()
        {
            Debug.Log("matrix:");

            for (int i = 0; i < _values.GetLength(1); i++)
            {
                string rowString = "";

                for (int j = 0; j < _values.GetLength(0); j++)
                {
                    rowString = rowString + _values[i, j] + ",";
                }

                Debug.Log(rowString);
            }
        }
    }
}