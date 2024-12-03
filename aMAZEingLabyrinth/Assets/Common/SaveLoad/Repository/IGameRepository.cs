public interface IGameRepository
{
    T GetData<T>();

    bool TryGetData<T>(out T value);

    void SetData<T>(T value);

    public void SaveState();

    public void LoadState();
}