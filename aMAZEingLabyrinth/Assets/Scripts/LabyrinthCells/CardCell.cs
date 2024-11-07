using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    public class CardCell
    {
        public int Size => 3;

        private readonly List<(int X, int Y)> _walkablePoints = new();

        private readonly int _shift;

        private readonly bool[,] _values;

        private Transform _view;

        public CardCell(List<(int X, int Y)> walkablePoints, Transform view)
        {
            _values = new bool[Size, Size];

            foreach (var point in walkablePoints)
            {
                _walkablePoints.Add(point);
            }

            _view = view;


            _shift = (int)(_values.GetLength(0) / 2);

            Init();
        }

        private void Init()
        {
            SetCoordinate((0, 0), true);

            SetWalkablePoints(true);
        }

        public bool GetValue(int row, int col)
        {
            return _values[row, col];
        }

        public void Rotate(int angleDeg)
        {
            SetWalkablePoints(false);

            for (int i = 0; i < _walkablePoints.Count; i++)
            {
                _walkablePoints[i] = GetRotatePoint(_walkablePoints[i],
                    CellRotationInfo.Rot90MatrixCoef[angleDeg]);
            }

            SetWalkablePoints(true);

            _view.rotation = CellRotationInfo.Quaternioins90[angleDeg] * _view.rotation;
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
            _values[AxisToIndex(point.Y, -1), AxisToIndex(point.X, 1)] = value;
        }

        private int AxisToIndex(int coordinate, int sign)
        {
            return sign * coordinate + _shift;
        }

        private int IndexToAxis(int index, int sign)
        {
            return (index - _shift) * sign;
        }

        //[SerializeField]
        //private int _angleDeg = 90;

        //[SerializeField]
        //private bool _check = false;

        //private void Start()
        //{
        //    Init();

        //    PrintMatrix();
        //}

        //private void Update()
        //{
        //    if (_check)
        //    {
        //        Rotate(_angleDeg);

        //        PrintMatrix();

        //        _check = false;
        //    }
        //}

        public void PrintMatrix()
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