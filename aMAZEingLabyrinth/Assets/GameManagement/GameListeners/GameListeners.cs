namespace GameManagement
{
    public interface IGameListener
    {
    }

    public interface IAppQuitListener : IGameListener
    {
        public void OnAppQuit();
    }
}