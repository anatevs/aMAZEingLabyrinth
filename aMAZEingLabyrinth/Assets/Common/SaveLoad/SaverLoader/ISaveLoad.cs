using System;
using VContainer;

namespace SaveLoadNamespace
{
    public interface ISaveLoad
    {
        public event Action OnNoDataFound;

        public void Save(IGameRepository gameRepository, IObjectResolver context);

        public void Load(IGameRepository gameRepository, IObjectResolver context);

        public void LoadNewGame(IObjectResolver context);
    }
}