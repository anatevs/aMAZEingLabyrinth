using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

namespace SaveLoadNamespace
{
    public class GameRepository : IGameRepository
    {
        public Dictionary<string, string> objectsPresentations = new();

        private const string SAVE_KEY = "SaveLoadGameData";

        public void SetData<T>(T value)
        {
            string keyName = typeof(T).Name;
            string paramsJson = JsonConvert.SerializeObject(value);
            objectsPresentations[keyName] = paramsJson;
        }

        public T GetData<T>()
        {
            string paramsInJson = objectsPresentations[typeof(T).Name];
            return JsonConvert.DeserializeObject<T>(paramsInJson);
        }

        public bool TryGetData<T>(out T value)
        {
            string paramsInJson;
            if (objectsPresentations.TryGetValue(typeof(T).Name, out paramsInJson))
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
            string gameData = JsonConvert.SerializeObject(objectsPresentations);

            PlayerPrefs.SetString(SAVE_KEY, gameData);
        }

        public void LoadState()
        {
            if (PlayerPrefs.HasKey(SAVE_KEY))
            {
                string gameData = PlayerPrefs.GetString(SAVE_KEY);

                objectsPresentations = JsonConvert.DeserializeObject<Dictionary<string, string>>(gameData);
            }
        }
    }
}