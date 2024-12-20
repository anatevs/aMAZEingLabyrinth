using GameCore;
using System;
using System.Collections;
using UnityEngine;
using static Codice.Client.Common.WebApi.WebApiEndpoints;

namespace GamePipeline
{
    public class MakeShiftVisualTask : Task
    {
        private readonly CellsLabyrinth _cellsLabyrinth;
        private readonly PlayersList _players;
        private readonly (int row, int col) _arrowIndex;

        public MakeShiftVisualTask(CellsLabyrinth cellsLabyrinth,
            PlayersList players,
            (int row, int col) arrowIndex)
        {
            _cellsLabyrinth = cellsLabyrinth;
            _players = players;
            _arrowIndex = arrowIndex;
        }

        protected override void OnRun()
        {
            _cellsLabyrinth.MakeShift(_arrowIndex.row, _arrowIndex.row, out var oppositeIndex);

            MakePlayersShift(_arrowIndex.row, _arrowIndex.row, oppositeIndex);
        }


        private void MakePlayersShift(int Row, int Col, (int row, int col) oppositeIndex)
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