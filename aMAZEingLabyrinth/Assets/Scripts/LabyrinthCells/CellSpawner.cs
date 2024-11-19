﻿using UnityEngine;

namespace GameCore
{
    public class CellSpawner
    {
        private readonly CellPrefabsConfig _cellPrefabsConfig;

        public CellSpawner(CellPrefabsConfig prefabs)
        {
            _cellPrefabsConfig = prefabs;
        }

        public CardCell SpawnCell(CellGeometry geometry, RewardName reward, int rotAngle, int localX, int localY, Transform parent)
        {
            var cell = GameObject.Instantiate(_cellPrefabsConfig.GetCardCell(geometry));

            cell.transform.SetParent(parent);

            cell.transform.localPosition = new Vector3(localX, localY, cell.transform.position.z);

            cell.Init(reward, rotAngle);

            return cell;
        }

        public CardCell SpawnCell(CellData cellData, Transform parentTransform)
        {
            return SpawnCell(cellData.Geometry, cellData.Reward, cellData.RotationDeg, cellData.X, cellData.Y, parentTransform);
        }
    }
}