using ArtificialIntelligence;
using Domain;
using System;
using System.Linq;
using System.Threading;

namespace ConsoleApp
{
    internal class Program
    {
        private static void Main()
        {
            Game game = new();
            IArtificialIntelligence ai = new KamikazeArtificialIntelligence();

            while (true)
            {
                var result = ai.AutoPlayNextMove(game);
                if (result.IsFailure)
                {
                    break;
                }

                PrintGame(game);
                Thread.Sleep(750);
            }
        }

        private static void PrintGame(Game game)
        {
            Console.Clear();
            PrintTitle();
            GamePrinter.Print(game);
            PrintRecentHistory(game);
        }

        private static void PrintTitle()
        {
            Console.WriteLine("Auto Checkers Player");
            Console.WriteLine("====================");
            Console.WriteLine();
            Console.WriteLine();
        }

        private static void PrintRecentHistory(Game game)
        {
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine($"{game.MovesHistory.Count} moves:");

            foreach (var move in game.MovesHistory.Reverse().Take(5))
            {
                Console.WriteLine(move.Move);
            }
        }
    }
}
