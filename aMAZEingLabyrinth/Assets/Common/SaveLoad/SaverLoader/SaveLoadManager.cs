using System.Collections.Generic;
using VContainer;
using GameManagement;

namespace SaveLoadNamespace
{
    public class SaveLoadManager :
        IStartGameListener,
        IEndGameListener,
        IAppQuitListener
    {
        public bool IsDataInRepository => _isDataInRepository;

        private readonly IEnumerable<ISaveLoad> _saveLoads;

        private readonly GameRepository _gameRepository;

        private readonly IObjectResolver _context;

        private bool _isDataInRepository = true;

        public SaveLoadManager(IEnumerable<ISaveLoad> saveLoads, GameRepository gameRepository, IObjectResolver context)
        {
            _saveLoads = saveLoads;
            _gameRepository = gameRepository;
            _context = context;
        }

        public void LoadGame()
        {
            _gameRepository.OnNoDataFound += SetNoDataInRepository;

            _gameRepository.LoadState();

            foreach (var loader in _saveLoads)
            {
                loader.Load(_gameRepository, _context);
            }
        }

        public void LoadNewGame()
        {
            foreach (var loader in _saveLoads)
            {
                loader.LoadNewGame(_context);
            }
        }

        public void SetNoDataInRepository()
        {
            _isDataInRepository = false;
        }

        public void StartGame()
        {
            _gameRepository.SetCanSave(true);
        }

        public void EndGame()
        {
            _gameRepository.SetCanSave(false);
        }

        public void OnAppQuit()
        {
            foreach (var loader in _saveLoads)
            {
                loader.Save(_gameRepository, _context);
            }
            _gameRepository.SaveState();

            _gameRepository.OnNoDataFound += SetNoDataInRepository;
        }
    }
}