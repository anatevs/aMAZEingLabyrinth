using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using static UnityEditor.PlayerSettings;

namespace GameCore
{
    public sealed class CellsCollection : MonoBehaviour
    {
        [SerializeField]
        private CardCell[] _fixedCells;

        [SerializeField]
        private PathMarkersPool _pathMarkersPool;

        [SerializeField]
        private Transform _movableParentTransform;

        [SerializeField]
        private CellPrefabsConfig _cellPrefabsConfig;

        [SerializeField]
        private MovableCellsConfig _movableCellsConfig;

        [SerializeField]
        private PlayableCell _playableCell;

        [SerializeField]
        private ShiftArrowsService _arrowsService;

        private LabyrinthGrid _grid;

        private readonly int _cellSize = 3;

        private CardCell[,] _cardCells;

        private (int Rows, int Cols) _size = (7, 7);

        private readonly int[] _fixedRowCols = new int[4] { 0, 2, 4, 6 };
        private readonly int[] _movableRowCols = new int[3] { 1, 3, 5 };


        //[Header("Rotation test")]
        //[SerializeField]
        //private int _angleDeg = 90;

        //[SerializeField]
        //private int[] _rotateCardIndex = new int[2];

        //[SerializeField]
        //private bool _checkRotation = false;


        [Header("Find path test")]
        [SerializeField]
        private int[] _startRowCol = new int[2];

        //[SerializeField]
        //private int[] _endRowCol = new int[2];

        //[SerializeField]
        //private bool _findPath;

        [Header("Print cell test")]
        [SerializeField]
        private int[] _printRowCol = new int[2];

        [SerializeField]
        private bool _printCell;

        //[Header("Shift by playable cell test")]
        //[SerializeField]
        //private int[] _shiftRowCol = new int[2];

        //[SerializeField]
        //private bool _shiftCell;

        private void Awake()
        {
            _arrowsService.OnClick += MakeShift;
        }

        private void OnDisable()
        {
            _arrowsService.OnClick -= MakeShift;
        }

        private void Start()
        {
            _grid = new LabyrinthGrid((_size.Rows * _cellSize, _size.Cols * _cellSize));

            _cardCells = new CardCell[_size.Rows, _size.Cols];

            foreach (var cell in _fixedCells)
            {
                var (iCell, jCell) = GetCardIndex(((int)cell.transform.localPosition.x,
                    (int)cell.transform.localPosition.y));

                cell.InitCellValues();

                SetCellsToLabyrinth(cell, iCell, jCell);
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
                var rotation = rotations[UnityEngine.Random.Range(0, rotations.Length)];

                var randomIndex = indexList[UnityEngine.Random.Range(0, indexList.Count)];

                var cellType = _movableCellsConfig.GetCardCellType(randomIndex);

                indexList.Remove(randomIndex);


                var (X, Y) = GetXYOrigin(Row, Col);

                var cell = spawner.SpawnCell(cellType.Geometry, cellType.Reward, rotation, X, Y, _movableParentTransform);

                SetCellsToLabyrinth(cell, Row, Col);
            }

            var plCellType = _movableCellsConfig.GetCardCellType(indexList[0]);

            var playableCellCard = spawner.SpawnCell(plCellType.Geometry, plCellType.Reward, 0, 0, 0, _movableParentTransform);
            _playableCell.ReplacePlayableCell(playableCellCard, out _);
        }


        private void Update()
        {
            //if (_checkRotation)
            //{
            //    var rotatedCell = _cardCells[_rotateCardIndex[0], _rotateCardIndex[1]].CellValues;

            //    rotatedCell.Rotate(_angleDeg);

            //    SetCardToGridValues(_rotateCardIndex[0], _rotateCardIndex[1]);

            //    _checkRotation = false;
            //}


            //if (_findPath)
            //{
            //    FindPath(_startRowCol, _endRowCol, out _);

            //    _findPath = false;
            //}

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

            //if (_shiftCell)
            //{
            //    MakeShift(_shiftRowCol[0], _shiftRowCol[1]);

            //    _shiftCell = false;
            //}
        }


        public Vector3 GetCellCoordinates(Vector3 pos)
        {
            (int i, int j) = GetCellIndexFromGlobalXY(pos);

            var cell = _cardCells[i, j];

            return cell.transform.position;
        }

