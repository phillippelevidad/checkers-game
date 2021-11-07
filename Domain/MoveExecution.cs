using System.Collections.Generic;
using System.Linq;

namespace Domain
{
    public class MoveExecution
    {
        public MoveExecution(Player player, ValidMove move)
        {
            Player = player;
            Move = move;
            AllMovesIncludingFollowUps = BuildAllMovesIncludingFollowUps(move);
            PawnsTaken = BuildPawnsTaken(AllMovesIncludingFollowUps);
        }

        public Player Player { get; }

        public ValidMove Move { get; }

        public IReadOnlyList<ValidMove> AllMovesIncludingFollowUps { get; }

        public IReadOnlyList<Pawn> PawnsTaken { get; }

        private static IReadOnlyList<ValidMove> BuildAllMovesIncludingFollowUps(ValidMove startingMove)
        {
            var allMoves = new List<ValidMove>();
            var currentMove = startingMove;

            while (currentMove is not null)
            {
                allMoves.Add(currentMove);
                currentMove = currentMove.FollowUpMove;
            }

            return allMoves.AsReadOnly();
        }

        private static IReadOnlyList<Pawn> BuildPawnsTaken(IEnumerable<ValidMove> allMovesIncludingFollowUps)
        {
            return allMovesIncludingFollowUps
                .Where(move => move.PawnToBeTaken is not null)
                .Select(move => move.PawnToBeTaken!.Pawn!)
                .ToList().AsReadOnly();
        }
    }
}
