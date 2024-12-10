using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using SaveLoadNamespace;
using VContainer;

namespace GameCore
{
    public sealed class CellsLabyrinth : MonoBehaviour
    {
        [SerializeField]
        private CardCell[] _fixedCells;

        [SerializeField]
        private PathMarkersPool _pathMarkersPool;

        [SerializeField]
        private Transform _movableParentTransform;

        [SerializeField]
        private MovableCellsConfig _movableCellsConfig;

        [SerializeField]
        private PlayableCell _playableCell;

        [SerializeField]
        private bool _isCrossCells;

        private LabyrinthGrid _grid;

        private CardCell[,] _cardCells;

        private UnfixedCellsDataConnector _unfixedCellsConnector;

        private CellsPool _cellsPool;

        [Inject]
        public void Construct(UnfixedCellsDataConnector unfixedCellsConnector, CellsPool cellsPool)
        {
            _cellsPool = cellsPool;

            _unfixedCellsConnector = unfixedCellsConnector;
        }

        private void OnEnable()
        {
            _unfixedCellsConnector.OnCellsRequested += SendCellsToConnector;
        }

        private void OnDisable()
        {
            _unfixedCellsConnector.OnCellsRequested -= SendCellsToConnector;
        }

        private void Start()
        {
            _grid = new LabyrinthGrid((LabyrinthMath.Size.Rows * LabyrinthMath.CellSize,
                LabyrinthMath.Size.Cols * LabyrinthMath.CellSize));

            _cardCells = new CardCell[LabyrinthMath.Size.Rows,
                LabyrinthMath.Size.Cols];

            foreach (var cell in _fixedCells)
            {
                var (iCell, jCell) = LabyrinthMath.GetCellIndex(((int)cell.transform.localPosition.x,
                    (int)cell.transform.localPosition.y));

                cell.InitCellValues();

                SetCellsToLabyrinth(cell, iCell, jCell);
            }

            _cellsPool.PopulatePool(_movableParentTransform);
        }

        private void SendCellsToConnector()
        {
            if (!_isCrossCells)
            {
                foreach (var (Row, Col) in LabyrinthMath.MovableCellsRowCol)
                {
                    var cell = _cardCells[Row, Col];
                    if (cell == null)
                    {
                        Debug.Log("null cell in labyrinth!");
                        return;
                    }
                    _unfixedCellsConnector.AddMovable(cell);
                }
                if (_playableCell.CardCell == null)
                {
                    Debug.Log("null playable cell!");
                    return;
                }
                _unfixedCellsConnector.SetPlayable(_playableCell.CardCell);
            }
        }

        public void InitMovableCells()
        {
            if (!_isCrossCells)
            {
                InitUnfixedCellsLoad(_unfixedCellsConnector.InitCellsData);
            }

            else
            {
                _cellsPool.UnspawnPooledCells();

                _unfixedCellsConnector.GenerateCrossTypeData();

                InitCrossType(_unfixedCellsConnector.CrossedCellsData);
            }
        }

        private void InitUnfixedCellsLoad(CellsData cellsData)
        {
            InitPlayebleCellFromData(cellsData.PlayableCellData, _movableParentTransform);

            foreach (var cellData in cellsData.MovableCellsData)
            {
                InitLabyrinthCellFromData(cellData);
            }
        }

        private void InitLabyrinthCellFromData(OneCellData cellData)
        {
            var cell = _cellsPool.SpawnFromPool(cellData);

            var (Row, Col) = LabyrinthMath.GetCellIndex(cellData.Origin);

            SetCellsToLabyrinth(cell, Row, Col);
        }

        private void InitPlayebleCellFromData(OneCellData cellData, Transform parentTransform)
        {
            var playableCell = _cellsPool.SpawnFromPool(cellData);

            _playableCell.ReplacePlayableCell(playableCell, out var oldPlayable);

            if (oldPlayable != null)
            {
                oldPlayable.transform.parent = parentTransform;
            }
        }

        private void InitCrossType(CellsData cellsData)
        {
            foreach (var cellData in cellsData.MovableCellsData)
            {
                var cell = _cellsPool.SpawnCell(cellData, _movableParentTransform);

                var (Row, Col) = LabyrinthMath.GetCellIndex(cellData.Origin);

                SetCellsToLabyrinth(cell, Row, Col);
            }

            var playableCell = _cellsPool.SpawnCell(cellsData.PlayableCellData, _movableParentTransform);

            _playableCell.ReplacePlayableCell(playableCell, out _);
        }


        public Vector3 GetCellCenterCoordinates(Vector3 pos)
        {
            (int i, int j) = LabyrinthMath.GetCellIndex(((int)(pos.x - transform.position.x),
                (int)(pos.y - transform.position.y)));

            return _cardCells[i, j].transform.position;
        }

        public void MakeShift(int shiftRow, int shiftCol, out (int row, int col) oppositeIndex)
        {
            oppositeIndex = (-1, -1);

            bool isRow = LabyrinthMath.MovableRowCols.Contains(shiftRow);
            bool isCol = LabyrinthMath.MovableRowCols.Contains(shiftCol);

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
                iterNumber = LabyrinthMath.Size.Cols - 1;
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
                iterNumber = LabyrinthMath.Size.Rows - 1;
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

            oppositeIndex = (row, col);

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

        public bool HasCellReward((int x, int y) localXY, RewardName reward)
        {
            var (i, j) = LabyrinthMath.GetCellIndex(localXY);

            return _cardCells[i, j].Reward == reward;
        }

        public bool FindPath((int, int) startXY, Vector2 endCellGlobalXY, out List<(int x, int y)> path)
        {
            var endXY = ((int)(endCellGlobalXY.x - transform.position.x),
                (int)(endCellGlobalXY.y - transform.position.y));

            var result = FindPath(startXY, endXY, out path);

            return result;
        }

        private bool FindPath((int x, int y) startXY, (int x, int y) endXY, out List<(int x, int y)> path)
        {
            path = new List<(int x, int y)>();

            var start = new Vector2Int(startXY.x, startXY.y);
            var end = new Vector2Int(endXY.x, endXY.y);

            var resultBool = _grid.TryFindAStarPath(start, end, out List<Vector2Int> resultVector2);

            _pathMarkersPool.SpawnedTime = Time.time;

            _pathMarkersPool.Spawn(start.x, start.y);
            _pathMarkersPool.Spawn(end.x, end.y);

            if (resultBool)
            {
                foreach (var pathPoint in resultVector2)
                {
                    path.Add((pathPoint.x, pathPoint.y));

                    _pathMarkersPool.Spawn(pathPoint.x, pathPoint.y);
                }
            }

            Debug.Log($"path found: {resultBool}");

            return resultBool;
        }

        private void SetCellsToLabyrinth(CardCell cell, int i, int j, bool setTransformPos = false)
        {
            _cardCells[i, j] = cell;

            SetCardToGridValues(i, j);

            if (setTransformPos)
            {
                var (x, y) = LabyrinthMath.GetXYOrigin(i, j);
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
                    var xy = LabyrinthMath.GetXY((i, j), (ic, jc));

                    _grid.SetValue(card.GetValue(ic, jc), xy);
                }
            }
        }
    }
}