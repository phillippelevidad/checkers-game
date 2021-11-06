using System.Collections.Generic;
using System.Linq;

namespace Domain
{
    public static class MovesEngine
    {
        public static ValidMoves CalculateValidMoves(Board board, Player asPlayer)
        {
            var moves = ListCellsWithPlayerPawns(board, asPlayer)
                .SelectMany(cell => CalculateValidMovesFromPosition(board, cell.Pawn!.Owner, cell.Position, cell.Pawn.IsCrowned))
                .ToList();

            return new ValidMoves(moves);
        }

        private static List<BoardCell> ListCellsWithPlayerPawns(Board board, Player player)
        {
            return board.Cells
                .Where(cell => cell.HasPawn && cell.Pawn!.Owner == player)
                .ToList();
        }

        private static List<ValidMove> CalculateValidMovesFromPosition(Board board, Player asPlayer, Position position, bool ignoreDirection, int recursionLevel = 0)
        {
            const int MaxRecursionLevel = 3;

            if (recursionLevel == MaxRecursionLevel)
            {
                return new List<ValidMove>(0);
            }

            var moves = new List<ValidMove>();

            foreach (var next in board.ListNeighboringCells(position, asPlayer))
            {
                if (next.Cell.IsFree)
                {
                    if (ignoreDirection || next.Direction == MoveDirection.Advancing)
                    {
                        moves.Add(ValidMove.SimpleMove(asPlayer, position, next.Cell.Position));
                    }
                }
                else if (next.Cell.Pawn!.Owner == asPlayer.GetAdversary() && next.NextCell is not null && next.NextCell.IsFree)
                {
                    var fromPosition = position;
                    var toPosition = next.NextCell.Position;

                    var followUpMove = CalculateValidMovesFromPosition(board, asPlayer, next.NextCell.Position, true, recursionLevel + 1)
                        .Where(move => move.IsMandatory)
                        .Where(move =>
                        {
                            var wouldBackTrack = (move.FromPosition, move.ToPosition) == (toPosition, fromPosition);
                            return !wouldBackTrack;
                        })
                        .FirstOrDefault();

                    moves.Add(ValidMove.JumpMove(asPlayer, fromPosition, toPosition, next.Cell, followUpMove));
                }
            }

            return moves;
        }
    }
}
