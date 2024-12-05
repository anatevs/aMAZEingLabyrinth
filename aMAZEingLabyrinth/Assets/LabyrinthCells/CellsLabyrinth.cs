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
        private CellPrefabsConfig _cellPrefabsConfig;

        [SerializeField]
        private MovableCellsConfig _movableCellsConfig;

        [SerializeField]
        private PlayableCell _playableCell;

        private CellSpawner _cellSpawner;

        private LabyrinthGrid _grid;

        private CardCell[,] _cardCells;


        [Header("Print cell test")]
        [SerializeField]
        private int[] _printRowCol = new int[2];

        [SerializeField]
        private bool _printCell;

        private UnfixedCellsDataConnector _unfixedCellsConnector;

        [Inject]
        public void Construct(UnfixedCellsDataConnector unfixedCellsConnector)
        {
            _unfixedCellsConnector = unfixedCellsConnector;

            _unfixedCellsConnector.OnCellsRequested += SendCellsToConnector;
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

            _cellSpawner = new CellSpawner(_cellPrefabsConfig);

            //InitMovableCells();
            //InitAllMovableCrossType();
        }

        private void OnDisable()
        {
            _unfixedCellsConnector.OnCellsRequested -= SendCellsToConnector;
        }

        private void SendCellsToConnector()
        {
            foreach (var (Row, Col) in LabyrinthMath.MovableCellsRowCol)
            {
                var cell = _cardCells[Row, Col];
                _unfixedCellsConnector.AddMovable(cell);
            }

            _unfixedCellsConnector.SetPlayable(_playableCell.CardCell);
        }

        public void InitMovableCells()
        {
            InitMovableCellsLoad(_unfixedCellsConnector.InitCellsData);
        }

        private void InitMovableCellsLoad(CellsData cellsData)
        {
            foreach (var cellData in cellsData.MovableCellsData)
            {
                InitLabyrinthCellFromData(cellData, _movableParentTransform);
            }

            InitPlayebleCellFromData(cellsData.PlayableCellData, _movableParentTransform);
        }

        private void InitLabyrinthCellFromData(OneCellData cellData, Transform parentTransform)
        {
            var cell = _cellSpawner.SpawnCell(cellData, parentTransform);

            var (Row, Col) = LabyrinthMath.GetCellIndex(cellData.Origin);

            SetCellsToLabyrinth(cell, Row, Col);
        }

        private void InitPlayebleCellFromData(OneCellData cellData, Transform parentTransform)
        {
            var playableCell = _cellSpawner.SpawnCell(cellData, parentTransform);

            _playableCell.ReplacePlayableCell(playableCell, out _);
        }

        private void InitAllMovableCrossType()
        {
            var cellGeometry = CellGeometry.Cross;

            var indexList = new List<int>(Enumerable
                .Range(0, LabyrinthMath.MovableCellsRowCol.Length + 1));

            var spawner = new CellSpawner(_cellPrefabsConfig);

            foreach (var (Row, Col) in LabyrinthMath.MovableCellsRowCol)
            {
                var rotation = 0;

                var randomIndex = indexList[UnityEngine.Random.Range(0, indexList.Count)];

                var cellType = _movableCellsConfig.GetCardCellType(randomIndex);

                indexList.Remove(randomIndex);


                var (X, Y) = LabyrinthMath.GetXYOrigin(Row, Col);

                Debug.Log($"original rowcol: {(Row, Col)}");
                Debug.Log($"xy: {(X, Y)}");

                var cell = spawner.SpawnCell(cellGeometry, cellType.Reward, rotation, X, Y, _movableParentTransform);

                Debug.Log($"calc rowcol: {LabyrinthMath.GetCellIndex(((int)cell.transform.localPosition.x, (int)cell.transform.localPosition.y))}");

                SetCellsToLabyrinth(cell, Row, Col);
            }

            var plCellType = _movableCellsConfig.GetCardCellType(indexList[0]);

            var playableCellCard = spawner.SpawnCell(cellGeometry, plCellType.Reward, 0, 0, 0, _movableParentTransform);

            _playableCell.ReplacePlayableCell(playableCellCard, out _);
        }


        private void Update()
        {
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

            _pathMarkersPool.UnspawnAll();
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

        private bool FindPathRowCol((int i, int j) startRowCol, (int i, int j) endRowCol, out List<(int x, int y)> resultXY)
        {
            var start = LabyrinthMath.GetXYCenter(startRowCol.i, startRowCol.j);
            var end = LabyrinthMath.GetXYCenter(endRowCol.i, endRowCol.j);

            return FindPath(start, end, out resultXY);
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