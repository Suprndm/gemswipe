using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GemSwipe.GameEngine;
using GemSwipe.GameEngine.Floors;
using GemSwipe.Generator.LittleStar;
using GemSwipe.Models;
using LittleStar;

namespace GemSwipe.Generator
{
    class Program
    {
        static void Main(string[] args)
        {
            var game = new GemSwipeEngine(new Board("3 0 0 3 2-2 0 0 0 2-0 1 0 2 1-1 0 2 2 1-3 0 2 0 2"));
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
    }
}
