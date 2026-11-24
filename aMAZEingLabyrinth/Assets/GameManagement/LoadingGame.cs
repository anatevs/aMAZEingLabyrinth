using Cysharp.Threading.Tasks;
using SaveLoadNamespace;
using UnityEngine.SceneManagement;
using VContainer.Unity;

namespace GameManagement
{
    public sealed class LoadingGame :
        IInitializable,
        IPostInitializable
    {
        private readonly SaveLoadManager _loadManager;

        private readonly int _gameSceneID = 1;

        public LoadingGame(SaveLoadManager loadManager)
        {
            _loadManager = loadManager;
        }

        async void IInitializable.Initialize()
        {
            await LoadScene(_gameSceneID);
        }

        void IPostInitializable.PostInitialize()
        {
            _loadManager.LoadGame();
        }

        private async UniTask LoadScene(int sceneID)
        {
            var operation = SceneManager.LoadSceneAsync(sceneID);

            while (!operation.isDone)
            {
                await UniTask.Yield();
            }
        }
    }
}