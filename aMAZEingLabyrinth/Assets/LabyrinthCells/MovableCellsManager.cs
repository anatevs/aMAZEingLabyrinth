using SaveLoadNamespace;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameCore
{
    public class MovableCellsManager
    {
        public event Action OnCellsRequested;

        public IEnumerable<CardCell> MovableCells => _movableCells;

        public CardCell PlayableCell => _playableCell;

        public CellsData InitCellsData => _initCellsData;

        private readonly CellsData _initCellsData = new();

        private readonly List<CardCell> _movableCells = new();

        private CardCell _playableCell;

        private readonly MovableCellsConfig _movableCellsConfig;

        public MovableCellsManager(MovableCellsConfig movableCellsConfig)
        {
            _movableCellsConfig = movableCellsConfig;
        }

        public void AddMovableData(OneCellData cellData)
        {
            _initCellsData.AddMovableCell(cellData);
        }

        public void SetPlayableData(OneCellData cellData)
        {
            _initCellsData.SetPlayableCell(cellData);
        }

        public void ClearMovableData()
        {
            _initCellsData.ClearMovableData();
        }

        public void SetPlayable(CardCell playableCell)
        {
            _playableCell = playableCell;
        }

        public void AddMovable(CardCell cardCell)
        {
            _movableCells.Add(cardCell);
        }

        public void SetupCells()
        {
            _movableCells.Clear();

            OnCellsRequested?.Invoke();
        }

        public void GenerateMovableCellsDefault()
        {
            var indexList = new List<int>(Enumerable
                .Range(0, LabyrinthMath.MovableCellsRowCol.Length + 1));

            int[] rotations = new int[4] { 0, 90, 180, 270 };

            ClearMovableData();

            foreach (var (Row, Col) in LabyrinthMath.MovableCellsRowCol)
            {
                var rotation = rotations[UnityEngine.Random.Range(0, rotations.Length)];

                var randomIndex = indexList[UnityEngine.Random.Range(0, indexList.Count)];

                var cellType = _movableCellsConfig.GetCardCellType(randomIndex);

                indexList.Remove(randomIndex);

                var (X, Y) = LabyrinthMath.GetXYOrigin(Row, Col);

                var cellData = new OneCellData(cellType.Geometry, cellType.Reward, rotation, (X, Y));

                AddMovableData(cellData);
            }

            var plCellType = _movableCellsConfig.GetCardCellType(indexList[0]);

            var plCellData = new OneCellData(plCellType.Geometry, plCellType.Reward, 0, (-1, -1));

            SetPlayableData(plCellData);
        }
    }
}