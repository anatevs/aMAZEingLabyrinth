using GameCore;
using VContainer;

namespace SaveLoadNamespace
{
    public class MovingCellsSaveLoad : SaveLoader<CellsData, MovableCellsManager>
    {
        protected override CellsData ConvertDataToParams(MovableCellsManager movableCellsManager)
        {
            var result = new CellsData();

            movableCellsManager.SetupCells();

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

            movableManager.GenerateMovableCellsDefault();
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