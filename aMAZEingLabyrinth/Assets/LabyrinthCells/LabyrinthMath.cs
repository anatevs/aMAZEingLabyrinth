namespace GameCore
{
    public static class LabyrinthMath
    {
        public static (int Rows, int Cols) Size => _size;

        public static int[] FixedRowCols => _fixedRowCols;

        public static int[] MovableRowCols => _movableRowCols;

        public static int FixedAmount => _fixedAmount;
        public static int UnfixedAmount => _unfixedAmount;

        public static int CellSize => _cellSize;

        public static (int row, int col)[] MovableCellsRowCol => _movableCellsRowCol;

        private static readonly int _cellSize = 3;

        private static readonly (int Rows, int Cols) _size = (7, 7);

        private static readonly int[] _fixedRowCols = new int[4] { 0, 2, 4, 6 };
        private static readonly int[] _movableRowCols = new int[3] { 1, 3, 5 };

        private static readonly (int row, int col)[] _movableCellsRowCol;

        private static readonly int _fixedAmount;
        private static readonly int _unfixedAmount;

        static LabyrinthMath()
        {
            _fixedAmount = _fixedRowCols.Length * _fixedRowCols.Length;
            _unfixedAmount = _size.Rows * _size.Cols + 1 - _fixedAmount;

            _movableCellsRowCol = new (int Row, int Col)[_unfixedAmount - 1];
            InitMovableIndexes();
        }

        public static (int X, int Y) GetXYCenter(int row, int col)
        {
            return GetXY((row, col), (1, 1));
        }

        public static (int X, int Y) GetXYOrigin(int row, int col)
        {
            return GetXY((row, col), (2, 0));
        }

        public static (int X, int Y) GetXY((int i, int j) cellIndex, (int ic, int jc) elementIndex)
        {
            var x = cellIndex.j * _cellSize + elementIndex.jc;
            var y = _size.Rows * _cellSize - 1 - (cellIndex.i * _cellSize + elementIndex.ic);
            return (x, y);
        }

        public static (int xDir, int yDir) GetXYDirection((int i, int j) directionRowCol)
        {
            var x = directionRowCol.j * _cellSize;
            var y = - directionRowCol.i * _cellSize;

            return (x, y);
        }

        public static (int iCell, int jCell) GetCellIndex((int x, int y) coordinates)
        {
            int i = _size.Rows - 1 - coordinates.y / _cellSize;
            int j = coordinates.x / _cellSize;

            return (i, j);
        }

        private static void InitMovableIndexes()
        {
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
    }
}