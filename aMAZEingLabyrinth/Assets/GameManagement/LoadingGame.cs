using Cysharp.Threading.Tasks;
using SaveLoadNamespace;
using UnityEngine.SceneManagement;
using VContainer.Unity;

namespace GameManagement
{
    public sealed class LoadingGame : IInitializable, IPostInitializable
    {
        private SaveLoadManager _loadManager;

        private readonly int _gameSceneID = 1;

        public LoadingGame(SaveLoadManager loadManager)
        {
            _loadManager = loadManager;
        }

        async void IInitializable.Initialize()
        {
            await SceneManager.LoadSceneAsync(_gameSceneID);
        }

        void IPostInitializable.PostInitialize()
        {
            //var scope = LifetimeScope.Find<SceneLifetimeScope>();

            _loadManager.LoadGame();
        }
    }
}