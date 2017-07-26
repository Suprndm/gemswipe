using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GemSwipe.Generator.LittleStar;
using GemSwipe.Models;
using LittleStar;

namespace GemSwipe.Generator
{
    class Program
    {
        static void Main(string[] args)
        {
            var game = new GemSwipeEngine(new Board("1 1 2 1-0 1 2 1-0 0 2 2-1 0 1 1"));
            var logger = new ConsoleLogger();
            //logger.IsEnabled = false;
            var littleStarEngine = new LittleStarEngine(logger);

            var moves = littleStarEngine.Resolve(game, game.GetInitialState());
            if (moves.Count == 0) Console.Write("Impossible");
            foreach (var move in moves)
            {
                Console.Write(move.Direction.ToString() + " ");
            }
            Console.ReadKey();
        }
    }
}
