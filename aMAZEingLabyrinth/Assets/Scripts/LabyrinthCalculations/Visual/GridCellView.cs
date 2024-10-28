using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

namespace GameCore
{
    public class GridCellView : MonoBehaviour
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

        private string ShowString(bool value)
        {
            return value ? "1" : "0";
        }

        private TileBase ShowTile(bool value)
        {
            return value ? _tileGround : _tileWall;
        }
    }
}