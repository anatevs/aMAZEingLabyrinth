using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

namespace SaveLoadNamespace
{
    public class GameRepository : IGameRepository
    {
        public Dictionary<string, string> objectsPresentations = new Dictionary<string, string>();

        private const string SAVE_KEY = "SaveLoadGameData";

        private readonly string _password = "hw4";

        private readonly byte[] _salt = new byte[] { 10, 39, 80, 240, 90, 3, 74, 67, 173 };

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

            //string encryptedData = AESCryptographicSystem.Encrypt(gameData, _password, _salt);

            PlayerPrefs.SetString(SAVE_KEY, gameData);
        }

        public void LoadState()
        {
            if (PlayerPrefs.HasKey(SAVE_KEY))
            {
                string gameData = PlayerPrefs.GetString(SAVE_KEY);

                //string gameData = AESCryptographicSystem.Decrypt(encryptedData, _password, _salt);

                objectsPresentations = JsonConvert.DeserializeObject<Dictionary<string, string>>(gameData);
            }
        }
    }
}