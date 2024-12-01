using UnityEngine;
using VContainer;

namespace GamePipeline
{
    public sealed class TurnPipelineRunner : MonoBehaviour
    {
        private bool _isRunOnFinish = true;

        private TurnPipeline _turnPipeline;
        //private GameManager _gameManager;

        [Inject]
        public void Construct(TurnPipeline turnPipeline)//, GameManager gameManager)
        {
            _turnPipeline = turnPipeline;
            //_gameManager = gameManager;
        }

        private void OnEnable()
        {
            _turnPipeline.OnFinished += RunAgain;
            //_gameManager.OnGameFinished += FinishGame;
        }

        private void Start()
        {
            _turnPipeline.Run();
        }

        private void OnDisable()
        {
            _turnPipeline.OnFinished -= RunAgain;
            //_gameManager.OnGameFinished -= FinishGame;
        }

        private void RunAgain()
        {
            if (_isRunOnFinish)
            {
                _turnPipeline.Run();
            }
        }

        private void FinishGame()
        {
            _isRunOnFinish = false;
        }
    }
}