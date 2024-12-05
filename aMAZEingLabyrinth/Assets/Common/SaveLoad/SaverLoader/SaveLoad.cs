using System;
using VContainer;

namespace SaveLoadNamespace
{
    public abstract class SaveLoad<TData, TService> : ISaveLoad
    {
        public event Action OnNoDataFound;

        public void Load(IGameRepository gameRepository, IObjectResolver context)
        {

            if (gameRepository.TryGetData(out TData paramsData))
            {
                SetupParamsData(paramsData, context);

                OnNoDataFound?.Invoke();
            }
            else
            {
                LoadDefault(context);
            }
        }

        public void Save(IGameRepository gameRepository, IObjectResolver context)
        {
            TService service = context.Resolve<TService>();

            TData paramsData = ConvertDataToParams(service);

            gameRepository.SetData<TData>(paramsData);
        }

        public void LoadNewGame(IObjectResolver context)
        {
            LoadDefault(context);
        }

        protected abstract void SetupParamsData(TData paramsData, IObjectResolver context);

        protected abstract void LoadDefault(IObjectResolver context);

        protected abstract TData ConvertDataToParams(TService service);
    }
}