using DG.Tweening;
using GameCore;
using UnityEngine;

namespace EventBusNamespace
{
    public sealed class MakeShiftHandler : BaseHandler<MakeShiftEvent>
    {
        private readonly ShiftArrowsService _shiftArrowsService;
        private readonly CellsLabyrinth _cellsLabyrinth;
        private readonly PlayersList _players;

        private float _postShiftDuration;

        public MakeShiftHandler(EventBus eventBus,
            ShiftArrowsService shiftArrowsService,
            CellsLabyrinth cellsLabyrinth, PlayersList players) : base(eventBus)
        {
            _shiftArrowsService = shiftArrowsService;
            _cellsLabyrinth = cellsLabyrinth;
            _players = players;
        }

        protected override void RaiseEvent(MakeShiftEvent evnt)
        {
            _cellsLabyrinth.MakeShift(evnt.Row, evnt.Col, out var shiftParams);

            var oppositeIndex = (shiftParams.row, shiftParams.col);

            var postShiftSequence = DOTween.Sequence().Pause();

            var plViewsShift = MakePlayersShift((evnt.Row, evnt.Col), oppositeIndex, shiftParams.direction, postShiftSequence);

            var playableTween = _cellsLabyrinth.PreparePlayableSet();

            _postShiftDuration = playableTween.Duration();

            postShiftSequence.Join(playableTween);


            _shiftArrowsService.ChangeDisabledArrow(oppositeIndex);

            _shiftArrowsService.DisableAllArrows();


            EventBus.RaiseEvent(new MakeShiftVisualEvent(_cellsLabyrinth, plViewsShift, postShiftSequence));
        }


        private Sequence MakePlayersShift((int row, int col) shiftIndex,
            (int row, int col) oppositeIndex, Vector3Int shiftVector,
            Sequence postShift)
        {
            var (X, Y) = LabyrinthMath.GetXYCenter(shiftIndex.row, shiftIndex.col);

            var (opX, opY) = LabyrinthMath.GetXYCenter(oppositeIndex.row, oppositeIndex.col);

            var playersViewShift = DOTween.Sequence().Pause();

            if (shiftVector.x == 0)
            {
                if (_players.IsPlayerAtPointWithX(X, out var players))
                {
                    foreach (var player in players)
                    {
                        playersViewShift.Join(player.PrepareViewShift(shiftVector, _cellsLabyrinth.ShiftDuration));

                        if (player.Coordinate.Y != opY)
                        {
                            player.ShiftCoordinate(shiftVector);
                        }
                        else
                        {
                            player.SetCoordinateToDefaultPos();

                            var plDefaultTween = player.PrepareViewSetToDefault(_postShiftDuration);

                            postShift.Join(plDefaultTween);
                        }
                    }
                }
            }
            else if (shiftVector.y == 0)
            {
                if (_players.IsPlayerAtPointWithY(Y, out var players))
                {
                    foreach (var player in players)
                    {
                        playersViewShift.Join(player.PrepareViewShift(shiftVector, _cellsLabyrinth.ShiftDuration));

                        if (player.Coordinate.X != opX)
                        {
                            player.ShiftCoordinate(shiftVector);
                        }
                        else
                        {
                            player.SetCoordinateToDefaultPos();

                            var plDefaultTween = player.PrepareViewSetToDefault(_postShiftDuration);

                            postShift.Join(plDefaultTween);
                        }
                    }
                }
            }

            return playersViewShift;
        }
    }
}