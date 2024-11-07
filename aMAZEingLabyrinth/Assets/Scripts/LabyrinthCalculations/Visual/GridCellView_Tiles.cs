using UnityEngine;
using UnityEngine.Tilemaps;

namespace GameCore
{
    public sealed class GridCellView_Tiles : MonoBehaviour
    {
        [SerializeField]
        private Tilemap _tilemap;

        [SerializeField]
        private TileBase _tileWall;

        [SerializeField]
        private TileBase _tileGround;

        [SerializeField]
        private TileBase _tilePath;

        [SerializeField]
        private int[] _pos = new int[2];

        [SerializeField]
        private bool _setTile = false;

        [SerializeField]
        private int[] _posGet = new int[2];

        [SerializeField]
        private bool _getTile;

        private void Update ()
        {
            if (_setTile)
            {
                var position = new Vector3Int(_pos[0], _pos[1], 0);
                _tilemap.SetTile(position, _tileWall);
                _setTile = false;
            }

            if (_getTile)
            {
                var position = new Vector3Int(_posGet[0], _posGet[1], 0);
                var tile = _tilemap.GetTile(position);

                if (tile == _tileGround)
                {
                    Debug.Log("ground");
                }
                else if (tile == _tilePath)
                {
                    Debug.Log("path");
                }
                else if (tile == _tileWall)
                {
                    Debug.Log("wall");
                }
                else
                {
                    Debug.Log("no type tile");
                }

                _getTile = false;
            }
        }

        public void SetPathCell(Vector2Int position)
        {
            var pos = new Vector3Int(position.x, position.y, 0);
            _tilemap.SetTile(pos, _tilePath);
        }

        private TileBase GetTile(bool value)
        {
            return value ? _tileGround : _tileWall;
        }

        public bool GetValue((int X, int Y) position)
        {
            var pos = new Vector3Int(position.X, position.Y, 0);
            var tile = _tilemap.GetTile(pos);

            return GetValue(tile);
        }

        private bool GetValue(TileBase tile)
        {
            if (tile == _tileGround)
            {
                return true;
            }
            else if (tile == _tileWall)
            {
                return false;
            }
            else
            {
                Debug.Log("not wall or ground tile!");
                return false;
            }
        }
    }
}