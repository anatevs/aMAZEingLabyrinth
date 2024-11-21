using System.Collections.Generic;

namespace GameCore
{
    public static class CellGeometryInfo
    {
        private static readonly Dictionary<CellGeometry, List<(int, int)>> _geometries = new()
        {
            {CellGeometry.Angle, new()
            {
                (1, 0),
                (0, 1)
            }
        },
            {CellGeometry.TShape, new()
            {
                (1, 0),
                (-1, 0),
                (0, 1)
            }
        },
            {CellGeometry.Line, new()
            {
                (1, 0),
                (-1, 0)
            }
        },
            {CellGeometry.Cross, new()
            {
                (1, 0),
                (-1, 0),
                (0, 1),
                (0, -1)
            }
        }
        };

        public static List<(int, int)> GetGeometry(CellGeometry type)
        {
            return _geometries[type];
        }
    }
}