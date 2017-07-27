using System.Collections.Generic;
using GemSwipe.Generator.LittleStar;
using LittleStar;

namespace GemSwipe.Generator
{
    public class Solver
    {
        private readonly LittleStarEngine _littleStarEngine;

        public Solver()
        {
            var logger = new ConsoleLogger();
            _littleStarEngine = new LittleStarEngine(logger);
        }

        public IList<GemSwipeMove> Solve(GemSwipeEngine game)
        {
            return _littleStarEngine.Resolve(game, game.GetInitialState());
        }
    }
}
