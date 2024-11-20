﻿using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace GameCore
{
    public sealed class CellsCollection : MonoBehaviour
    {
        [SerializeField]
        private CardCell[] _fixedCells;

        [SerializeField]
        private Transform _pathMarkersTransform;

        [SerializeField]
        private Transform _movableParentTransform;

        [SerializeField]
        private CellPrefabsConfig _cellPrefabsConfig;

        [SerializeField]
        private MovableCellsConfig _movableCellsConfig;

        [SerializeField]
        private Transform _playableCardTransform;

        private LabyrinthGrid _grid;

        private readonly int _cellSize = 3;

        private CardCell[,] _cardCells;

        private (int Rows, int Cols) _size = (7, 7);

        private readonly int[] _fixedRowCols = new int[4] { 0, 2, 4, 6 };
        private readonly int[] _movableRowCols = new int[3] { 1, 3, 5 };

        [SerializeField]
        private GameObject _pathMarker;

        [Header("Rotation test")]
        [SerializeField]
        private int _angleDeg = 90;

        [SerializeField]
        private int[] _rotateCardIndex = new int[2];

        [SerializeField]
        private bool _check = false;


        [Header("Find path test")]
        [SerializeField]
        private int[] _startRowCol = new int[2];

        [SerializeField]
        private int[] _endRowCol = new int[2];


        [SerializeField]
        private bool _findPath;

        [Header("Print cell test")]
        [SerializeField]
        private int[] _printRowCol = new int[2];

        [SerializeField]
        private bool _printCell;

        private void Start()
        {
            _grid = new LabyrinthGrid((_size.Rows * _cellSize, _size.Cols * _cellSize));

            _cardCells = new CardCell[_size.Rows, _size.Cols];

            foreach (var cell in _fixedCells)
            {
                var (iCell, jCell) = GetCardIndex(((int)cell.transform.localPosition.x,
                    (int)cell.transform.localPosition.y));

                cell.InitCellValues();

                SetValuesToLabyrinth(cell, iCell, jCell);
            }

            InitMovableCellsNewGame();
        }

        private void InitMovableCellsNewGame()
        {
            var movableAmount = _size.Rows * _size.Cols + 1 - _fixedCells.Length;

            if (_movableCellsConfig.Count != movableAmount)
            {
                throw new System.Exception("Movable cells count in config and in collection must be equal!");
            }

            (int Row, int Col)[] movableIndexes =
                new (int Row, int Col)[movableAmount - 1];

            int count = 0;
            foreach (var i in _movableRowCols)
            {
                for (int j = 0; j < _size.Cols; j++)
                {
                    movableIndexes[count] = (i, j);
                    count++;
                }
            }

            foreach (var i in _fixedRowCols)
            {
                foreach (var j in _movableRowCols)
                {
                    movableIndexes[count] = (i, j);
                    count++;
                }
            }

            var indexList = new List<int>(Enumerable
                .Range(0, movableAmount));

            int[] rotations = new int[4] { 0, 90, 180, 270 };

            var spawner = new CellSpawner(_cellPrefabsConfig);

            foreach (var (Row, Col) in movableIndexes)
            {
                var rotation = rotations[Random.Range(0, rotations.Length)];

                var randomIndex = indexList[Random.Range(0, indexList.Count)];

                var cellType = _movableCellsConfig.GetCardCellType(randomIndex);

                indexList.Remove(randomIndex);


                var (X, Y) = GetXY((Row, Col), (2, 0));

                var cell = spawner.SpawnCell(cellType.Geometry, cellType.Reward, rotation, X, Y, _movableParentTransform);

                SetValuesToLabyrinth(cell, Row, Col);
            }

            var plCellType = _movableCellsConfig.GetCardCellType(indexList[0]);

            spawner.SpawnCell(plCellType.Geometry, plCellType.Reward, 0, 0, 0, _playableCardTransform);
        }


        private void Update()
        {
            if (_check)
            {
                var rotatedCell = _cardCells[_rotateCardIndex[0], _rotateCardIndex[1]].CellValues;

                rotatedCell.Rotate(_angleDeg);

                SetCardToGridValues(_rotateCardIndex[0], _rotateCardIndex[1]);

                _check = false;
            }


            if (_findPath)
            {
                for (int i = 0; i < _pathMarkersTransform.childCount; i++)
                {
                    Destroy(_pathMarkersTransform.GetChild(i).gameObject);
                }

                var (xStart, yStart) = GetXY((_startRowCol[0], _startRowCol[1]), (1, 1));
                var (xEnd, yEnd) = GetXY((_endRowCol[0], _endRowCol[1]), (1, 1));

                var start = new Vector2Int(xStart, yStart);
                var end = new Vector2Int(xEnd, yEnd);

                var res = _grid.TryFindAStarPath(start, end, out List<Vector2Int> result);

                var xLocal = (int)transform.position.x;
                var yLocal = (int)transform.position.y;

                var markerPos = new Vector3(xStart + xLocal, yStart + yLocal, _pathMarker.transform.position.z);

                Instantiate(_pathMarker, markerPos, Quaternion.identity, transform);

                markerPos.x = xEnd + xLocal;
                markerPos.y = yEnd + yLocal;
                Instantiate(_pathMarker, markerPos, Quaternion.identity, _pathMarkersTransform);

                if (res)
                {
                    Debug.Log($"path found: {res}");

                    foreach (var pathPoint in result)
                    {
                        markerPos.x = pathPoint.x + xLocal;
                        markerPos.y = pathPoint.y + yLocal;

                        Instantiate(_pathMarker, markerPos, Quaternion.identity, _pathMarkersTransform);
                    }
                }
                else
                {
                    Debug.Log($"path found: {res}");
                }

                _findPath = false;
            }

            if (_printCell)
            {
                var cell = _cardCells[_printRowCol[0], _printRowCol[1]].CellValues;
                if (cell == null)
                {
                    Debug.Log("null cell");
                }
                else
                {
                    cell.PrintMatrix();
                }

                _printCell = false;
            }
        }

        private void ShiftRowOrCol((int row, int col) index, CardCell newCard)
        {
            CardCell outCell = null;

            bool isRow = _movableRowCols.Contains(index.row);
            bool isCol = _movableRowCols.Contains(index.col);

            if (isRow == isCol)
            {
                Debug.Log($"trying to move a fixed row or column with edge element index {index}");
                return;
            }



            int direction = 1;

            if (isRow)
            {
                if (index.col == _size.Cols)
                {
                    direction = -1;

                    //outCell = _cardCells[index.row, 0];
                }

                var col = index.col;
                for (int i = 0; i < _size.Cols - 1; i++)
                {

                }
            }
        }

        private void SetValuesToLabyrinth(CardCell cell, int i, int j)
        {
            _cardCells[i, j] = cell;

            SetCardToGridValues(i, j);
        }

        private void SetCardToGridValues(int i, int j)
        {
            var card = _cardCells[i, j].CellValues;

            for (int ic = 0; ic < card.Size; ic++)
            {
                for (int jc = 0; jc < card.Size; jc++)
                {
                    var xy = GetXY((i, j), (ic, jc));

                    _grid.SetValue(card.GetValue(ic, jc), xy);
                }
            }
        }

        private (int X, int Y) GetXY((int i, int j) cellIndex, (int ic, int jc) elementIndex)
        {
            var x = cellIndex.j * _cellSize + elementIndex.jc;
            var y = _size.Rows * _cellSize - 1 - (cellIndex.i * _cellSize + elementIndex.ic);
            return (x, y);
        }

        private (int iCell, int jCell) GetCardIndex((int x, int y) coordinates)
        {
            int i = _size.Rows - 1 - coordinates.y / _cellSize;
            int j = coordinates.x / _cellSize;

            return (i, j);
        }

        private (int iElement, int jElement) GetCardElementIndex((int x, int y) coordinates)
        {
            int ic = _cellSize - 1 - coordinates.y % _cellSize;
            int jc = coordinates.x % _cellSize;

            return (ic, jc);
        }
    }
}