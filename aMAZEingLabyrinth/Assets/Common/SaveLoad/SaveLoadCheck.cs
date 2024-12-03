using GameCore;
using Newtonsoft.Json;
using System.Collections;
using UnityEngine;

namespace SaveLoadNamespace
{
    public class SaveLoadCheck : MonoBehaviour
    {
        private CellsData _cellsData;

        private CellsData _loadedCellsData;

        private readonly string _saveCheck = "CheckData";

        private void Start ()
        {
            var data = new OneCellData(CellGeometry.Line, RewardName.Map, 0, (1, 10));
            _cellsData = new CellsData(data);

            _loadedCellsData = Load();

            if (_loadedCellsData != null)
            {
                
            }

            Save();
        }

        private void Save()
        {
            string json = JsonConvert.SerializeObject(_cellsData);
            PlayerPrefs.SetString(_saveCheck, json);
        }

        private CellsData Load()
        {
            if (PlayerPrefs.HasKey(_saveCheck))
            {
                string json = PlayerPrefs.GetString(_saveCheck);
                return JsonConvert.DeserializeObject<CellsData>(json);
            }
            else
            {
                Debug.Log("no data in prefs");
                return null;
            }
        }
    }
}