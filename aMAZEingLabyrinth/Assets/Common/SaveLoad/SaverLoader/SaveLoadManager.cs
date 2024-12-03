using System.Collections.Generic;
using VContainer;
using VContainer.Unity;
using GameManagement;

namespace SaveLoadNamespace
{
    public class SaveLoadManager :
        IPostInitializable,
        IAppQuitListener
    {
        private readonly IEnumerable<ISaveLoad> _saveLoads;

        private readonly GameRepository _gameRepository;

        private readonly IObjectResolver _context;

        public SaveLoadManager(IEnumerable<ISaveLoad> saveLoads, GameRepository gameRepository, IObjectResolver context)
        {
            _saveLoads = saveLoads;
            _gameRepository = gameRepository;
            _context = context;
        }

        public void PostInitialize()
        {
            _gameRepository.LoadState();

            foreach (var loader in _saveLoads)
            {
                loader.Load(_gameRepository, _context);
            }
        }

        public void OnAppQuit()
        {
            foreach (var loader in _saveLoads)
            {
                loader.Save(_gameRepository, _context);
            }
            //_gameRepository.SaveState();
        }
    }
}