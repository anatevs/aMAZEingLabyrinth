using Cysharp.Threading.Tasks;
using SaveLoadNamespace;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;

namespace GameManagement
{
    public sealed class LoadingGame : MonoBehaviour
    {
        private SaveLoadManager _loadManager;

        private readonly int _gameSceneID = 1;

        [Inject]
        public void Construct(SaveLoadManager loadManager)
        {
            _loadManager = loadManager;
        }

        private async void Start()
        {
            _loadManager.LoadGame();

            await SceneManager.LoadSceneAsync(_gameSceneID);
        }
    }
}