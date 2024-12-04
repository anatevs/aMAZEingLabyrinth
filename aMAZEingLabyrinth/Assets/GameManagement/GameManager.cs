using GamePipeline;
using VContainer.Unity;

namespace GameManagement
{
    public class GameManager : IPostStartable
    {
        private readonly TurnPipeline _turnPipeline;

        public GameManager(TurnPipeline turnPipeline)
        {
            _turnPipeline = turnPipeline;
        }

        void IPostStartable.PostStart()
        {
            //_turnPipeline.Run();
        }
    }
}