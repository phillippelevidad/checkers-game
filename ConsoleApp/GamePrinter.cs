using Domain;
using System;
using System.Linq;

namespace ConsoleApp
{
    public static class GamePrinter
    {
        public static void Print(Game game)
        {
            PrintScore(Player.Player2, game.Score.Player2Score);
            Console.WriteLine();

            PrintBoard(game.Board, game.MovesHistory.LastOrDefault());

            Console.WriteLine();
            PrintScore(Player.Player1, game.Score.Player1Score);
        }

        private static void PrintScore(Player player, int score)
        {
            var pawn = GetPawn(player == Player.Player1 ? Player.Player2 : Player.Player1);
            var takenPawns = Enumerable.Range(1, score).Select(x => $"{pawn}");
            var print = $"   {score}: " + string.Join("", takenPawns);
            Console.WriteLine(print);
        }

        private static void PrintBoard(Board board, MoveExecution? lastMove)
        {
            var rows = board.Cells.GroupBy(cell => cell.Position.Row).Reverse();
            PrintColumnLetters();

            foreach (var row in rows)
            {
                PrintRowSeparator();
                Console.Write($" {row.Key} ");

                var isEven = row.Key.Number % 2 == 0;

                foreach (var column in row)
                {
                    var content = GetCellContent(column);

                    if (isEven)
                    {
                        PrintWhiteCell();
                        PrintBlackCell(column, lastMove);
                    }
                    else
                    {
                        PrintBlackCell(column, lastMove);
                        PrintWhiteCell();
                    }
                }

                Console.WriteLine($"| {row.Key}");
            }

            PrintRowSeparator();
            PrintColumnLetters();
        }

        private static void PrintColumnLetters()
        {
            var letters = "ABCDEFGH";
            var print = "   " + string.Join("", letters.Select(letter => $"  {letter} "));
            Console.WriteLine(print);
        }

        private static void PrintRowSeparator()
        {
            var parts = Enumerable.Range(1, 8).Select(x => "+---");
            var all = "   " + string.Join("", parts) + "+";
            Console.WriteLine(all);
        }

        private static void PrintWhiteCell()
        {
            Console.Write("|");
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.Write("   ");
            Console.BackgroundColor = ConsoleColor.Black;
        }

        private static void PrintBlackCell(BoardCell cell, MoveExecution? lastMove)
        {
            Console.Write("|");
            if (lastMove is not null)
            {
                if (lastMove.AllMovesIncludingFollowUps.Any(move =>
                    cell.Position == move.FromPosition ||
                    cell.Position == move.ToPosition))
                {
                    Console.BackgroundColor = ConsoleColor.DarkYellow;
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                else if (lastMove.AllMovesIncludingFollowUps.Any(move =>
                    move.PawnToBeTaken is not null &&
                    move.PawnToBeTaken.Position == cell.Position))
                {
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    Console.ForegroundColor = ConsoleColor.Black;
                }
            }

            var content = GetCellContent(cell);
            Console.Write($"{content}");

            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        private static string GetCellContent(BoardCell cell)
        {
            return cell.Pawn is null
                ? "   "
                : GetPawn(cell.Pawn.Owner, cell.Pawn.IsCrowned);
        }

        private static string GetPawn(Player player, bool isCrowned = false)
        {
            if (player == Player.Player1)
            {
                return isCrowned ? "<X>" : " X ";
            }
            else
            {
                return isCrowned ? "<O>" : " O ";
            }
        }
    }
}
