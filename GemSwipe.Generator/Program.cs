using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GemSwipe.BoardSolver;
using GemSwipe.BoardSolver.LittleStar;
using GemSwipe.Game.Entities;
using LittleStar;
using Newtonsoft.Json;

namespace GemSwipe.Generator
{
    class Program
    {
        static void Main(string[] args)
        {
            // GenerateLevels();
            SolveLevel();
        }

        static void SolveLevel()
        {
            var game = new GemSwipeEngine(new Board("0 9 0 0 0 0-0 0 0 0 0 2-9 0 0 0 0 3-0 0 0 0 0 2-0 0 0 0 0 0-0 0 0 0 0 0"));
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
            ConcurrentBag<string> generatedBoards = new ConcurrentBag<string>();
            ConcurrentDictionary<int, ConcurrentBag<string>> sortedBoards = new ConcurrentDictionary<int, ConcurrentBag<string>>();
            for (int i = 1; i < 20; i++)
            {
                sortedBoards.TryAdd(i, new ConcurrentBag<string>());
            }

            Parallel.ForEach(Enumerable.Range(0, 100000), new ParallelOptions { MaxDegreeOfParallelism = 8 }, (i) =>
            {
                count++;
                var board = generator.GenerateRandomLevel(4, 4);
                if (!generatedBoards.Contains(board))
                {
                    var game = new GemSwipeEngine(new Board(board));

                    var moves = solver.Solve(game);
                    generatedBoards.Add(board);

                    if (moves.Count > 0)
                    {
                        sortedBoards[moves.Count].Add(board);
                    }
                }

                if (count % 10 == 0)
                {
                    Console.Clear();
                    foreach (var boardsKey in sortedBoards.Keys)
                    {
                        Console.WriteLine($"Generated level {boardsKey} - {sortedBoards[boardsKey].Count} boards");
                    }
                }
                var save = false;
                if (save)
                {
                    string json = JsonConvert.SerializeObject(sortedBoards, Formatting.Indented);
                    File.WriteAllText("boards.json", json);
                }
            });

            string finalJson = JsonConvert.SerializeObject(sortedBoards, Formatting.Indented);
            File.WriteAllText("boards.json", finalJson);
            Console.ReadKey();
        }
    }
}
