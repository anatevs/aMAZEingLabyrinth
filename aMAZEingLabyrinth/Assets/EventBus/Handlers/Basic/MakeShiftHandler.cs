using DG.Tweening;
using GameCore;
using System;
using UnityEngine;

namespace EventBusNamespace
{
    public sealed class MakeShiftHandler : BaseHandler<MakeShiftEvent>
    {
        private readonly ShiftArrowsService _shiftArrowsService;
        private readonly CellsLabyrinth _cellsLabyrinth;
        private readonly PlayersList _players;

        private float _shiftDuration = 1f;
        private float _postShiftDuration = 2f;

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

            var shiftSequence = _cellsLabyrinth.PrepareShiftViews(_shiftDuration, out _);

            var postShiftSequence = DOTween.Sequence().Pause();

            MakePlayersShift((evnt.Row, evnt.Col), oppositeIndex, shiftParams.direction, shiftSequence, postShiftSequence);

            _shiftArrowsService.ChangeDisabledArrow(oppositeIndex);

            _shiftArrowsService.DisableAllArrows();


            var playableTween = _cellsLabyrinth.PreparePlayableSet(_postShiftDuration);

            postShiftSequence.Join(playableTween);


            EventBus.RaiseEvent(new MakeShiftVisualEvent(shiftSequence, postShiftSequence));
        }


        private void MakePlayersShift((int row, int col) shiftIndex,
            (int row, int col) oppositeIndex, Vector3Int shiftVector,
            Sequence shiftSequence, Sequence postShift)
        {
            var (X, Y) = LabyrinthMath.GetXYCenter(shiftIndex.row, shiftIndex.col);

            var (opX, opY) = LabyrinthMath.GetXYCenter(oppositeIndex.row, oppositeIndex.col);

            if (shiftVector.x == 0)
            {
                if (_players.IsPlayerAtPointWithX(X, out var players))
                {
                    foreach (var player in players)
                    {
                        shiftSequence.Join(player.PrepareViewShift(shiftVector, _shiftDuration));

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
                        shiftSequence.Join(player.PrepareViewShift(shiftVector, _shiftDuration));

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
        }
    }
}