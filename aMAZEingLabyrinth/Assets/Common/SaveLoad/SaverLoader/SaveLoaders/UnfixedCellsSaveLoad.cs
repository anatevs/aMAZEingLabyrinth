using GameCore;
using VContainer;

namespace SaveLoadNamespace
{
    public class UnfixedCellsSaveLoad : SaveLoad<CellsData, UnfixedCellsDataConnector>
    {
        protected override CellsData ConvertDataToParams(UnfixedCellsDataConnector unfixedCellsConnector)
        {
            var result = new CellsData();

            unfixedCellsConnector.SetupCells();

            var movableCells = unfixedCellsConnector.MovableCells;
            var playableCell = unfixedCellsConnector.PlayableCell;

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
            var unfixedCellsConnector = context.Resolve<UnfixedCellsDataConnector>();

            unfixedCellsConnector.GenerateMovableCellsDefault();
        }

        protected override void SetupParamsData(CellsData cellsData, IObjectResolver context)
        {
            var movableManager = context.Resolve<UnfixedCellsDataConnector>();
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