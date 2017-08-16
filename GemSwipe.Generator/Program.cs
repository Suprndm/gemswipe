using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GemSwipe.BoardSolver;
using GemSwipe.BoardSolver.LittleStar;
using GemSwipe.GameEngine;
using GemSwipe.GameEngine.Floors;
using GemSwipe.Models;
using LittleStar;

namespace GemSwipe.Generator
{
    class Program
    {
        static void Main(string[] args)
        {
            SolveLevel();
        }

        static void SolveLevel()
        {
            var game = new GemSwipeEngine(new Board("0 0 3 0-0 1 1 1-2 0 0 2-1 3 0 0"));
            var solver = new Solver();
            var moves = solver.Solve(game);


            if (moves.Count == 0) Console.Write("Impossible");
            Console.WriteLine($"Résolu en {moves.Count} coups");

            foreach (var move in moves)
            {
                Console.Write(move.Direction.ToString() + " ");
            }

            Console.ReadKey();
        }

        static void GenerateLevels()
        {
            var generator = new Generator();
            var solver = new Solver();
            int count = 0;
            ConcurrentDictionary<int, ConcurrentBag<string>> boards = new ConcurrentDictionary<int, ConcurrentBag<string>>();
            for (int i = 1; i < 20; i++)
            {
                boards.TryAdd(i, new ConcurrentBag<string>());
            }
            Parallel.ForEach(Enumerable.Range(0, 100), new ParallelOptions { MaxDegreeOfParallelism = 4 }, (i) =>
            {
                count++;
                var board = generator.GenerateRandomLevel(4, 4);
                var game = new GemSwipeEngine(new Board(board));

                var moves = solver.Solve(game);
                if (moves.Count > 0)
                {
                    if (!boards[moves.Count].Contains(board))
                        boards[moves.Count].Add(board);
                }

                if (count % 10 == 0)
                {
                    Console.Clear();
                    foreach (var boardsKey in boards.Keys)
                    {
                        Console.WriteLine($"Generated level {boardsKey} - {boards[boardsKey].Count} boards");
                    }
                }
            });


            Console.ReadKey();
        }
    }
}
