using GameCore;
using System;

namespace EventBusNamespace
{
    public sealed class MakeShiftHandler : BaseHandler<MakeShiftEvent>
    {
        private readonly ShiftArrowsService _shiftArrowsService;
        private readonly CellsLabyrinth _cellsLabyrinth;
        private readonly PlayersList _players;

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
            _cellsLabyrinth.MakeShift(evnt.Row, evnt.Col, out var oppositeIndex);

            MakePlayersShift(evnt.Row, evnt.Col, oppositeIndex);

            _shiftArrowsService.ChangeDisabledArrow(oppositeIndex);

            _shiftArrowsService.DisableAllArrows();
        }


        private void MakePlayersShift(int Row, int Col, (int row, int col) oppositeIndex )
        {
            var (X, Y) = LabyrinthMath.GetXYCenter(Row, Col);

            var (opX, opY) = LabyrinthMath.GetXYCenter(oppositeIndex.row, oppositeIndex.col);

            if (X == opX)
            {
                if (_players.IsPlayerAtPointWithX(X, out var players))
                {
                    var direction = Math.Sign(opY - Y);
                    var shift = direction * LabyrinthMath.CellSize;

                    foreach (var player in players)
                    {
                        if (player.Coordinate.Y != opY)
                        {
                            player.SetCoordinate((player.Coordinate.X, player.Coordinate.Y + shift));
                        }
                        else
                        {
                            player.SetToDefaultPos();
                        }
                    }
                }
            }
            else if (Y == opY)
            {
                if (_players.IsPlayerAtPointWithY(Y, out var players))
                {
                    var direction = Math.Sign(opX - X);
                    var shift = direction * LabyrinthMath.CellSize;

                    foreach (var player in players)
                    {
                        if (player.Coordinate.X != opX)
                        {
                            player.SetCoordinate((player.Coordinate.X + shift, player.Coordinate.Y));
                        }
                        else
                        {
                            player.SetToDefaultPos();
                        }
                    }
                }
            }
        }
    }
}