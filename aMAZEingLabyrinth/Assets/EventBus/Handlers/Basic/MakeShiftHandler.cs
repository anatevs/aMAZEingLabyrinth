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

        private float _shiftDuration = 0.5f;

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


            MakePlayersShift((evnt.Row, evnt.Col), oppositeIndex, shiftParams.direction, shiftSequence);

            _shiftArrowsService.ChangeDisabledArrow(oppositeIndex);

            _shiftArrowsService.DisableAllArrows();


            EventBus.RaiseEvent(new MakeShiftVisualEvent(shiftSequence));
        }


        private void MakePlayersShift((int row, int col) shiftIndex,
            (int row, int col) oppositeIndex, Vector3Int shiftVector,
            Sequence shiftSequence)
        {
            var (X, Y) = LabyrinthMath.GetXYCenter(shiftIndex.row, shiftIndex.col);

            var (opX, opY) = LabyrinthMath.GetXYCenter(oppositeIndex.row, oppositeIndex.col);

            //if (X == opX)
            if (shiftVector.x == 0)
            {
                if (_players.IsPlayerAtPointWithX(X, out var players))
                {
                    //var direction = Math.Sign(opY - Y);
                    //var shift = direction * LabyrinthMath.CellSize;

                    foreach (var player in players)
                    {
                        //visual shift anyway
                        shiftSequence.Join(player.PrepareViewShift(shiftVector, _shiftDuration));

                        if (player.Coordinate.Y != opY)
                        {
                            player.ShiftCoordinate(shiftVector);

                            //player.SetCoordinateAndView((player.Coordinate.X, player.Coordinate.Y + shiftVector.y));
                        }
                        else
                        {
                            //player.SetToDefaultPos();

                            player.SetCoordinateToDefaultPos();

                            //raise visual event ReturnPlayerToDefault
                        }
                    }
                }
            }
            else if (shiftVector.y == 0)//(Y == opY)
            {
                if (_players.IsPlayerAtPointWithY(Y, out var players))
                {
                    //var direction = Math.Sign(opX - X);
                    //var shift = direction * LabyrinthMath.CellSize;

                    foreach (var player in players)
                    {
                        //visual shift anyway
                        shiftSequence.Join(player.PrepareViewShift(shiftVector, _shiftDuration));

                        if (player.Coordinate.X != opX)
                        {
                            player.ShiftCoordinate(shiftVector);

                            //player.SetCoordinateAndView((player.Coordinate.X + shiftVector.x, player.Coordinate.Y));
                        }
                        else
                        {
                            //player.SetToDefaultPos();


                            player.SetCoordinateToDefaultPos();

                            //raise visual event ReturnPlayerToDefault
                        }
                    }
                }
            }
        }
    }
}