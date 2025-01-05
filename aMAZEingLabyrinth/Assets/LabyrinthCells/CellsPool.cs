using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    public class CellsPool
    {
        private readonly CellPrefabsConfig _cellPrefabsConfig;

        private readonly MovableCellsConfig _movableCellsConfig;

        private readonly Dictionary<RewardName, CardCell> _rewardCells = new();

        private readonly Dictionary<CellGeometry,
            (int currentIndex, List<CardCell> cards)> _emptyCells = new();

        public CellsPool(CellPrefabsConfig prefabs, MovableCellsConfig movableCellsConfig)
        {
            _cellPrefabsConfig = prefabs;
            _movableCellsConfig = movableCellsConfig;
        }

        public void PopulatePool(Transform parent)
        {
            for (int i = 0; i < _movableCellsConfig.Count; i++)
            {
                var cellType = _movableCellsConfig.GetCardCellType(i);

                var cell = InitSpawnCell(cellType.Geometry, cellType.Reward, parent);

                if (cellType.Reward != RewardName.None)
                {
                    _rewardCells.Add(cellType.Reward, cell);
                }
                else
                {
                    if (_emptyCells.ContainsKey(cellType.Geometry))
                    {
                        _emptyCells[cellType.Geometry].cards.Add(cell);
                    }
                    else
                    {
                        var value = (0, new List<CardCell>() { cell });
                        _emptyCells.Add(cellType.Geometry, value);
                    }
                }
            }
        }


        private CardCell InitSpawnCell(CellGeometry geometry, RewardName reward, Transform parent)
        {
            var cell = GameObject.Instantiate(_cellPrefabsConfig.GetCardCell(geometry));

            cell.transform.SetParent(parent);

            return cell;
        }

        public CardCell SpawnFromPool(OneCellData cellData)
        {
            return SpawnFromPool(cellData.Geometry, cellData.Reward,
                cellData.RotationDeg, cellData.Origin.X, cellData.Origin.Y);
        }

        public CardCell SpawnFromPool(CellGeometry geometry, RewardName reward,
            int rotAngle, int localX, int localY)
        {
            CardCell cell;

            if (reward == RewardName.None)
            {
                var value = _emptyCells[geometry];

                var currentIndex = value.currentIndex;

                cell = value.cards[currentIndex];

                var nextIndex = (currentIndex + 1) % value.cards.Count;

                _emptyCells[geometry] = (nextIndex, _emptyCells[geometry].cards);
            }
            else
            {
                cell = _rewardCells[reward];
            }

            cell.transform.localPosition = new Vector3(localX, localY, cell.transform.position.z);

            cell.Init(reward, rotAngle);

            return cell;
        }

        public void UnspawnPooledCells()
        {
            foreach (var cell in _rewardCells.Values)
            {
                cell.gameObject.SetActive(false);
            }

            foreach (var (_, cards) in _emptyCells.Values)
            {
                foreach (var cell in cards)
                {
                    cell.gameObject.SetActive(false);
                }
            }
        }

        public CardCell SpawnCell(OneCellData cellData, Transform parentTransform)
        {
            return SpawnCell(cellData.Geometry, cellData.Reward, cellData.RotationDeg,
                cellData.Origin.X, cellData.Origin.Y, parentTransform);
        }

        private CardCell SpawnCell(CellGeometry geometry, RewardName reward, int rotAngle, int localX, int localY, Transform parent)
        {
            var cell = GameObject.Instantiate(_cellPrefabsConfig.GetCardCell(geometry));

            cell.transform.SetParent(parent);

            cell.transform.localPosition = new Vector3(localX, localY, cell.transform.position.z);

            cell.Init(reward, rotAngle);

            return cell;
        }
    }
}