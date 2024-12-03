using GameCore;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VContainer;

namespace SaveLoadNamespace
{
    public class MovingCellsSaveLoad : SaveLoader<CellsData, MovableCellsManager>
    {
        private readonly MovableCellsConfig _movableCellsConfig;

        private (int Row, int Col)[] _movableCellsRowCol;



        private IObjectResolver _objectResolver;

        public MovingCellsSaveLoad(MovableCellsConfig movableCellsConfig, IObjectResolver objectResolver)
        {
            _movableCellsConfig = movableCellsConfig;



            _objectResolver = objectResolver;
        }


        public void TestLoadDefault()
        {
            LoadDefault(_objectResolver);
        }

        protected override CellsData ConvertDataToParams(MovableCellsManager movableCellsManager)
        {
            var result = new CellsData();

            var movableCells = movableCellsManager.MovableCells;
            var playableCell = movableCellsManager.PlayableCell;

            foreach ( var cell in movableCells )
            {
                var data = FromCellToData(cell);
                result.AddMovableCell(data);
            }

            result.PlayableCellData = FromCellToData(playableCell);

            return result;
        }

        protected override void LoadDefault(IObjectResolver context)
        {
            var movableManager = context.Resolve<MovableCellsManager>();

            GenerateMovableCellsDefault(movableManager);
        }

        protected override void SetupParamsData(CellsData cellsData, IObjectResolver context)
        {
            var movableManager = context.Resolve<MovableCellsManager>();
            movableManager.ClearMovableData();

            foreach (var data in cellsData.MovableCellsData)
            {
                movableManager.AddMovableData(data);
            }

            movableManager.SetPlayableData(cellsData.PlayableCellData);
        }

        private void InitMovableIndexes()
        {
            if (_movableCellsConfig.Count != LabyrinthMath.UnfixedAmount)
            {
                throw new System.Exception("Movable cells count in config and in collection must be equal!");
            }

            _movableCellsRowCol =
                new (int Row, int Col)[LabyrinthMath.UnfixedAmount - 1];

            int count = 0;
            foreach (var i in LabyrinthMath.MovableRowCols)
            {
                for (int j = 0; j < LabyrinthMath.Size.Cols; j++)
                {
                    _movableCellsRowCol[count] = (i, j);
                    count++;
                }
            }

            foreach (var i in LabyrinthMath.FixedRowCols)
            {
                foreach (var j in LabyrinthMath.MovableRowCols)
                {
                    _movableCellsRowCol[count] = (i, j);
                    count++;
                }
            }
        }

        private void GenerateMovableCellsDefault(MovableCellsManager movableCellsManager)
        {
            InitMovableIndexes();

            var indexList = new List<int>(Enumerable
                .Range(0, _movableCellsRowCol.Length + 1));

            int[] rotations = new int[4] { 0, 90, 180, 270 };

            movableCellsManager.ClearMovableData();

            foreach (var (Row, Col) in _movableCellsRowCol)
            {
                var rotation = rotations[UnityEngine.Random.Range(0, rotations.Length)];

                var randomIndex = indexList[UnityEngine.Random.Range(0, indexList.Count)];

                var cellType = _movableCellsConfig.GetCardCellType(randomIndex);

                indexList.Remove(randomIndex);

                var (X, Y) = LabyrinthMath.GetXYOrigin(Row, Col);

                var cellData = new OneCellData(cellType.Geometry, cellType.Reward, rotation, (X, Y));

                movableCellsManager.AddMovableData(cellData);
            }

            var plCellType = _movableCellsConfig.GetCardCellType(indexList[0]);

            var plCellData = new OneCellData(plCellType.Geometry, plCellType.Reward, 0, (-1, -1));

            movableCellsManager.SetPlayableData(plCellData);
        }

        private OneCellData FromCellToData(CardCell cell)
        {
            var data = new OneCellData(cell.Geometry, cell.Reward,
                (int)cell.transform.eulerAngles.z,
                ((int)cell.transform.localPosition.x,
                (int)cell.transform.localPosition.y));

            return data;
        }
    }
}