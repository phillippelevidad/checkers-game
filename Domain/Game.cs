using System.Collections.Generic;
using System.Linq;

namespace Domain
{
    public class Game
    {
        public const int PawnsPerPlayer = 12;

        private readonly List<MoveExcecution> movesHistory = new();

        public Game()
        {
            Board = new Board();
            LayOutPawns();
            CurrentPlayer = Player.Player1;
            Score = new Score(0, 0);
        }

        public Board Board { get; }

        public Player CurrentPlayer { get; private set; }

        public IReadOnlyList<MoveExcecution> MovesHistory => movesHistory.AsReadOnly();

        public Score Score { get; private set; }

        public bool IsGameOver => Score.Player1Wins || Score.Player2Wins;

        public Result<MoveExcecution> AttemptMove(Position fromPosition, Position toPosition)
        {
            var moveCalculation = MovesEngine
                .CalculateValidMoves(Board, CurrentPlayer)
                .Find(fromPosition, toPosition);

            if (moveCalculation.IsFailure)
            {
                return Result<MoveExcecution>.Failure(moveCalculation.Error!);
            }

            var execution = ExecuteMove(moveCalculation.Value!);
            Score = CalculateScore();

            if (!IsGameOver)
            {
                ToggleCurrentPlayer();
            }

            return execution;
        }

        private MoveExcecution ExecuteMove(ValidMove move)
        {
            var currentMove = move;
            while (currentMove is not null)
            {
                var pawn = Board.GetCell(currentMove.FromPosition).Free();
                Board.GetCell(currentMove.ToPosition).PlacePawn(pawn.Value!);

                if (currentMove.PawnToBeTaken is not null)
                {
                    Board.GetCell(currentMove.PawnToBeTaken!.Position).Free();
                }

                if (Board.GetCell(currentMove.ToPosition).IsAdversaryKingsRow(move.Player))
                {
                    pawn.Value!.Crown();
                }

                currentMove = currentMove.FollowUpMove;
            }

            var execution = new MoveExcecution(move.Player, move);
            movesHistory.Add(execution);

            return execution;
        }

        private Score CalculateScore()
        {
            int CountPawns(Player player)
            {
                return Board.Cells.Count(cell => cell.HasPawn && cell.Pawn!.Owner == player);
            }

            var player1Score = PawnsPerPlayer - CountPawns(Player.Player2);
            var player2Score = PawnsPerPlayer - CountPawns(Player.Player1);

            return new Score(player1Score, player2Score);
        }

        private void LayOutPawns()
        {

            var position = Board.GetFirstCell();
            for (int i = 0; i < PawnsPerPlayer; i++)
            {
                position.PlacePawn(new Pawn(Player.Player1));
                position = Board.GetNextCell(position.Position).Value!;
            }

            position = Board.GetLastCell();
            for (int i = 0; i < PawnsPerPlayer; i++)
            {
                position.PlacePawn(new Pawn(Player.Player2));
                position = Board.GetPreviousCell(position.Position).Value!;
            }
        }

        private void ToggleCurrentPlayer()
        {
            CurrentPlayer = CurrentPlayer == Player.Player1
                ? Player.Player2
                : Player.Player1;
        }
    }
}
