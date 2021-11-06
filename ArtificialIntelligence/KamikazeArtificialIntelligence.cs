using Domain;
using System;
using System.Linq;

namespace ArtificialIntelligence
{
    public class KamikazeArtificialIntelligence : IArtificialIntelligence
    {
        public Result AutoPlayNextMove(Game game)
        {
            if (game.IsGameOver)
            {
                return Result.Failure("Cannot auto play when the game is over.");
            }

            var validMoves = MovesEngine.CalculateValidMoves(game.Board, game.CurrentPlayer);
            if (validMoves.Entries.Count == 0)
            {
                return Result.Failure("Cannot auto play when there are no valid moves.");
            }

            var selectedMove = SelectMove(game, validMoves);
            return game.AttemptMove(selectedMove.FromPosition, selectedMove.ToPosition);
        }

        private static ValidMove SelectMove(Game game, ValidMoves validMoves)
        {
            validMoves = new ValidMoves(validMoves.Any(move => move.IsMandatory)
                ? validMoves.Where(move => move.IsMandatory)
                : validMoves);

            if (validMoves.Entries.Count == 1)
            {
                return validMoves.Entries.Single();
            }

            if (validMoves.Any(move => move.IsMandatory))
            {
                var randomIndex = new Random().Next(0, validMoves.Entries.Count - 1);
                return validMoves.Entries.ElementAt(randomIndex);
            }

            return SelectMoveThatPutsMeCloserToAdversaryPawn(game, validMoves);
        }

        private static ValidMove SelectMoveThatPutsMeCloserToAdversaryPawn(Game game, ValidMoves validMoves)
        {
            return validMoves
                .Select(move =>
                {
                    var adversaryPaws = game.Board.Cells.Where(cell => cell.Pawn is not null && cell.Pawn.Owner == game.CurrentPlayer.GetAdversary());
                    var distance = adversaryPaws
                        .Select(pawn => GetDistance(move.ToPosition, pawn.Position))
                        .OrderBy(distance => distance)
                        .First();

                    return (distance, move);
                })
                .OrderBy(entry => entry.distance)
                .Select(entry => entry.move)
                .First();
        }

        private static double GetDistance(Position fromPosition, Position toPosition)
        {
            return Math.Sqrt(
                Math.Pow(fromPosition.Column.Number - toPosition.Column.Number, 2) +
                Math.Pow(fromPosition.Row.Number - toPosition.Row.Number, 2));
        }
    }
}
