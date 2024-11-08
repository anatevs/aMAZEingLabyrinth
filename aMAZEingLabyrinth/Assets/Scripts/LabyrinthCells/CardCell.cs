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

        private readonly int[,] _values;

        private Transform _view;

        public CardCell(List<(int X, int Y)> walkablePoints, Transform view)
        {
            _values = new int[Size, Size];

            foreach (var point in walkablePoints)
            {
                _walkablePoints.Add(point);
            }

            _view = view;


            _shift = (int)(Size / 2);

            Init();
        }

        private void Init()
        {
            SetValue((0, 0), 1);

            SetWalkablePoints(1);

            Debug.Log($"{_view.name} start matrix:");
            PrintMatrix();

            int rotStep = 90;
            var initRotCount = (int)_view.eulerAngles.z / rotStep;
            Debug.Log($"init rotation is {_view.eulerAngles.z}: {initRotCount} times {rotStep}");
            for (int i = 0; i < initRotCount; i++)
            {
                RotateMatrix(rotStep);
            }

            Debug.Log($"{_view.name} init matrix:");
            PrintMatrix();
        }

        public int GetValue(int row, int col)
        {
            return _values[row, col];
        }

        public void Rotate(int angleDeg)
        {
            RotateMatrix(angleDeg);

            _view.rotation = CellRotationInfo.Quaternioins90[angleDeg] * _view.rotation;
        }

        private void RotateMatrix(int angleDeg)
        {
            SetWalkablePoints(0);

            for (int i = 0; i < _walkablePoints.Count; i++)
            {
                _walkablePoints[i] = GetRotatePoint(_walkablePoints[i],
                    CellRotationInfo.Rot90MatrixCoef[angleDeg]);
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