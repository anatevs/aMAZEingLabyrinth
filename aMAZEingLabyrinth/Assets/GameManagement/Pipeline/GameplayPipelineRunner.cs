using UnityEngine;
using VContainer;

namespace GamePipeline
{
    public sealed class GameplayPipelineRunner : MonoBehaviour
    {
        private bool _isRunOnFinish = true;

        private GameplayPipeline _gameplayPipeline;
        //private GameManager _gameManager;

        [Inject]
        public void Construct(GameplayPipeline turnPipeline)//, GameManager gameManager)
        {
            _gameplayPipeline = turnPipeline;
            //_gameManager = gameManager;
        }

        private void OnEnable()
        {
            _gameplayPipeline.OnFinished += RunAgain;
            //_gameManager.OnGameFinished += FinishGame;
        }

        private void Start()
        {
            _gameplayPipeline.Run();
        }

        private void OnDisable()
        {
            _gameplayPipeline.OnFinished -= RunAgain;
            //_gameManager.OnGameFinished -= FinishGame;
        }

        private void RunAgain()
        {
            if (_isRunOnFinish)
            {
                _gameplayPipeline.Run();
            }
        }

        private void FinishGame()
        {
            _isRunOnFinish = false;
        }
    }
}