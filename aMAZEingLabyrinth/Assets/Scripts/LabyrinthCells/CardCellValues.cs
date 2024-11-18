using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    public class CardCellValues
    {
        public event Action<int> OnRotated;

        public int Size => 3;

        private readonly List<(int X, int Y)> _walkablePoints = new();

        private readonly int _shift;

        private readonly int[,] _values;

        private readonly int _rotStep = 90;

        private readonly int _initRotation;

        public CardCellValues(CellGeometry geometryType, float eulerAngleZ, int centerValue = 1)
        {
            var walkablePoints = CellGeometryInfo.GetGeometry(geometryType);

            _values = new int[Size, Size];

            foreach (var point in walkablePoints)
            {
                _walkablePoints.Add(point);
            }

            _initRotation = (int)eulerAngleZ;


            _shift = (int)(Size / 2);

            Init(centerValue);
        }

        private void Init(int centerValue)
        {
            SetValue((0, 0), centerValue);

            SetWalkablePoints(1);

            //int initRotCount = _initRotation / _rotStep;

            //RotateMatrix(_rotStep, initRotCount);

            RotateMatrix(_initRotation);
        }

        public int GetValue(int row, int col)
        {
            return _values[row, col];
        }

        public void Rotate(int angleDeg)
        {
            RotateMatrix(angleDeg);

            OnRotated?.Invoke(angleDeg);
        }

        private void RotateMatrix(int angleDeg)
        {
            if (angleDeg % _rotStep != 0)
            {
                throw new Exception($"angle {angleDeg} is not multiple of 90");
            }

            int rotCount = angleDeg / _rotStep;

            RotateMatrix(_rotStep, rotCount);
        }

        private void RotateMatrix(int angleDeg, int rotCount)
        {
            SetWalkablePoints(0);

            for (int c = 0; c < rotCount; c++)
            {
                for (int i = 0; i < _walkablePoints.Count; i++)
                {
                    _walkablePoints[i] = GetRotatePoint(_walkablePoints[i],
                        CellRotationInfo.GetRot90Coef(angleDeg));
                }
            }
            SetWalkablePoints(1);
        }

        private (int X, int Y) GetRotatePoint((int X, int Y) point, (int, int) rot90)
        {
            return (point.Y * rot90.Item1, point.X * rot90.Item2);
        }

        private void SetWalkablePoints(int value)
        {
            foreach (var point in _walkablePoints)
            {
                SetValue(point, value);
            }
        }

        private void SetValue((int X, int Y) point, int value)
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

        public void PrintMatrix()
        {
            Debug.Log("matrix:");

            for (int i = 0; i < _values.GetLength(1); i++)
            {
                string rowString = "";

                for (int j = 0; j < _values.GetLength(0); j++)
                {
                    rowString = rowString + _values[i, j] + "  ";
                }

                Debug.Log(rowString);
            }
        }
    }
}