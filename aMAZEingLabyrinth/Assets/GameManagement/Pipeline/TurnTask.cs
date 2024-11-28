using EventBusNamespace;

namespace GamePipeline
{
    public sealed class TurnTask : Task
    {
        //private readonly HeroListService _heroListService;
        //private readonly CurrentTeamData _teamData;
        private readonly EventBus _eventBus;
        //private readonly GameManager _gameManager;

        private bool _isGameFinished = false;

        public TurnTask(EventBus eventBus)//, HeroListService heroListService, CurrentTeamData teamData, GameManager gameManager)
        {
            _eventBus = eventBus;
            //_heroListService = heroListService;
            //_teamData = teamData;
            //_gameManager = gameManager;
        }

        protected override void OnRun()
        {
            //_heroListService.OnClickEntity += OnHeroClicked;
            //_gameManager.OnGameFinished += OnGameFinish;
        }

        protected override void OnFinished()
        {
            //_heroListService.OnClickEntity -= OnHeroClicked;
            //_gameManager.OnGameFinished -= OnGameFinish;
        }

        //private void OnHeroClicked(HeroEntity clickedEntity)
        //{
        //    Team team = clickedEntity.Get<TeamInfoComponent>().team;
        //    if (team != _teamData.Enemy)
        //    {
        //        return;
        //    }
        //    else
        //    {
        //        HeroEntity playerHero = _heroListService.GetCurrentActive(_teamData.Player);
        //        _eventBus.RaiseEvent(new AttackEvent(clickedEntity, playerHero));

        //        if (!_isGameFinished)
        //        {
        //            _eventBus.RaiseEvent(new NextMoveEvent(playerHero));
        //        }
        //    }

        //    Finish();
        //}

        private void OnGameFinish()
        {
            _isGameFinished = true;
            Finish();
        }
    }
}