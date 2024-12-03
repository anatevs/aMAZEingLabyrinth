using VContainer;

namespace SaveLoadNamespace
{
    public interface ISaveLoad
    {
        public void Save(IGameRepository gameRepository, IObjectResolver context);

        public void Load(IGameRepository gameRepository, IObjectResolver context);
    }
}