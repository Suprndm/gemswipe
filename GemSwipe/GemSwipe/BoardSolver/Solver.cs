using System.Collections.Generic;
using GemSwipe.BoardSolver.LittleStar;
using GemSwipe.BoardSolver.LittleStar.Engine;
using GemSwipe.Services;

namespace GemSwipe.BoardSolver
{
    public class Solver
    {
        private readonly LittleStarEngine _littleStarEngine;

        public Solver()
        {
            var logger = new LittleStarLogger();
            _littleStarEngine = new LittleStarEngine(logger);
        }

        public IList<GemSwipeMove> Solve(GemSwipeEngine game)
        {
            return _littleStarEngine.Resolve(game, game.GetInitialState());
        }
    }
}
