using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    [CreateAssetMenu(
        fileName = "CardCellPrefabs",
        menuName = "Configs/CardCellPrefab"
        )]
    public class CellPrefabsConfig : ScriptableObject
    {
        [SerializeField]
        private CardCell[] _prefabs = new CardCell[3];

        private readonly Dictionary<CellGeometry, CardCell> _prefabsDict = new();

        private void OnEnable()
        {
            foreach (var prefab in _prefabs)
            {
                _prefabsDict.Add(prefab.Geometry, prefab);
            }
        }

        public CardCell GetCardCell(CellGeometry cellGeometry)
        {
            return _prefabsDict[cellGeometry];
        }
    }
}