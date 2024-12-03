using GameCore;
using System.Collections.Generic;
using System.Linq;
using VContainer;

namespace SaveLoadNamespace
{
    public class MovingCellsSaveLoad : SaveLoader<CellsData, MovableCellsManager>
    {
        private CellPrefabsConfig _cellPrefabsConfig;

        private MovableCellsConfig _movableCellsConfig;

        private (int Rows, int Cols) _size = LabyrinthParams.Size;

        private readonly int[] _fixedRowCols = LabyrinthParams.FixedRowCols;
        private readonly int[] _movableRowCols = LabyrinthParams.MovableRowCols;

        private int _fixedAmount = LabyrinthParams.FixedAmount;

        private (int Row, int Col)[] _movableCellsRowCol;

        

        protected override CellsData ConvertDataToParams(MovableCellsManager movableCellsManager)
        {
            var result = new CellsData();

            var movableCells = movableCellsManager.MovableCells;
            var playableCell = movableCellsManager.PlayableCell;

            return result;
        }

        protected override void LoadDefault(IObjectResolver context)
        {
            var movableManager = context.Resolve<MovableCellsManager>();

            GenerateMovableCellsDefault(movableManager);
        }

        protected override void SetupParamsData(CellsData paramsData, IObjectResolver context)
        {
            
        }

        private void InitMovableIndexes()
        {
            var movableAmount = _size.Rows * _size.Cols + 1 - _fixedAmount;

            if (_movableCellsConfig.Count != movableAmount)
            {
                throw new System.Exception("Movable cells count in config and in collection must be equal!");
            }

            _movableCellsRowCol =
                new (int Row, int Col)[movableAmount - 1];

            int count = 0;
            foreach (var i in _movableRowCols)
            {
                for (int j = 0; j < _size.Cols; j++)
                {
                    _movableCellsRowCol[count] = (i, j);
                    count++;
                }
            }

            foreach (var i in _fixedRowCols)
            {
                foreach (var j in _movableRowCols)
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

            foreach (var (Row, Col) in _movableCellsRowCol)
            {
                var rotation = rotations[UnityEngine.Random.Range(0, rotations.Length)];

                var randomIndex = indexList[UnityEngine.Random.Range(0, indexList.Count)];

                var cellType = _movableCellsConfig.GetCardCellType(randomIndex);

                indexList.Remove(randomIndex);

                var cellData = new OneCellData(cellType.Geometry, cellType.Reward, rotation, (Row, Col));

                movableCellsManager.AddMovableData(cellData);
            }

            var plCellType = _movableCellsConfig.GetCardCellType(indexList[0]);

            var plCellData = new OneCellData(plCellType.Geometry, plCellType.Reward, 0, (-1, -1));

            movableCellsManager.SetPlayableData(plCellData);
        }

        private OneCellData FromCellToData(CardCell cell)
        {
            var data = new OneCellData(cell.Geometry, cell.Reward,
                (int)cell.transform.eulerAngles.z, (0, 0));

            return data;
        }
    }
}