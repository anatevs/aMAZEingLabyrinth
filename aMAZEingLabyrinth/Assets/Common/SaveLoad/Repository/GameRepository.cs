using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

namespace SaveLoadNamespace
{
    public sealed class GameRepository : IGameRepository
    {
        public Dictionary<string, string> ObjectsPresentations = new();

        private const string SAVE_KEY = "SaveLoadGameData";

        public void SetData<T>(T value)
        {
            string keyName = typeof(T).Name;
            string paramsJson = JsonConvert.SerializeObject(value);
            ObjectsPresentations[keyName] = paramsJson;
        }

        public T GetData<T>()
        {
            string paramsInJson = ObjectsPresentations[typeof(T).Name];
            return JsonConvert.DeserializeObject<T>(paramsInJson);
        }

        public bool TryGetData<T>(out T value)
        {
            string paramsInJson;
            if (ObjectsPresentations.TryGetValue(typeof(T).Name, out paramsInJson))
            {
                value = JsonConvert.DeserializeObject<T>(paramsInJson);
                return true;
            }
            else
            {
                value = default(T);
                return false;
            }
        }

        public void SaveState()
        {
            string gameData = JsonConvert.SerializeObject(ObjectsPresentations);

            PlayerPrefs.SetString(SAVE_KEY, gameData);
        }

        public void LoadState()
        {
            if (PlayerPrefs.HasKey(SAVE_KEY))
            {
                string gameData = PlayerPrefs.GetString(SAVE_KEY);

                ObjectsPresentations = JsonConvert.DeserializeObject<Dictionary<string, string>>(gameData);
            }
        }
    }
}