        private (int i, int j) GetCellIndexFromGlobalXY(Vector3 pos)
        {
            return GetCardIndex(((int)(pos.x - transform.position.x),
                (int)(pos.y - transform.position.y)));
        }

        private void MakeShift(int shiftRow, int shiftCol)
        {
            bool isRow = _movableRowCols.Contains(shiftRow);
            bool isCol = _movableRowCols.Contains(shiftCol);

            if (isRow == isCol)
            {
                Debug.Log($"trying to move a fixed row or column with edge element index ({shiftRow}, {shiftCol})");
                return;
            }

            int[] iterDirectionRowCol = { 0, -1 };
            int iterNumber = 1;
            int row = 0;
            int col = 0;

            if (isRow)
            {
                iterNumber = _size.Cols - 1;
                var startIter = iterNumber;

                iterDirectionRowCol[0] = 0;
                iterDirectionRowCol[1] = -1;

                if (shiftCol == iterNumber)
                {
                    iterDirectionRowCol[0] = 0;
                    iterDirectionRowCol[1] = 1;

                    startIter = 0;
                }

                col = startIter;
                row = shiftRow ;
            }

            if (isCol)
            {
                iterNumber = _size.Rows - 1;
                var startIter = iterNumber;

                iterDirectionRowCol[0] = -1;
                iterDirectionRowCol[1] = 0;

                if (shiftRow == iterNumber)
                {
                    iterDirectionRowCol[0] = 1;
                    iterDirectionRowCol[1] = 0;

                    startIter = 0;
                }

                row = startIter;
                col = shiftCol;
            }

            _arrowsService.ChangeDisabledArrow((row, col));

            _playableCell.ReplacePlayableCell(_cardCells[row, col], out var oldPlayable);

            for (int i = 0; i < iterNumber; i++)
            {
                row += iterDirectionRowCol[0];
                col += iterDirectionRowCol[1];

                var cell = _cardCells[row, col];

                SetCellsToLabyrinth(cell, row - iterDirectionRowCol[0],
                    col - iterDirectionRowCol[1], setTransformPos: true);
            }

            oldPlayable.transform.SetParent(_movableParentTransform);
            SetCellsToLabyrinth(oldPlayable, shiftRow, shiftCol, setTransformPos: true);
        }

        public bool FindPath(Vector3 cellGlobalXY, out List<(int x, int y)> globalXY)
        {
            globalXY = new();

            (int i, int j) = GetCellIndexFromGlobalXY(cellGlobalXY);

            return FindPath(_startRowCol, new int[2] {i, j}, out globalXY);
        }

        private bool FindPath(int[] startRowCol, int[] endRowCol, out List<(int x, int y)> globalXY)
        {
            globalXY = new List<(int x, int y)>();

            _pathMarkersPool.UnspawnAll();

            var (xStart, yStart) = GetXY((startRowCol[0], startRowCol[1]), (1, 1));
            var (xEnd, yEnd) = GetXY((endRowCol[0], endRowCol[1]), (1, 1));

            var start = new Vector2Int(xStart, yStart);
            var end = new Vector2Int(xEnd, yEnd);

            var res = _grid.TryFindAStarPath(start, end, out List<Vector2Int> result);

            var xLocal = (int)transform.position.x;
            var yLocal = (int)transform.position.y;

            _pathMarkersPool.Spawn(xStart + xLocal, yStart + yLocal);
            _pathMarkersPool.Spawn(xEnd + xLocal, yEnd + yLocal);

            if (res)
            {
                Debug.Log($"path found: {res}");

                foreach (var pathPoint in result)
                {
                    var (glX, glY) = (pathPoint.x + xLocal, pathPoint.y + yLocal);

                    globalXY.Add((glX, glY));

                    _pathMarkersPool.Spawn(glX, glY);
                }
            }
            else
            {
                Debug.Log($"path found: {res}");
            }

            return res;
        }

        private void SetCellsToLabyrinth(CardCell cell, int i, int j, bool setTransformPos = false)
        {
            _cardCells[i, j] = cell;

            SetCardToGridValues(i, j);

            if (setTransformPos)
            {
                var (x, y) = GetXYOrigin(i, j);
                cell.transform.localPosition = new Vector3(x, y, cell.transform.position.z);
            }
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

        private (int X, int Y) GetXYOrigin(int row, int col)
        {
            return GetXY((row, col), (2, 0));
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