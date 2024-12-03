namespace GameCore
{
    public static class LabyrinthParams
    {
        public static (int Rows, int Cols) Size => _size;

        public static int[] FixedRowCols => _fixedRowCols;

        public static int[] MovableRowCols => _movableRowCols;

        public static int FixedAmount => 16;

        private static readonly (int Rows, int Cols) _size = (7, 7);

        private static readonly int[] _fixedRowCols = new int[4] { 0, 2, 4, 6 };
        private static readonly int[] _movableRowCols = new int[3] { 1, 3, 5 };
    }
}