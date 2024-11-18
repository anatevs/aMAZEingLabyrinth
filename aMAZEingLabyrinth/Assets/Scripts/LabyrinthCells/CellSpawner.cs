using UnityEngine;

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

            cell.Init(reward);

            cell.transform.SetParent(parent);

            cell.transform.localPosition = new Vector3(localX, localY, cell.transform.position.z);

            cell.SetInitRotation(rotAngle);

            cell.InitCellValues();

            return cell;
        }
    }
}